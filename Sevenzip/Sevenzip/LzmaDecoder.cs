// LzmaDecoder.cs

using System;

//namespace SevenZip
//{
    //using RangeCoder;

    public class LZMADecoder : ICoder, ISetDecoderProperties // ,System.IO.Stream
	{
		class LenDecoder
		{
			RangeCoderBitDecoder m_Choice = new RangeCoderBitDecoder();
			RangeCoderBitDecoder m_Choice2 = new RangeCoderBitDecoder();
			RangeCoderBitTreeDecoder[] m_LowCoder = new RangeCoderBitTreeDecoder[LZMABase.kNumPosStatesMax];
			RangeCoderBitTreeDecoder[] m_MidCoder = new RangeCoderBitTreeDecoder[LZMABase.kNumPosStatesMax];
			RangeCoderBitTreeDecoder m_HighCoder = new RangeCoderBitTreeDecoder(LZMABase.kNumHighLenBits);
			uint m_NumPosStates = 0;

			public void Create(uint numPosStates)
			{
				for (uint posState = m_NumPosStates; posState < numPosStates; posState++)
				{
					m_LowCoder[posState] = new RangeCoderBitTreeDecoder(LZMABase.kNumLowLenBits);
					m_MidCoder[posState] = new RangeCoderBitTreeDecoder(LZMABase.kNumMidLenBits);
				}
				m_NumPosStates = numPosStates;
			}

			public void Init()
			{
				m_Choice.Init();
				for (uint posState = 0; posState < m_NumPosStates; posState++)
				{
					m_LowCoder[posState].Init();
					m_MidCoder[posState].Init();
				}
				m_Choice2.Init();
				m_HighCoder.Init();
			}

			public uint Decode(RangeCoderDecoder rangeDecoder, uint posState)
			{
				if (m_Choice.Decode(rangeDecoder) == 0)
					return m_LowCoder[posState].Decode(rangeDecoder);
				else
				{
					uint symbol = LZMABase.kNumLowLenSymbols;
					if (m_Choice2.Decode(rangeDecoder) == 0)
						symbol += m_MidCoder[posState].Decode(rangeDecoder);
					else
					{
						symbol += LZMABase.kNumMidLenSymbols;
						symbol += m_HighCoder.Decode(rangeDecoder);
					}
					return symbol;
				}
			}
		}

		class LiteralDecoder
		{
			struct Decoder2
			{
				RangeCoderBitDecoder[] m_Decoders;
				public void Create() { m_Decoders = new RangeCoderBitDecoder[0x300]; }
				public void Init() { for (int i = 0; i < 0x300; i++) m_Decoders[i].Init(); }

				public byte DecodeNormal(RangeCoderDecoder rangeDecoder)
				{
					uint symbol = 1;
					do
						symbol = (symbol << 1) | m_Decoders[symbol].Decode(rangeDecoder);
					while (symbol < 0x100);
					return (byte)symbol;
				}

				public byte DecodeWithMatchByte(RangeCoderDecoder rangeDecoder, byte matchByte)
				{
					uint symbol = 1;
					do
					{
						uint matchBit = (uint)(matchByte >> 7) & 1;
						matchByte <<= 1;
						uint bit = m_Decoders[((1 + matchBit) << 8) + symbol].Decode(rangeDecoder);
						symbol = (symbol << 1) | bit;
						if (matchBit != bit)
						{
							while (symbol < 0x100)
								symbol = (symbol << 1) | m_Decoders[symbol].Decode(rangeDecoder);
							break;
						}
					}
					while (symbol < 0x100);
					return (byte)symbol;
				}
			}

			Decoder2[] m_Coders;
			int m_NumPrevBits;
			int m_NumPosBits;
			uint m_PosMask;

			public void Create(int numPosBits, int numPrevBits)
			{
				if (m_Coders != null && m_NumPrevBits == numPrevBits &&
					m_NumPosBits == numPosBits)
					return;
				m_NumPosBits = numPosBits;
				m_PosMask = ((uint)1 << numPosBits) - 1;
				m_NumPrevBits = numPrevBits;
				uint numStates = (uint)1 << (m_NumPrevBits + m_NumPosBits);
				m_Coders = new Decoder2[numStates];
				for (uint i = 0; i < numStates; i++)
					m_Coders[i].Create();
			}

			public void Init()
			{
				uint numStates = (uint)1 << (m_NumPrevBits + m_NumPosBits);
               
                for (uint i = 0; i < numStates; i++)
                {
                  
                    m_Coders[i].Init();

                }
			}

			uint GetState(uint pos, byte prevByte)
			{ return ((pos & m_PosMask) << m_NumPrevBits) + (uint)(prevByte >> (8 - m_NumPrevBits)); }

			public byte DecodeNormal(RangeCoderDecoder rangeDecoder, uint pos, byte prevByte)
			{ return m_Coders[GetState(pos, prevByte)].DecodeNormal(rangeDecoder); }

			public byte DecodeWithMatchByte(RangeCoderDecoder rangeDecoder, uint pos, byte prevByte, byte matchByte)
			{ return m_Coders[GetState(pos, prevByte)].DecodeWithMatchByte(rangeDecoder, matchByte); }
		};

		LZOutWindow m_OutWindow = new LZOutWindow();
		RangeCoderDecoder m_RangeDecoder = new RangeCoderDecoder();

		RangeCoderBitDecoder[] m_IsMatchDecoders = new RangeCoderBitDecoder[LZMABase.kNumStates << LZMABase.kNumPosStatesBitsMax];
		RangeCoderBitDecoder[] m_IsRepDecoders = new RangeCoderBitDecoder[LZMABase.kNumStates];
		RangeCoderBitDecoder[] m_IsRepG0Decoders = new RangeCoderBitDecoder[LZMABase.kNumStates];
		RangeCoderBitDecoder[] m_IsRepG1Decoders = new RangeCoderBitDecoder[LZMABase.kNumStates];
		RangeCoderBitDecoder[] m_IsRepG2Decoders = new RangeCoderBitDecoder[LZMABase.kNumStates];
		RangeCoderBitDecoder[] m_IsRep0LongDecoders = new RangeCoderBitDecoder[LZMABase.kNumStates << LZMABase.kNumPosStatesBitsMax];

		RangeCoderBitTreeDecoder[] m_PosSlotDecoder = new RangeCoderBitTreeDecoder[LZMABase.kNumLenToPosStates];
		RangeCoderBitDecoder[] m_PosDecoders = new RangeCoderBitDecoder[LZMABase.kNumFullDistances - LZMABase.kEndPosModelIndex];

		RangeCoderBitTreeDecoder m_PosAlignDecoder = new RangeCoderBitTreeDecoder(LZMABase.kNumAlignBits);

		LenDecoder m_LenDecoder = new LenDecoder();
		LenDecoder m_RepLenDecoder = new LenDecoder();

		LiteralDecoder m_LiteralDecoder = new LiteralDecoder();

		uint m_DictionarySize;
		uint m_DictionarySizeCheck;

		uint m_PosStateMask;

		public LZMADecoder()
		{
			m_DictionarySize = 0xFFFFFFFF;
			for (int i = 0; i < LZMABase.kNumLenToPosStates; i++)
				m_PosSlotDecoder[i] = new RangeCoderBitTreeDecoder(LZMABase.kNumPosSlotBits);
		}

		void SetDictionarySize(uint dictionarySize)
		{
			if (m_DictionarySize != dictionarySize)
			{
				m_DictionarySize = dictionarySize;
				m_DictionarySizeCheck = Math.Max(m_DictionarySize, 1);
				uint blockSize = Math.Max(m_DictionarySizeCheck, (1 << 12));
				m_OutWindow.Create(blockSize);
			}
		}

		void SetLiteralProperties(int lp, int lc)
		{
			if (lp > 8)
				throw new InvalidParamException();
			if (lc > 8)
				throw new InvalidParamException();
			m_LiteralDecoder.Create(lp, lc);
		}

		void SetPosBitsProperties(int pb)
		{
			if (pb > LZMABase.kNumPosStatesBitsMax)
				throw new InvalidParamException();
			uint numPosStates = (uint)1 << pb;
			m_LenDecoder.Create(numPosStates);
			m_RepLenDecoder.Create(numPosStates);
			m_PosStateMask = numPosStates - 1;
		}

		void Init(System.IO.Stream inStream, System.IO.Stream outStream)
		{
			m_RangeDecoder.Init(inStream);
			m_OutWindow.Init(outStream);

			uint i;
			for (i = 0; i < LZMABase.kNumStates; i++)
			{
				for (uint j = 0; j <= m_PosStateMask; j++)
				{
					uint index = (i << LZMABase.kNumPosStatesBitsMax) + j;
					m_IsMatchDecoders[index].Init();
					m_IsRep0LongDecoders[index].Init();
				}
				m_IsRepDecoders[i].Init();
				m_IsRepG0Decoders[i].Init();
				m_IsRepG1Decoders[i].Init();
				m_IsRepG2Decoders[i].Init();
			}

			m_LiteralDecoder.Init();
			for (i = 0; i < LZMABase.kNumLenToPosStates; i++)
				m_PosSlotDecoder[i].Init();
			// m_PosSpecDecoder.Init();
			for (i = 0; i < LZMABase.kNumFullDistances - LZMABase.kEndPosModelIndex; i++)
				m_PosDecoders[i].Init();

			m_LenDecoder.Init();
			m_RepLenDecoder.Init();
			m_PosAlignDecoder.Init();
		}

		public void Code(System.IO.Stream inStream, System.IO.Stream outStream,
			Int64 inSize, Int64 outSize, ICodeProgress progress)
		{
			Init(inStream, outStream);

			LZMABase.State state = new LZMABase.State();
			state.Init();
			uint rep0 = 0, rep1 = 0, rep2 = 0, rep3 = 0;

			UInt64 nowPos64 = 0;
			UInt64 outSize64 = (UInt64)outSize;
			if (nowPos64 < outSize64)
			{
				if (m_IsMatchDecoders[state.Index << LZMABase.kNumPosStatesBitsMax].Decode(m_RangeDecoder) != 0)
					throw new DataErrorException();
				state.UpdateChar();
				byte b = m_LiteralDecoder.DecodeNormal(m_RangeDecoder, 0, 0);
				m_OutWindow.PutByte(b);
				nowPos64++;
			}
			while (nowPos64 < outSize64)
			{
				// UInt64 next = Math.Min(nowPos64 + (1 << 18), outSize64);
					// while(nowPos64 < next)
				{
					uint posState = (uint)nowPos64 & m_PosStateMask;
					if (m_IsMatchDecoders[(state.Index << LZMABase.kNumPosStatesBitsMax) + posState].Decode(m_RangeDecoder) == 0)
					{
						byte b;
						byte prevByte = m_OutWindow.GetByte(0);
						if (!state.IsCharState())
							b = m_LiteralDecoder.DecodeWithMatchByte(m_RangeDecoder,
								(uint)nowPos64, prevByte, m_OutWindow.GetByte(rep0));
						else
							b = m_LiteralDecoder.DecodeNormal(m_RangeDecoder, (uint)nowPos64, prevByte);
						m_OutWindow.PutByte(b);
						state.UpdateChar();
						nowPos64++;
					}
					else
					{
						uint len;
						if (m_IsRepDecoders[state.Index].Decode(m_RangeDecoder) == 1)
						{
							if (m_IsRepG0Decoders[state.Index].Decode(m_RangeDecoder) == 0)
							{
								if (m_IsRep0LongDecoders[(state.Index << LZMABase.kNumPosStatesBitsMax) + posState].Decode(m_RangeDecoder) == 0)
								{
									state.UpdateShortRep();
									m_OutWindow.PutByte(m_OutWindow.GetByte(rep0));
									nowPos64++;
									continue;
								}
							}
							else
							{
								UInt32 distance;
								if (m_IsRepG1Decoders[state.Index].Decode(m_RangeDecoder) == 0)
								{
									distance = rep1;
								}
								else
								{
									if (m_IsRepG2Decoders[state.Index].Decode(m_RangeDecoder) == 0)
										distance = rep2;
									else
									{
										distance = rep3;
										rep3 = rep2;
									}
									rep2 = rep1;
								}
								rep1 = rep0;
								rep0 = distance;
							}
							len = m_RepLenDecoder.Decode(m_RangeDecoder, posState) + LZMABase.kMatchMinLen;
							state.UpdateRep();
						}
						else
						{
							rep3 = rep2;
							rep2 = rep1;
							rep1 = rep0;
							len = LZMABase.kMatchMinLen + m_LenDecoder.Decode(m_RangeDecoder, posState);
							state.UpdateMatch();
							uint posSlot = m_PosSlotDecoder[LZMABase.GetLenToPosState(len)].Decode(m_RangeDecoder);
							if (posSlot >= LZMABase.kStartPosModelIndex)
							{
								int numDirectBits = (int)((posSlot >> 1) - 1);
								rep0 = ((2 | (posSlot & 1)) << numDirectBits);
								if (posSlot < LZMABase.kEndPosModelIndex)
									rep0 += RangeCoderBitTreeDecoder.ReverseDecode(m_PosDecoders,
											rep0 - posSlot - 1, m_RangeDecoder, numDirectBits);
								else
								{
									rep0 += (m_RangeDecoder.DecodeDirectBits(
										numDirectBits - LZMABase.kNumAlignBits) << LZMABase.kNumAlignBits);
									rep0 += m_PosAlignDecoder.ReverseDecode(m_RangeDecoder);
								}
							}
							else
								rep0 = posSlot;
						}
						if (rep0 >= nowPos64 || rep0 >= m_DictionarySizeCheck)
						{
							if (rep0 == 0xFFFFFFFF)
								break;
							throw new DataErrorException();
						}
						m_OutWindow.CopyBlock(rep0, len);
						nowPos64 += len;
					}
				}
			}
			m_OutWindow.Flush();
			m_OutWindow.ReleaseStream();
			m_RangeDecoder.ReleaseStream();
		}

		public void SetDecoderProperties(byte[] properties)
		{
			if (properties.Length < 5)
				throw new InvalidParamException();
			int lc = properties[0] % 9;
			int remainder = properties[0] / 9;
			int lp = remainder % 5;
			int pb = remainder / 5;
			if (pb > LZMABase.kNumPosStatesBitsMax)
				throw new InvalidParamException();
			UInt32 dictionarySize = 0;
			for (int i = 0; i < 4; i++)
				dictionarySize += ((UInt32)(properties[1 + i])) << (i * 8);
			SetDictionarySize(dictionarySize);
			SetLiteralProperties(lp, lc);
			SetPosBitsProperties(pb);
		}

		/*
		public override bool CanRead { get { return true; }}
		public override bool CanWrite { get { return true; }}
		public override bool CanSeek { get { return true; }}
		public override long Length { get { return 0; }}
		public override long Position
		{
			get { return 0;	}
			set { }
		}
		public override void Flush() { }
		public override int Read(byte[] buffer, int offset, int count) 
		{
			return 0;
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
		}
		public override long Seek(long offset, System.IO.SeekOrigin origin)
		{
			return 0;
		}
		public override void SetLength(long value) {}
		*/
	}
//}
