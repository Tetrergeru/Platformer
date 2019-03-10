using System;
using System.Collections.Generic;

namespace viper_script
{
    internal class LineParser
    {
        private HashSet<string> RouteOperators { get; } = new HashSet<string> {"(", ".", ","};

        private Dictionary<string, int> Operators { get; } = new Dictionary<string, int>
        {
            {"(", int.MinValue},
            {"=", -1},
            {",", 0},
            {"+", 1},
            { "-",1},
            { "*",2},
            { "/",2},
            { "ByIdx",5},
        };

        private HashSet<string> Functions { get; } = new HashSet<string> {"f", "max", "len", "MkList", "print"};
        
        private Stack<string> OperationsStack { get; } = new Stack<string>();

        private Stack<MultiTreeNode<Value>> OperandsStack { get; } = new Stack<MultiTreeNode<Value>>();

        public MultiTreeNode<Value> ParseLine(string line)
        {
            var currentToken = new TokenLine(line).GetEnumerator();

            while (currentToken.MoveNext())
                ParseToken(currentToken);

            while (OperationsStack.Count != 0)
                EvalOperator();

            var result = OperandsStack.Pop();
            OperationsStack.Clear();
            OperandsStack.Clear();
            return result;
        }

        private string EvalOperator()
        {
            var op = OperationsStack.Pop();

            if (op == ",")
                return op;

            if (!Operators.ContainsKey(op))
                throw new ArgumentException($"Expected operator, but was <{op}>");

            var opNode = new MultiTreeNode<Value>(new Value(ValueType.Function, op));
            opNode.AddChild(OperandsStack.Pop());
            opNode.AddChild(OperandsStack.Pop());
            OperandsStack.Push(opNode);
            return op;
        }

        private void OnClosingBracket()
        {
            var funcNode = new MultiTreeNode<Value>(new Value(ValueType.Function, null));

            while (OperationsStack.Peek() != "(")
                if (EvalOperator() == ",")
                    funcNode.AddChild(OperandsStack.Pop());

            OperationsStack.Pop();

            if (OperationsStack.Count > 0 && Functions.Contains(OperationsStack.Peek()))
            {
                funcNode.Data.Val = OperationsStack.Pop();
                OperandsStack.Push(funcNode);
                return;
            }

            if (funcNode.Count > 0)
            {
                funcNode.Data.Val = "MkTuple";
                OperandsStack.Push(funcNode);
            }
        }

        private void OnOpeningSquareBracket(TokenLine.TokenEnum token)
        {
            if (token.PeekPrev() == null || token.PeekPrev() == "(" || token.PeekPrev() == "[" || Operators.ContainsKey(token.PeekPrev()))
            {
                OperationsStack.Push("MkList");
                OperationsStack.Push("(");
                if (token.PeekNext() != "]")
                    OperationsStack.Push(",");
            }
            else
            {
                OperationsStack.Push("ByIdx");
                OperationsStack.Push("(");
            }
        }

        private void OnFunction(TokenLine.TokenEnum token)
        {
            if (token.PeekNext() == "(")
            {
                OperationsStack.Push(token.Current);
                OperationsStack.Push("(");
                token.MoveNext();
                if (token.PeekNext() != ")")
                    OperationsStack.Push(",");
            }
            else
                OperandsStack.Push(new MultiTreeNode<Value>(new Value(ValueType.Variable, token.Current)));
        }

        private void ParseToken(TokenLine.TokenEnum token)
        {
            if (Functions.Contains(token.Current))
                OnFunction(token);
            
            else if (token.Current == "[")
                OnOpeningSquareBracket(token);

            else if (RouteOperators.Contains(token.Current))
                OperationsStack.Push(token.Current);
            
            else if (token.Current == ")" || token.Current == "]")
                OnClosingBracket();
            
            else if (Operators.ContainsKey(token.Current))
            {
                while (OperationsStack.Count > 0 && Operators[OperationsStack.Peek()] >= Operators[token.Current])
                    EvalOperator();

                OperationsStack.Push(token.Current);
            }

            else
            {
                var value = Interpreter.ParseTypes(token.Current);
                OperandsStack.Push(value == null
                    ? new MultiTreeNode<Value>(new Value(ValueType.Variable, token.Current))
                    : new MultiTreeNode<Value>(new Value(ValueType.Value, value)));
            }
        }
    }
}