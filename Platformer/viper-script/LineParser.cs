using System;
using System.Collections.Generic;

namespace viper_script
{
    internal class LineParser
    {
        private static HashSet<string> RouteOperators { get; } = new HashSet<string> {"(", ".", ","};
        
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

            if (!Library.Operators.ContainsKey(op))
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

            if (OperationsStack.Count > 0 && Library.IsFunction(OperationsStack.Peek()))
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

        private void OnClosingSquareBracket()
        {
            var funcNode = new MultiTreeNode<Value>(new Value(ValueType.Function, null));

            while (OperationsStack.Peek() != "[")
                if (EvalOperator() == ",")
                    funcNode.AddChild(OperandsStack.Pop());

            OperationsStack.Pop();
            
            if (OperationsStack.Count > 0 && OperationsStack.Peek() == "MkList")
            {
                funcNode.Data.Val = OperationsStack.Pop();
                OperandsStack.Push(funcNode);
                return;
            }

            if (OperationsStack.Count > 0 && OperationsStack.Peek() == "ByIdx")
            {
                funcNode.Data.Val = OperationsStack.Pop();
                funcNode.AddChild(OperandsStack.Pop());
                funcNode.AddChild(OperandsStack.Pop());
                OperandsStack.Push(funcNode);
                return;
            }
        }

        private void OnOpeningSquareBracket(TokenLine.TokenEnum token)
        {
            if (token.PeekPrev() == null || token.PeekPrev() == "(" || token.PeekPrev() == "[" ||
                Library.Operators.ContainsKey(token.PeekPrev()))
            {
                OperationsStack.Push("MkList");
                OperationsStack.Push("[");
                if (token.PeekNext() != "]")
                    OperationsStack.Push(",");
            }
            else
            {
                OperationsStack.Push("ByIdx");
                OperationsStack.Push("[");
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
            if (Library.IsFunction(token.Current))
                OnFunction(token);
            
            else if (token.Current == "[")
                OnOpeningSquareBracket(token);

            else if (RouteOperators.Contains(token.Current))
                OperationsStack.Push(token.Current);
            
            else if (token.Current == ")")
                OnClosingBracket();

            else if (token.Current == "]")
                OnClosingSquareBracket();

            else if (Library.IsOperator(token.Current))
            {
                while (OperationsStack.Count > 0 && Library.Operators[OperationsStack.Peek()] >= Library.Operators[token.Current])
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