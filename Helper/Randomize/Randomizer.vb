Imports System.Security.Cryptography
Imports System.Text

Namespace RandomizeHelper
    Public Class Randomizer

#Region " Fields "
        Private m_maxLength%
        Private ReadOnly m_generatedNames As List(Of String)
        Private ReadOnly m_alphabeticChars As Char()() = {"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()}
        Private ReadOnly m_japenese$ = "れづれなるまゝに日暮らし硯にむかひて心にうりゆくよな事を、こはかとなく書きつくればあやうこそものぐるほけれ。"
        Private ReadOnly m_greek$ = "αβγδεζηθικλµνξοπρστυϕχψω"
        Private ReadOnly m_Symbols$ = "☹☺☻☼☽☾☿♀♁♂♔♕♖♗♘♙♚♛♜♝♞♟♠♡♢♣♤♥♦♧♩♪♫♬♭♮♯♻♼♿⚐⚑⚒⚓⚔⚕⚖⚠⚢⚣⚤⚥⚦⚧⚨⚩⛄⛅⚽✔✓✕✖✗✘✝✞✟❗❓❤☸"
        Private ReadOnly m_rdn As Random
        Private ReadOnly m_rdnBool As Random
        Private ReadOnly m_reservedWords As String()
        Private ReadOnly m_flowing As String = ".̃̏̂͑̽̿̄ͯ҉̬̬̭͉͙.ͮͦ͌̑̄ͤͥͫ̕͏͉̖̪̪̤͈̝̠͓̥̹̥̺͉ͅ.̶̸̘̘̙̭̻͖̪͍̖̰͍͇̟̯̖̃͒ͧ̈́ͧ͛͑ͣ̂̌͑̽̎̔̚͢.͙̙͓͔̻̺̪͙̼̺͙̠̭̣̯̫͔̭ͥͦͫ͜͟͞.̩͈̤̼̬̬̼̘͎̻̼̠̼͓̝̯̰̌̆̋̃͗̃̅͂̂̈́̔͒̑̕͜.̧̭̫̭̮͙̣̺̳̦̝̦͍͚̟̯̟̽̂ͨ͑̈̀͟͠.̡̣̖͚̮͓͇͔͈̱̯̞͓̙̞͕͚́ͩ̾ͯ̍̏̿̆ͫ̆͛̑́͝ͅ.̛̊ͨ̆͂͌ͭ͏̸͓͕͕͓̗͙͇̤͍̦͕̥̘͇.̡͔͇͍̦͚̲͔̯̪̙̘͓͚̬̲͔̼͕̽̃̉ͫ̓̑ͫ̉ͫ̒̊͜.͎̹̫͕ͯ̐͌̐͒͛̐̎̏ͨͮ͂̒̀̚͠.̶̷̨̝̣͖͇̲̯͇̰͈̙͉͙͚͉̄͑͗̏̒ͪ̏ͮ͌͗ͬͪͥͭ̊͋͞.̜̟͕̺̣͕̥͚͔͓̠̞̳̭̠ͪ̽ͭ͒ͮ͘͝.̛̝̦͎͚̈̉̈́̈͛ͯ͑ͫ̊ͮͬ̆ͣ͂ͥ.̸͇̮͙͈͇̱͈͕̜̬̻̮ͫ͊ͭ̏͑̔͐̑ͬ̾̂ͩ͆ͫ̀.ͤ͂ͩ̀͑̒͏̷́͏͓̮̙͈̮̳̲̭̺̟̱̞͍̜̥͜.̶̷̣̮͍͇͈̝̞͓̦͐ͤͦͤͦͭ̆͒̓̀ͫ̅̐̚͡.̵̢͇̯͕͕̤̥̘͍͂̆̊ͮ̆̋̿ͧ͊ͩ̑͜.̸̧̗̜̼͖̲̟̹̞̈ͭ̊̔ͪ̐ͤ͆̇̔ͫͮ̀́.̏̅̃̓ͭ͏͇̲͎̹̖͙͎̯̥͡.̢̰͕̭̲͖͇͒́̾͋ͬ̅̈͑ͥͅͅ.͊̒̃͒́͜͏̢͉̲̹̼̥̥͖̘̼̹͈͉.̸̷̶̜̞͖̪̻̦͕͕̼̮̳͙̯̹̩̗̓͌̑ͭ̏͂̾͂̒ͭ̍̀̚.̵̡̘͕͚̳͐ͦͥ̉͘͢ͅ.̶̸̨̨͍̳̣̱͓̫̫̱̖̣͔̅ͧ̂́ͯ̓͋͋͂̾͑̈́̇̑̎̑ͭ̍͜.̴̵̘̩͍͖̻̦̣͕̗̖͔̘͓͗̈͛͂́̾ͫ͛̄͆ͤ̑͘͘.̡̛̮͇̫̮͔̲͕̫̹̘̞̱̾̈ͬ̆ͦ̈́͂̀̌̈́̆͋͆͋́.̶̴̨̩̻̮̹͔̞̻͖̭̻̲̉͆̓ͨͥ̈́̈́ͤ̅͑̆̑̔̔̍̀͘͝.̃̏̂͑̽̿̄ͯ҉̬̬̭͉͙.ͮͦ͌̑̄ͤͥͫ̕͏͉̖̪̪̤͈̝̠͓̥̹̥̺͉ͅ.̶̸̘̘̙̭̻͖̪͍̖̰͍͇̟̯̖̃͒ͧ̈́ͧ͛͑ͣ̂̌͑̽̎̔̚͢.͙̙͓͔̻̺̪͙̼̺͙̠̭̣̯̫͔̭ͥͦͫ͜͟͞.̩͈̤̼̬̬̼̘͎̻̼̠̼͓̝̯̰̌̆̋̃͗̃̅͂̂̈́̔͒̑̕͜.̧̭̫̭̮͙̣̺̳̦̝̦͍͚̟̯̟̽̂ͨ͑̈̀͟͠.̡̣̖͚̮͓͇͔͈̱̯̞͓̙̞͕͚́ͩ̾ͯ̍̏̿̆ͫ̆͛̑́͝ͅ.̛̊ͨ̆͂͌ͭ͏̸͓͕͕͓̗͙͇̤͍̦͕̥̘͇.̡͔͇͍̦͚̲͔̯̪̙̘͓͚̬̲͔̼͕̽̃̉ͫ̓̑ͫ̉ͫ̒̊͜.͎̹̫͕ͯ̐͌̐͒͛̐̎̏ͨͮ͂̒̀̚͠.̶̷̨̝̣͖͇̲̯͇̰͈̙͉͙͚͉̄͑͗̏̒ͪ̏ͮ͌͗ͬͪͥͭ̊͋͞.̜̟͕̺̣͕̥͚͔͓̠̞̳̭̠ͪ̽ͭ͒ͮ͘͝.̛̝̦͎͚̈̉̈́̈͛ͯ͑ͫ̊ͮͬ̆ͣ͂ͥ.̸͇̮͙͈͇̱͈͕̜̬̻̮ͫ͊ͭ̏͑̔͐̑ͬ̾̂ͩ͆ͫ̀.ͤ͂ͩ̀͑̒͏̷́͏͓̮̙͈̮̳̲̭̺̟̱̞͍̜̥͜.̶̷̣̮͍͇͈̝̞͓̦͐ͤͦͤͦͭ̆͒̓̀ͫ̅̐̚͡.̵̢͇̯͕͕̤̥̘͍͂̆̊ͮ̆̋̿ͧ͊ͩ̑͜.̸̧̗̜̼͖̲̟̹̞̈ͭ̊̔ͪ̐ͤ͆̇̔ͫͮ̀́.̏̅̃̓ͭ͏͇̲͎̹̖͙͎̯̥͡.̢̰͕̭̲͖͇͒́̾͋ͬ̅̈͑ͥͅͅ.͊̒̃͒́͜͏̢͉̲̹̼̥̥͖̘̼̹͈͉.̸̷̶̜̞͖̪̻̦͕͕̼̮̳͙̯̹̩̗̓͌̑ͭ̏͂̾͂̒ͭ̍̀̚.̵̡̘͕͚̳͐ͦͥ̉͘͢ͅ.̶̸̨̨͍̳̣̱͓̫̫̱̖̣͔̅ͧ̂́ͯ̓͋͋͂̾͑̈́̇̑̎̑ͭ̍͜.̴̵̘̩͍͖̻̦̣͕̗̖͔̘͓͗̈͛͂́̾ͫ͛̄͆ͤ̑͘͘.̡̛̮͇̫̮͔̲͕̫̹̘̞̱̾̈ͬ̆ͦ̈́͂̀̌̈́̆͋͆͋́.̶̴̨̩̻̮̹͔̞̻͖̭̻̲̉͆̓ͨͥ̈́̈́ͤ̅͑̆̑̔̔̍̀͘͝ḩ̷̸͎̞̬͚͙́͒̃̿̑ส็็็็็็็็็็็็็็็็็็็i͇̠̱̽͛ͣͯͭ̐͐ͩͪ̀͒̿̍̆̌ͣ̕͞ţ̈́̄ͦ͑͐ͤ̇ͯ̚͜͢͏̺͎̰̯̰̳̣̺͉͉̻̯̱͉̱̳̠̫l̢̮̝̰̖̲̯͉̱͉̤̗̯͇ͫ͋͑͋͊́͑͠e̛̼͉̝̯̼͚͇̜̹̬̼͚̥̝̟̩̮̎̾ͧ͟͝ͅr̷͎̣͙͇̦̱̺͚̬͍͎̗̺͍͈͍̔̃̆ͬ̃͌ͦ͗ͧ̓͋̓͟͟͡ͅ ̵̩̼͙̣̦͕̃ͨͧ̂ͭ͂̀͜ḣ͚͖͉͓̫̲̦͓́̆̈ͯ͒͂ͫ͛ͣ̓ͫ̄́́̕͜͜͜ą̴̢̺̼͎̩͓̱͍̯͓̻̖͓̯̿ͩͩͦ̕ͅt͂ͫ̔͋͆̀ͩͨ͂̎̓ͧ̿̈́̓̏̃ͯ̈͘͘҉͚̬̝͙̟̗̰̹̱̗ ̵̠̤̼̬̩͔̲̖̎̍̈̌̾̎̋̂̓ͬ̒ͫ̽ͭ́̕͠n̛̲͙̻̤̮̥̠͇͇͖͎̘̠̲ͥͣ̋͛ͨ̀i̧͓͖͈̭͔͉̼ͪͫ̓͂̔̿͠ͅč̨̆̉͂̑͞͏̻̠̖̼̹̻̹̯͇͙̰̪̯hͭ͌ͦ̉̊͐͂҉̸͇̹̹̬̖͕̱͕͕̠̗̀͘͝tͤͫͫͧ̈́͂͛ͭ̉ͧ͛ͫ̚҉̴͚̯̭̲̫̦͖̮̭͖̗͎̳̟̀͘s̸ͧ͗̌͊ͨ̐̅̇͟҉͈̤̘͉̤̯̝͈͚ ͤ͆͆ͨ̓҉̤̣̩̠̩̯̩̱͕̹͜͝f̡̞͔̮͖̩͔̀̆̅̓̈ͪͥ͋͊̉ͪ̇̉̃̔͋̃̈́͟͠͝ä̵̸̺̖͓͖̳̬̲̲͎͎͔̈͋͑͋ͦͅͅl̻̟̲̞̘͚̤͎͉̯̫̹̜̥̳͈̙ͧ͋̇ͫ͋̎ͯ̋͂ͮ̈͂̾ͯ̎̊̾ͯ͘͢ͅs̴̡͍̖̖͕̱̫̤̣͛ͩͤ͆̅ͣ͐̿ͣ͐̔ͨ̄ͫͩ̄̍͘̕͜c̵̷̨͇̰̰̼̝̝̼̤͎̯̺̰͕̤̤͇ͧͦ̄̇̓ͩ̎͂̊ͯ͋͋̋̀ͬͧ͗̔̚͝͡ͅͅh͓͙̩̭̬̠̜͇̗̮̐ͥͧͭ̆͆̔ͬ́̄͊ͮ͡͝͠ ̩̹̥̯̲͉͔̟͕͎̪͔̱̬̌ͭ͗̔̏̊̚͡͞g̴̴̫͖̥̲̦͉̩̲̪̹̙̘̩̣̯̜̱͌ͩ͑̆̿̏̽ͤ͂ͩ́͢͠e̤̳͈̹͉̹̪̥̜̲͙͕͍̟̱̱̳͗̽ͤ̈́̽̽̊͢͡ḿ͈̮͓͇̞̯͍̦͖̟͔̫͈̏̑̋̂̒ͬ͌̌̓̄͢͞ą̶̴͈̳̥͙͚͓͉̟̬̤͋͒ͫ̓̿́̒͐͒͘͡c̡̞̪̞̣̦̖̙̬̜̜̋̿͐̇̓̋̃͆̚h̨̛̝͓͚̱͍͕̝̬̯̩̓̽̉͌͊̇͐̒̈͋͋̌ͩ̋ͭt̶̄́͑̐҉҉̟̭̟͓̜̩͖̲͔̀ ̷̧̡̦̞̝̯̥͍̻͚̠̞̣̯͎̇̓̇̐̓́͝h̡̺̳͇̤̬̻̮̭͇̿̑̔̽ͮ̉̏̃͂̄̌̍͒̑̇ͪ̑͡͞ĭ̵͔͙̞̻̰̻̬̖͇̩͔̪̮̩̘̔ͪͥ̈́͋͞t̸̝̪̹̲̤̜̓͌ͤ̍ͫͧ͋͋ͣ͆̈́ͩͯ̒ͮ̊ͧ̕͢͜ͅl̈́ͦ̈̌͆ͧ͏̷̛̮̼̬͓̞̻̣̼͙ͅe̗͚̺̰͎͖̥̙̻͕̮͕̱͇̓̑ͫ͋ͭͯ̍ͨ̌͗̊̔̒̈́̽̕͜͝ͅṛ̶̷̟̹̥̹̬̖͖͇̬̭̲̬̠̮ͣͧ̓̓̈́̅͢ ̨̛̘̲͉̮̲̹̳͔̗̣̣̗̱̱̘͚ͦ͋̀̍̃̑ͣ̍͒̇ͭ̆ͧ͒͒̋̕h̷͔̠͈̙̝̻̺͔͕̤ͯ̃̎̋͑ͫ̎̾̃̿̀̄́̀͞a̍̏͛̔͆͗͒̆̐́̈ͥ̃͌͝҉̧҉̷̪̳̻͓̘̜̘͔̘̞̱̫͈̹ͅť̷̢̫̰̦̫̯̮̟͇̍͊̒̅̀̐͊ͯ́̒̅ͤͅ ̵̮̤̳̺̤̼̝̉̑̎ͧ̏̀̽̽́n̴̴̯̹̮̣͉̹̝̑͑̐̿ͮ̈̆̔́̏ͥ͋ͨ̒͘͘͟i̲̬͓̯̋͑̂̋͋͡c͑̒̈́ͫ̓̎͆̃̃ͬͯ͜͏͓̲̝̺̥̘͍͚̕ḩ̜͖̤̼͇̳̳̭̻͖̳̙͑͌̑̓͒ͦ͂ͮ̅̅͌̈ͭͭ̄ͅṫ̴͎̺̭̺̞̺̮̼̣͔͎͔̗̱̭̄̆̓͋̔͝s̛͓͇̮̹̙̫̮̦̪̜̋ͪ̊̊ͮ̎͌̂̂̿̽̔̉̓̍̔͆̕ ̛̽͌͋ͩ̃̈̍ͨ̅ͦ̀̏̍̓̑̍̊ͧ͘͏̪͔͇ͅf̢̰̲̺͙̝̣͕̭̝̙͍ͧ̌ͤͮ͛͋ͭ́̓̽̔̈́̂ͤ̉̆ͩ̚͘ͅa̧̡ͪ̊̊ͩ̈͠͏҉̠̬̝̻͙̰̖̻̼̖̘̠̺̝l̸̷͊͒ͨ̄̓̂҉̥̩̮̳̯̠̻͎̹͈̟̠̫̮̫̠̖̥͙s̶͒̓̇͑͗̍̿̐ͮ҉͇͖͓̣͉̗̰̯͎̖͎̱c̶͇̭̣͍̞̝͓͇̫̯̜̫̞͉̑ͤͧ̎̒̈ͯͣͥ̍ͪ̌̎̒́h̵̢̼̮̖͎̭̭͇͚̮͙͙͇̗̤̝̺͚̐̏͋ͬ̋̿̎ͭ̂̾̂̓ͪ̏̋̀ ̵̨̛̱̖̫͇̱͈̞̭̱́̔ͪ̋̎̎ͭͨ̈̿ͤ̎̿͟ͅg̉͂͛̍̓ͫ̇̿̎͐̓̏͏͏̴̤̗̙͖͙̱̺͎̖̩͝ę̵̞͖̭̳͔̻͉̯̻̯̣̈́́̈̊̽̾͗ͣ̃͊ͬ̔ṃ̶̳̫̲̩̺͍̝̰̻̱̖̦̪̘̠̏ͫ̎ͤ̓ͣ̾͊̑͒͗̋͂̉̈́͜͞a̰̯͔̗̠͖̣̬̖͐ͬ̏͐ͬ͊̂̍̇ͣͩ́͠͞c̨̡̬̥̟̩̠̬̟̪͖̙̮̺̩͍̮̝ͥ̌̉ͣ͊̈͌̓̈̉̈͐̿̈ͬ̀h̛̬͚̝͉̮̝͉̥̺̩̼̞̙̖͎̮̳̒͂̋̋ͬ̓͋̂̊̚̕t͒ͮ̿ͤͬ̎ͭ͌̅̂̾̐̉ͦ͌ͧͯ͋̈͏̮̮̰̻̕͜͞ ̸͎̥̞̟͙͚͙̥ͬ̈́̋̔̿ͣͨͧͫ͒̿ͬͫͧ̊͢͜͠ḫ̝̘̤̰͐͆ͤ̑̒͛ͩ́͞iͤ̓͑̊̏ͥ̀͘͞҉̨̗̬͉̜̘̜t̑ͨ͋̾ͦ̋̊ͤ̔̒̑̿̓́͏̵̛̱͚̖͓̲̕l̶͍̳̳͎͓̀ͭ̎̉̌̓̊̌̍̍̀̕e̵̡͇͈͉̥̼̼̺͎͉̦ͮ̾̒̃ͫ̃͒̃̓́̅͆ͬ͗ͨ̿ͥ̂̚͝͡ŗ̡̊͋̾ͩ̽͆̋̈́͐͊̂ͮͨ̉̏ͨ͐͐̆̕͞͏͍͍̰̻̖̥ ͨ́̿̃̇ͯ̾̕͢҉̩͇̟̪̥̬͍̲͈͔͔͍̼̭͇̣h̷̻̝̫̪͚̦͙͉͎̥̦̳͉̖̃ͥ̅͗͢ḁ̛̪̗̮̦̳̪̭̞̗̠̟̈́̌͐̈́ͦ̄̉̎ͬ͆̒̉̕͟t̶̛͔͍͔͎̫̞̖͓̰̒̇̆ͯ̀ͥ̈́̏̓̀ͮͥ̍̀ ̨ͥ̄̓ͩ̿̃̿̊̈̔͒͗͊ͭ̽ͥͥ͐҉̶̸̲̻̱̩̖̪̹͈̙̩͎̲̘̙n"
        Public invisibleChars As Integer()
#End Region

#Region " Enumerations "
        Public Enum RenameEnum
            Alphabetic = 0
            Greek = 1
            Invisible = 2
            Chinese = 3
            Japanese = 4
            Dot = 5
            Symbols = 6
            Flowing = 7
        End Enum
#End Region

#Region " Properties "
        Private ReadOnly RenameScheme As RenameEnum
#End Region

#Region " Constructors "
        Public Sub New(RenameSch As RenameEnum)
            RenameScheme = RenameSch
            m_maxLength = 1
            m_generatedNames = New List(Of String)
            m_rdn = New Random()
            m_rdnBool = New Random(Date.Now.Millisecond)
            m_reservedWords = New String() {"addhandler", "addressof", "alias", "and", "andalso", "ansi", "append", "as", "assembly", "auto", "binary", "boolean", "byref", "byte", "byval", "call", "case", "catch", "cbool", "cbyte", "cchar", "cdate", "cdec", "cdbl", "char", "cint", "class", "clng", "cobj", "compare", "const", "continue", "cbyte", "cshort", "csng", "cstr", "ctype", "cuint", "culng", "cushort", "date", "decimal", "declare", "default", "delegate", "dim", "directcast", "do", "double", "each", "else", "elseif", "end", "endif", "enum", "erase", "error", "event", "explicit", "exit", "false", "finally", "for", "friend", "function", "get", "gettype", "global", "gosub", "goto", "handles", "if", "implements", "imports", "in", "inherits", "input", "integer", "interface", "internal", "is", "isnot", "let", "lib", "like", "lock", "long", "loop", "me", "mid", "mod", "module", "notinheritable", "mustinherit", "mustoverride", "my", "mybase", "myclass", "namespace", "narrowing", "new", "next", "not", "nothing", "notinheritable", "notoverridable", "object", "of", "off", "on", "operator", "option", "optional", "or", "orelse", "output", "overloads", "overridable", "overrides", "paramarray", "preserve", "partial", "private", "property", "protected", "public", "raiseevent", "random", "read", "readonly", "redim", "rem", "removehandler", "resume", "return", "sbyte", "seek", "select", "set", "shadows", "shared", "short", "single", "static", "step", "stop", "strict", "string", "structure", "sub", "synclock", "t", "then", "throw", "to", "true", "try", "trycast", "typeof", "variant", "wend", "uinteger", "ulong", "until", "ushort", "using", "void", "when", "while", "widening", "with", "withevents", "writeonly", "xor", "region"}
            invisibleChars = New Integer(3) {8203, 8204, 8206, 8207}
        End Sub
#End Region

#Region " Methods "
        Public Function GenerateInvisible() As Integer
            Dim b = invisibleChars
            Return m_rdn.Next(b(0), b(3))
        End Function

        Public Function GenerateBoolean() As Boolean
            Dim b = New Integer(9) {5, 8, 1, 7, 3, 2, 9, 0, 4, 6}
            Return (Odd(b(m_rdnBool.Next(10))))
        End Function

        Private Function Odd(value%) As Boolean
            Return (value And 1) = 1
        End Function

        ''' <summary>
        ''' Creates a pseudo-random password containing the number of character classes
        ''' </summary>
        Private Function GenerateKey(length%) As String
            Dim csp As New RNGCryptoServiceProvider()

            Dim allchars = m_alphabeticChars.Take(1).SelectMany(Function(c) c).ToArray()
            Dim bytes = New Byte(allchars.Length - 1) {}
            csp.GetBytes(bytes)

            For i% = 0 To allchars.Length - 1
                Dim tmp = allchars(i)
                allchars(i) = allchars(bytes(i) Mod allchars.Length)
                allchars(bytes(i) Mod allchars.Length) = tmp
            Next
            Array.Resize(bytes, length)
            Dim result = New Char(length - 1) {}
            While True
                csp.GetBytes(bytes)
                For i% = 0 To length - 1
                    result(i) = allchars(bytes(i) Mod allchars.Length)
                Next
                If Char.IsWhiteSpace(result(0)) OrElse Char.IsWhiteSpace(result((length - 1) Mod length)) Then
                    Continue While
                End If

                Dim testResult As New String(result)
                If 0 <> m_alphabeticChars.Take(1).Count(Function(c) testResult.IndexOfAny(c) < 0) Then
                    Continue While
                End If

                Return testResult
            End While
            Return m_alphabeticChars.Take(1).ToString
        End Function

        Private Function Alphabetic() As String
            Dim str$ = GenerateKey(m_maxLength)
            If m_generatedNames.Contains(str.ToLower) OrElse m_reservedWords.Contains(str.ToLower) Then
                m_maxLength += 1
                Return Alphabetic()
            End If
            m_generatedNames.Add(str.ToLower)
            Return str
        End Function

        Public Function GenerateNewAlphabetic() As String
            Return Alphabetic()
        End Function

        Private Function Japanese() As String
            Dim str$ = New String(Enumerable.Repeat(m_japenese, m_rdn.Next(1, m_maxLength)).Select(Function(s) s(m_rdn.Next(s.Length))).ToArray())
            If m_generatedNames.Contains(str) Then
                m_maxLength += 1
                Return Japanese()
            End If
            m_generatedNames.Add(str)
            Return str
        End Function

        Private Function Dots() As String
            Dim builder As New StringBuilder()
            Dim randomize As New Random(Guid.NewGuid().GetHashCode())

            For i% = 0 To m_maxLength - 1
                builder.Append(Convert.ToChar(randomize.Next(AscW("⠀"c), AscW("⠎"c))))
            Next

            If m_generatedNames.Contains(builder.ToString()) Then
                m_maxLength += 1
                Return Dots()
            End If

            m_generatedNames.Add(builder.ToString())
            Return builder.ToString()
        End Function

        Public Function GenerateNewInvisible() As String
            Return Invisible()
        End Function

        Private Function Invisible() As String
            Dim builder As New StringBuilder()
            Dim randomize As New Random(Guid.NewGuid().GetHashCode())

            For i% = 0 To m_maxLength - 1
                builder.Append(Convert.ToChar(randomize.Next(AscW("​"c), AscW("‏"c))))
            Next

            If m_generatedNames.Contains(builder.ToString()) Then
                m_maxLength += 1
                Return Invisible()
            End If

            m_generatedNames.Add(builder.ToString())
            Return builder.ToString()
        End Function

        ''' <summary>
        ''' by Yck from Confuser
        ''' </summary>
        Private Function Chinese() As String
            Dim arr As New BitArray(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().GetHashCode().ToString())))

            Dim rand As New Random(Guid.NewGuid().GetHashCode())
            Dim xorB As Byte() = New Byte(arr.Length / 8 - 1) {}
            rand.NextBytes(xorB)
            Dim x As New BitArray(xorB)

            Dim result As BitArray = arr.Xor(x)
            Dim ret As Byte() = New Byte(result.Length / 8 - 1) {}
            result.CopyTo(ret, 0)

            Return Encoding.Unicode.GetString(ret).Replace(vbNullChar, "").Replace(".", "").Replace("/", "")
        End Function

        Private Function Greek() As String
            Dim str$ = New String(Enumerable.Repeat(m_greek, m_rdn.Next(1, m_maxLength)).Select(Function(s) s(m_rdn.Next(s.Length))).ToArray())
            If m_generatedNames.Contains(str) Then
                m_maxLength += 1
                Return Greek()
            End If
            m_generatedNames.Add(str)
            Return str
        End Function

        Private Function Symbols() As String
            Dim str$ = New String(Enumerable.Repeat(m_Symbols, m_rdn.Next(1, m_maxLength)).Select(Function(s) s(m_rdn.Next(s.Length))).ToArray())
            If m_generatedNames.Contains(str) Then
                m_maxLength += 1
                Return Symbols()
            End If
            m_generatedNames.Add(str)
            Return str
        End Function

        Private Function Flowing() As String
            Dim str$ = New String(Enumerable.Repeat(m_flowing, m_rdn.Next(1, m_maxLength)).Select(Function(s) s(m_rdn.Next(s.Length))).ToArray())
            If m_generatedNames.Contains(str) Then
                m_maxLength += 1
                Return Flowing()
            End If
            m_generatedNames.Add(str)
            Return str
        End Function

        Public Function GenerateNew() As String
            Select Case RenameScheme
                Case RenameEnum.Alphabetic
                    Return Alphabetic()
                Case RenameEnum.Greek
                    Return Greek()
                Case RenameEnum.Invisible
                    Return Invisible()
                Case RenameEnum.Chinese
                    Return Chinese()
                Case RenameEnum.Japanese
                    Return Japanese()
                Case RenameEnum.Dot
                    Return Dots()
                Case RenameEnum.Symbols
                    Return Symbols()
                Case RenameEnum.Flowing
                    Return Flowing()
            End Select

            Return Nothing
        End Function

        Public Sub CleanUp()
            m_maxLength = 1
            m_generatedNames.Clear()
        End Sub
#End Region

    End Class
End Namespace