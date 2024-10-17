using System;
namespace mc
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(">");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    return;
                }

                var Lexer = new Lexer(line);
                while (true)
                {
                    var token = Lexer.NextToken();
                    if (token.kind == SyntaxKind.EndOfFileToken)
                        break;
                    Console.Write($"{token.kind}:`{token.text}`");
                    if (token.value != null)
                        Console.Write($"{token.value}");
                    Console.WriteLine();
                }
            }
        }
    }

    enum SyntaxKind
    {
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken, // Fix here
        BadToken,
        EndOfFileToken
    }



    class SyntaxToken
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            this.kind = kind;
            this.position = position;
            this.text = text;
            this.value = value;
        }

        public SyntaxKind kind { get; set; }
        public int position { get; set; }
        public string text { get; set; }
        public object value { get; set; }
    }

    class Lexer
    {
        private readonly string _text;
        private int _position;

        public Lexer(string text)
        {
            _text = text;
        }

        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';
                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        
        }

        public SyntaxToken NextToken()
        {
            //<number>
            //<whitespace>
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            // Handle numbers
            if (char.IsDigit(Current))
            {
                var start = _position;
                while (char.IsDigit(Current))
                {
                    Next();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            // Handle whitespace
            if (char.IsWhiteSpace(Current))
            {
                var start = _position;
                while (char.IsWhiteSpace(Current))
                {
                    Next();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            // Handle individual characters like operators
            if (Current == '+')
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            else if (Current == '-')
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            else if (Current == '*')
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
            else if (Current == '/')
                return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
            else if (Current == '(')
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            else if (Current == ')')
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);

            // Handle bad tokens
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
        /*    public SyntaxToken NextToken()
           {
               //<number>
               //<whitespace>
               if (_position >= _text.Length)
               {
                   return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
               }
               if (char.IsDigit(Current))
               {
                   var start = _position;
                   while (char.IsDigit(Current))
                   {
                       Next();

                       var length = _position - start;
                       var text = _text.Substring(start, length);
                       int.TryParse(text, out var value);
                       return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
                   }
               }

               if (char.IsWhiteSpace(Current))
               {
                   var start = _position;
                   while (char.IsWhiteSpace(Current))
                   {
                       Next();

                       var length = _position - start;
                       var text = _text.Substring(start, length);
                       return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
                   }
               }

               if (Current == '+')
                   return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
               else if (Current == '-')
                   return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
               else if (Current == '*')
                   return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
               else if (Current == '/')
                   return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
               else if (Current == '(')
                   return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
               else if (Current == ')')
                   return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);

               return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
           }
       } */
    }