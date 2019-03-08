using System;
using System.Collections;
using System.Collections.Generic;

namespace viper_script
{
    internal class TokenLine : IEnumerable
    {
        private static HashSet<char> UnionTokens { get; } = new HashSet<char> {'(', ')', '[', ']', '.', ','};

        private static bool IsUnionToken(char token)
            => UnionTokens.Contains(token);

        private static bool IsLetter(char token)
            => token >= 'a' && token <= 'z'
               || token >= 'A' && token <= 'Z'
               || token >= '0' && token <= '9'
               || token == '_';

        private string CodeLine { get; }

        private List<string> Tokens { get; } = new List<string>();

        public TokenLine(string line)
        {
            CodeLine = line;
            Tokenizate();
        }

        private void Tokenizate()
        {
            var startPos = 0;
            while (true)
            {
                SkipSpaces(ref startPos);
                var token = NextToken(startPos, out var endPos);

                if (token != null)
                    Tokens.Add(token);
                else
                    break;

                startPos = endPos + 1;
            }
        }

        private string NextToken(int startPos, out int endPos)
        {
            endPos = startPos;

            if (startPos >= CodeLine.Length)
                return null;

            if (IsUnionToken(CodeLine[startPos]))
                return $"{CodeLine[startPos]}";

            if (CodeLine[startPos] == '\'')
            {
                endPos = CodeLine.IndexOf('\'', startPos + 1);
                return CodeLine.Substring(startPos, endPos - startPos + 1);
            }

            var tokenIsAWord = IsLetter(CodeLine[startPos]);
            while (endPos < CodeLine.Length
                   && !IsUnionToken(CodeLine[endPos])
                   && CodeLine[endPos] != ' '
                   && IsLetter(CodeLine[endPos]) == tokenIsAWord)
                endPos++;

            endPos--;
            return CodeLine.Substring(startPos, endPos - startPos + 1);
        }

        private void SkipSpaces(ref int startPos)
        {
            while (startPos < CodeLine.Length && CodeLine[startPos] == ' ')
                startPos++;
        }

        //============    Реализация интерфейса IEnumerable    ============

        public class TokenEnum : IEnumerator
        {
            private int _position = -1;

            private readonly List<string> _tokens;

            public TokenEnum(List<string> tokens)
            {
                _tokens = tokens;
            }

            public bool MoveNext()
            {
                _position++;
                return (_position < _tokens.Count);
            }

            public bool MovePrev()
            {
                _position--;
                return (_position > -1);
            }

            public string PeekNext()
                => _position >= _tokens.Count ? null : _tokens[_position + 1];

            public string PeekPrev()
                => _position <= -1 ? null : _tokens[_position - 1];

            public void Reset()
            {
                _position = -1;
            }

            object IEnumerator.Current => Current;

            public string Current
            {
                get
                {
                    try
                    {
                        return _tokens[_position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TokenEnum GetEnumerator()
        {
            return new TokenEnum(Tokens);
        }
    }

    internal class Program
    {
        private static void Main()
        {
        }
    }
}
