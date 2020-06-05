//
// AssemblyWriter.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2010 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Mono.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;

using RVA = System.UInt32;
using RID = System.UInt32;
using CodedRID = System.UInt32;
using StringIndex = System.UInt32;
using BlobIndex = System.UInt32;

namespace Mono.Cecil.Metadata {

#if !READ_ONLY

	using TypeRefRow     = Row<CodedRID, StringIndex, StringIndex>;
	using TypeDefRow     = Row<TypeAttributes, StringIndex, StringIndex, CodedRID, RID, RID>;
	using FieldRow       = Row<FieldAttributes, StringIndex, BlobIndex>;
	using MethodRow      = Row<RVA, MethodImplAttributes, MethodAttributes, StringIndex, BlobIndex, RID>;
	using ParamRow       = Row<ParameterAttributes, ushort, StringIndex>;
	using InterfaceImplRow = Row<uint, CodedRID>;
	using MemberRefRow   = Row<CodedRID, StringIndex, BlobIndex>;
	using ConstantRow    = Row<ElementType, CodedRID, BlobIndex>;
	using CustomAttributeRow = Row<CodedRID, CodedRID, BlobIndex>;
	using FieldMarshalRow = Row<CodedRID, BlobIndex>;
	using DeclSecurityRow = Row<SecurityAction, CodedRID, BlobIndex>;
	using ClassLayoutRow = Row<ushort, uint, RID>;
	using FieldLayoutRow = Row<uint, RID>;
	using EventMapRow    = Row<RID, RID>;
	using EventRow       = Row<EventAttributes, StringIndex, CodedRID>;
	using PropertyMapRow = Row<RID, RID>;
	using PropertyRow    = Row<PropertyAttributes, StringIndex, BlobIndex>;
	using MethodSemanticsRow = Row<MethodSemanticsAttributes, RID, CodedRID>;
	using MethodImplRow  = Row<RID, CodedRID, CodedRID>;
	using ImplMapRow     = Row<PInvokeAttributes, CodedRID, StringIndex, RID>;
	using FieldRVARow    = Row<RVA, RID>;
	using AssemblyRow    = Row<AssemblyHashAlgorithm, ushort, ushort, ushort, ushort, AssemblyAttributes, uint, uint, uint>;
	using AssemblyRefRow = Row<ushort, ushort, ushort, ushort, AssemblyAttributes, uint, uint, uint, uint>;
	using FileRow        = Row<FileAttributes, StringIndex, BlobIndex>;
	using ExportedTypeRow = Row<TypeAttributes, uint, StringIndex, StringIndex, CodedRID>;
	using ManifestResourceRow = Row<uint, ManifestResourceAttributes, StringIndex, CodedRID>;
	using NestedClassRow = Row<RID, RID>;
	using GenericParamRow = Row<ushort, GenericParameterAttributes, CodedRID, StringIndex>;
    using MethodSpecRow = Row<CodedRID, BlobIndex>;
    using GenericParamConstraintRow = Row<RID, CodedRID>;
    using ENCLogRow = Row<uint, uint>;
    

	public abstract class MetadataTable {

		public abstract int Length { get; }

		public bool IsLarge {
			get { return Length > 65535; }
		}

		public abstract void Write (TableHeapBuffer buffer);
		public abstract void Sort ();
	}

	public abstract class MetadataTable<TRow> : MetadataTable, IEnumerable {

		internal TRow [] rows = new TRow [2];
		internal int length;

		public override int Length {
			get { return length; }
		}

		public int AddRow (TRow row)
		{
			if (rows.Length == length)
				Grow ();

			rows [length++] = row;
			return length;
		}

		void Grow ()
		{
			var rows = new TRow [this.rows.Length * 2];
			Array.Copy (this.rows, rows, this.rows.Length);
			this.rows = rows;
		}

		public override void Sort ()
		{
		}

        public IEnumerator GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        public TRow this[int idx]
        {
            get { return rows[idx]; }
            set { rows[idx] = value; }
        }

        public void Clear ()
        {
            length = 0;
        }
    }

	public abstract class SortedTable<TRow> : MetadataTable<TRow>, IComparer<TRow> {

		public sealed override void Sort ()
		{
			Array.Sort (rows, 0, length, this);
		}

		protected int Compare (uint x, uint y)
		{
			return x == y ? 0 : x > y ? 1 : -1;
		}

		public abstract int Compare (TRow x, TRow y);
	}

	public sealed class ModuleTable : MetadataTable<uint> {

		public override void Write (TableHeapBuffer buffer)
		{
            for (int i = 0; i < length; i++) {
                buffer.WriteUInt16(0);		// Generation
                buffer.WriteString(rows [i]);	// Name
                buffer.WriteUInt16(1);		// Mvid
                buffer.WriteUInt16(0);		// EncId
                buffer.WriteUInt16(0);		// EncBaseId
            }
		}
	}

	public sealed class TypeRefTable : MetadataTable<TypeRefRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteCodedRID (
					rows [i].Col1, CodedIndex.ResolutionScope);	// Scope
				buffer.WriteString (rows [i].Col2);			// Name
				buffer.WriteString (rows [i].Col3);			// Namespace
			}
		}
	}

	public sealed class TypeDefTable : MetadataTable<TypeDefRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 ((uint) rows [i].Col1);	// Attributes
				buffer.WriteString (rows [i].Col2);			// Name
				buffer.WriteString (rows [i].Col3);			// Namespace
				buffer.WriteCodedRID (
					rows [i].Col4, CodedIndex.TypeDefOrRef);	// Extends
				buffer.WriteRID (rows [i].Col5, Table.Field);	// FieldList
				buffer.WriteRID (rows [i].Col6, Table.Method);	// MethodList
			}
		}
	}

	public sealed class FieldTable : MetadataTable<FieldRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 ((ushort) rows [i].Col1);	// Attributes
				buffer.WriteString (rows [i].Col2);			// Name
				buffer.WriteBlob (rows [i].Col3);			// Signature
			}
		}
	}

	public sealed class MethodTable : MetadataTable<MethodRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 (rows [i].Col1);		// RVA
				buffer.WriteUInt16 ((ushort) rows [i].Col2);	// ImplFlags
				buffer.WriteUInt16 ((ushort) rows [i].Col3);	// Flags
				buffer.WriteString (rows [i].Col4);		// Name
				buffer.WriteBlob (rows [i].Col5);		// Signature
				buffer.WriteRID (rows [i].Col6, Table.Param);	// ParamList
			}
		}
	}

	public sealed class ParamTable : MetadataTable<ParamRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 ((ushort) rows [i].Col1);	// Attributes
				buffer.WriteUInt16 (rows [i].Col2);		// Sequence
				buffer.WriteString (rows [i].Col3);		// Name
			}
		}
	}

	public sealed class InterfaceImplTable : MetadataTable<InterfaceImplRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i].Col1, Table.TypeDef);		// Class
				buffer.WriteCodedRID (rows [i].Col2, CodedIndex.TypeDefOrRef);	// Interface
			}
		}

		/*public override int Compare (InterfaceImplRow x, InterfaceImplRow y)
		{
			return (int) (x.Col1 == y.Col1 ? y.Col2 - x.Col2 : x.Col1 - y.Col1);
		}*/
	}

	public sealed class MemberRefTable : MetadataTable<MemberRefRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteCodedRID (rows [i].Col1, CodedIndex.MemberRefParent);
				buffer.WriteString (rows [i].Col2);
				buffer.WriteBlob (rows [i].Col3);
			}
		}
	}

	public sealed class ConstantTable : SortedTable<ConstantRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 ((ushort) rows [i].Col1);
				buffer.WriteCodedRID (rows [i].Col2, CodedIndex.HasConstant);
				buffer.WriteBlob (rows [i].Col3);
			}
		}

		public override int Compare (ConstantRow x, ConstantRow y)
		{
			return Compare (x.Col2, y.Col2);
		}
	}

	public sealed class CustomAttributeTable : SortedTable<CustomAttributeRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteCodedRID (rows [i].Col1, CodedIndex.HasCustomAttribute);	// Parent
				buffer.WriteCodedRID (rows [i].Col2, CodedIndex.CustomAttributeType);	// Type
				buffer.WriteBlob (rows [i].Col3);
			}
		}

		public override int Compare (CustomAttributeRow x, CustomAttributeRow y)
		{
			return Compare (x.Col1, y.Col1);
		}
	}

	public sealed class FieldMarshalTable : SortedTable<FieldMarshalRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteCodedRID (rows [i].Col1, CodedIndex.HasFieldMarshal);
				buffer.WriteBlob (rows [i].Col2);
			}
		}

		public override int Compare (FieldMarshalRow x, FieldMarshalRow y)
		{
			return Compare (x.Col1, y.Col1);
		}
	}

	public sealed class DeclSecurityTable : SortedTable<DeclSecurityRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 ((ushort) rows [i].Col1);
				buffer.WriteCodedRID (rows [i].Col2, CodedIndex.HasDeclSecurity);
				buffer.WriteBlob (rows [i].Col3);
			}
		}

		public override int Compare (DeclSecurityRow x, DeclSecurityRow y)
		{
			return Compare (x.Col2, y.Col2);
		}
	}

	public sealed class ClassLayoutTable : SortedTable<ClassLayoutRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 (rows [i].Col1);		// PackingSize
				buffer.WriteUInt32 (rows [i].Col2);		// ClassSize
				buffer.WriteRID (rows [i].Col3, Table.TypeDef);	// Parent
			}
		}

		public override int Compare (ClassLayoutRow x, ClassLayoutRow y)
		{
			return Compare (x.Col3, y.Col3);
		}
	}

	public sealed class FieldLayoutTable : SortedTable<FieldLayoutRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 (rows [i].Col1);		// Offset
				buffer.WriteRID (rows [i].Col2, Table.Field);	// Parent
			}
		}

		public override int Compare (FieldLayoutRow x, FieldLayoutRow y)
		{
			return Compare (x.Col2, y.Col2);
		}
	}

	public sealed class StandAloneSigTable : MetadataTable<uint> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++)
				buffer.WriteBlob (rows [i]);
		}
	}

	public sealed class EventMapTable : MetadataTable<EventMapRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i].Col1, Table.TypeDef);		// Parent
				buffer.WriteRID (rows [i].Col2, Table.Event);		// EventList
			}
		}
	}

	public sealed class EventTable : MetadataTable<EventRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 ((ushort) rows [i].Col1);	// Flags
				buffer.WriteString (rows [i].Col2);		// Name
				buffer.WriteCodedRID (rows [i].Col3, CodedIndex.TypeDefOrRef);	// EventType
			}
		}
	}

	public sealed class PropertyMapTable : MetadataTable<PropertyMapRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i].Col1, Table.TypeDef);		// Parent
				buffer.WriteRID (rows [i].Col2, Table.Property);	// PropertyList
			}
		}
	}

	public sealed class PropertyTable : MetadataTable<PropertyRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 ((ushort) rows [i].Col1);	// Flags
				buffer.WriteString (rows [i].Col2);		// Name
				buffer.WriteBlob (rows [i].Col3);		// Type
			}
		}
	}

	public sealed class MethodSemanticsTable : SortedTable<MethodSemanticsRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 ((ushort) rows [i].Col1);	// Flags
				buffer.WriteRID (rows [i].Col2, Table.Method);	// Method
				buffer.WriteCodedRID (rows [i].Col3, CodedIndex.HasSemantics);	// Association
			}
		}

		public override int Compare (MethodSemanticsRow x, MethodSemanticsRow y)
		{
			return Compare (x.Col3, y.Col3);
		}
	}

	public sealed class MethodImplTable : MetadataTable<MethodImplRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i].Col1, Table.TypeDef);	// Class
				buffer.WriteCodedRID (rows [i].Col2, CodedIndex.MethodDefOrRef);	// MethodBody
				buffer.WriteCodedRID (rows [i].Col3, CodedIndex.MethodDefOrRef);	// MethodDeclaration
			}
		}
	}

	public sealed class ModuleRefTable : MetadataTable<uint> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++)
				buffer.WriteString (rows [i]);	// Name
		}
	}

	public sealed class TypeSpecTable : MetadataTable<uint> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++)
				buffer.WriteBlob (rows [i]);	// Signature
		}
	}

	public sealed class ImplMapTable : SortedTable<ImplMapRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 ((ushort) rows [i].Col1);	// Flags
				buffer.WriteCodedRID (rows [i].Col2, CodedIndex.MemberForwarded);	// MemberForwarded
				buffer.WriteString (rows [i].Col3);		// ImportName
				buffer.WriteRID (rows [i].Col4, Table.ModuleRef);	// ImportScope
			}
		}

		public override int Compare (ImplMapRow x, ImplMapRow y)
		{
			return Compare (x.Col2, y.Col2);
		}
	}

	public sealed class FieldRVATable : SortedTable<FieldRVARow> {

		internal int position;

		public override void Write (TableHeapBuffer buffer)
		{
			position = buffer.position;
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 (rows [i].Col1);		// RVA
				buffer.WriteRID (rows [i].Col2, Table.Field);	// Field
			}
		}

		public override int Compare (FieldRVARow x, FieldRVARow y)
		{
			return Compare (x.Col2, y.Col2);
		}
	}

	public sealed class AssemblyTable : MetadataTable<AssemblyRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
			    buffer.WriteUInt32 ((uint) rows [i].Col1);	// AssemblyHashAlgorithm
			    buffer.WriteUInt16 (rows [i].Col2);			// MajorVersion
			    buffer.WriteUInt16 (rows [i].Col3);			// MinorVersion
			    buffer.WriteUInt16 (rows [i].Col4);			// Build
			    buffer.WriteUInt16 (rows [i].Col5);			// Revision
			    buffer.WriteUInt32 ((uint) rows [i].Col6);	// Flags
			    buffer.WriteBlob (rows [i].Col7);			// PublicKey
			    buffer.WriteString (rows [i].Col8);			// Name
			    buffer.WriteString (rows [i].Col9);			// Culture
            }
		}
	}

	public sealed class AssemblyRefTable : MetadataTable<AssemblyRefRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 (rows [i].Col1);		// MajorVersion
				buffer.WriteUInt16 (rows [i].Col2);		// MinorVersion
				buffer.WriteUInt16 (rows [i].Col3);		// Build
				buffer.WriteUInt16 (rows [i].Col4);		// Revision
				buffer.WriteUInt32 ((uint) rows [i].Col5);	// Flags
				buffer.WriteBlob (rows [i].Col6);		// PublicKeyOrToken
				buffer.WriteString (rows [i].Col7);		// Name
				buffer.WriteString (rows [i].Col8);		// Culture
				buffer.WriteBlob (rows [i].Col9);		// Hash
			}
		}
	}

	public sealed class FileTable : MetadataTable<FileRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 ((uint) rows [i].Col1);
				buffer.WriteString (rows [i].Col2);
				buffer.WriteBlob (rows [i].Col3);
			}
		}
	}

	public sealed class ExportedTypeTable : MetadataTable<ExportedTypeRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 ((uint) rows [i].Col1);
				buffer.WriteUInt32 (rows [i].Col2);
				buffer.WriteString (rows [i].Col3);
				buffer.WriteString (rows [i].Col4);
				buffer.WriteCodedRID (rows [i].Col5, CodedIndex.Implementation);
			}
		}
	}

	public sealed class ManifestResourceTable : MetadataTable<ManifestResourceRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 (rows [i].Col1);
				buffer.WriteUInt32 ((uint) rows [i].Col2);
				buffer.WriteString (rows [i].Col3);
				buffer.WriteCodedRID (rows [i].Col4, CodedIndex.Implementation);
			}
		}
	}

	public sealed class NestedClassTable : SortedTable<NestedClassRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i].Col1, Table.TypeDef);		// NestedClass
				buffer.WriteRID (rows [i].Col2, Table.TypeDef);		// EnclosingClass
			}
		}

		public override int Compare (NestedClassRow x, NestedClassRow y)
		{
			return Compare (x.Col1, y.Col1);
		}
	}

	public sealed class GenericParamTable : MetadataTable<GenericParamRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt16 (rows [i].Col1);		// Number
				buffer.WriteUInt16 ((ushort) rows [i].Col2);	// Flags
				buffer.WriteCodedRID (rows [i].Col3, CodedIndex.TypeOrMethodDef);	// Owner
				buffer.WriteString (rows [i].Col4);		// Name
			}
		}
	}

	public sealed class MethodSpecTable : MetadataTable<MethodSpecRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteCodedRID (rows [i].Col1, CodedIndex.MethodDefOrRef);	// Method
				buffer.WriteBlob (rows [i].Col2);	// Instantiation
			}
		}
	}

	public sealed class GenericParamConstraintTable : MetadataTable<GenericParamConstraintRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i].Col1, Table.GenericParam);	// Owner
				buffer.WriteCodedRID (rows [i].Col2, CodedIndex.TypeDefOrRef);	// Constraint
			}
		}
	}
    
	public sealed class FieldPtrTable : MetadataTable<RID> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i], Table.Field);	// Field
			}
		}
	}
    
	public sealed class MethodPtrTable : MetadataTable<RID> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i], Table.Method);	// Method
			}
		}
	}

	public sealed class ParamPtrTable : MetadataTable<RID> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i], Table.Param);	// Param
			}
		}
	}
    
	public sealed class EventPtrTable : MetadataTable<RID> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i], Table.Event);	// Event
			}
		}
	}
    
	public sealed class PropertyPtrTable : MetadataTable<RID> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteRID (rows [i], Table.Property);	// Property
			}
		}
	}
    
	public sealed class ENCLogTable : MetadataTable<ENCLogRow> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 (rows [i].Col1);	// Token
				buffer.WriteUInt32 (rows [i].Col2);	// FuncCode
			}
		}
	}

	public sealed class ENCMapTable : MetadataTable<uint> {

		public override void Write (TableHeapBuffer buffer)
		{
			for (int i = 0; i < length; i++) {
				buffer.WriteUInt32 (rows [i]);	// Token
			}
		}
	}

	sealed class RowEqualityComparer : IEqualityComparer<Row<string, string>>, IEqualityComparer<Row<uint, uint>>, IEqualityComparer<Row<uint, uint, uint>> {

		public bool Equals (Row<string, string> x, Row<string, string> y)
		{
			return x.Col1 == y.Col1
				&& x.Col2 == y.Col2;
		}

		public int GetHashCode (Row<string, string> obj)
		{
			string x = obj.Col1, y = obj.Col2;
			return (x != null ? x.GetHashCode () : 0) ^ (y != null ? y.GetHashCode () : 0);
		}

		public bool Equals (Row<uint, uint> x, Row<uint, uint> y)
		{
			return x.Col1 == y.Col1
				&& x.Col2 == y.Col2;
		}

		public int GetHashCode (Row<uint, uint> obj)
		{
			return (int) (obj.Col1 ^ obj.Col2);
		}

		public bool Equals (Row<uint, uint, uint> x, Row<uint, uint, uint> y)
		{
			return x.Col1 == y.Col1
				&& x.Col2 == y.Col2
				&& x.Col3 == y.Col3;
		}

		public int GetHashCode (Row<uint, uint, uint> obj)
		{
			return (int) (obj.Col1 ^ obj.Col2 ^ obj.Col3);
		}
    }

#endif
}