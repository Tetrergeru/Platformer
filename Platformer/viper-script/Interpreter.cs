using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using viper_script.CodeBlocks;

namespace viper_script
{
    internal class Interpreter
    {
        private static readonly LineParser Parser = new LineParser();

        /// <summary>
        /// Проверяет строку на пустоту
        /// </summary>
        private static Regex Empty { get; } = new Regex(@"^ *$");

        /// <summary>
        /// Проверяет, является ли строка комментарием
        /// </summary>
        private static Regex Comment { get; } = new Regex(@"^ *(//)|(#)|(--).*$");

        /// <summary>
        /// Проверяет, является ли строка условным оператором.
        /// Если является -- записывает условие в группу condition
        /// </summary>
        private static Regex Condition { get; } = new Regex(@"^ *if (?<condition>.*): *$");

        /// <summary>
        /// Проверяет, является ли строка веткой "иначе если".
        /// Если является -- записывает условие в группу condition
        /// </summary>
        private static Regex ElifBranch { get; } = new Regex(@"^ *elif (?<condition>.*): *$");

        /// <summary>
        /// Проверяет, является ли строка веткой "иначе" условного оператора
        /// </summary>
        private static Regex ElseBranch { get; } = new Regex(@"^ *else *: *$");

        /// <summary>
        /// Стандартный тип Integer
        /// </summary>
        private static Regex Integer { get; } = new Regex(@"^ *(?<value>-?[0-9]+) *$");
        private static Regex Double { get; } = new Regex(@"^ *(?<value>-?[0-9]+\.[0-9]+) *$");
        private static Regex Str { get; } = new Regex(@"^ *'(?<value>.*)' *$");
        private static Regex Bool { get; } = new Regex(@"^ *(?<value>(True)|(False)) *$");


        public static ICodeBlock Translate(List<string> code, ICodeBlock parent = null, int start = 0, int finish = -1)
        {
            if (finish == -1) finish = code.Count;

            ICodeBlock result;

            if (parent != null)
                result = new ComplexBlock(parent);
            else
                result = new NamespaceBlock(null);

            var interpreted = new List<ICodeBlock>();

            var startPlainBlock = start;

            for (var i = start; i < finish; i++)
            {
                if (Empty.IsMatch(code[i]) || Comment.IsMatch(code[i]))
                    continue;

                var match = Condition.Match(code[i]);
                if (match.Success)
                {
                    CheckForPlain(code, result, startPlainBlock, i, interpreted);

                    interpreted.Add(TranslateCondition(code, result, match.Groups["condition"].ToString(), i, out i));

                    startPlainBlock = i;
                    continue;
                }
            }

            CheckForPlain(code, result, startPlainBlock, finish, interpreted);

            if (interpreted.Count == 1 && parent != null)
            {
                result = interpreted[0];
                result.ParentBlock = parent;
            }
            else if (result is ComplexBlock complexBlock)
                complexBlock.Code = interpreted;
            else
                ((NamespaceBlock)result).Code = interpreted;

            return result;
        }

        private static void CheckForPlain
            (IReadOnlyList<string> code, ICodeBlock parent, int start, int finish, ICollection<ICodeBlock> destList)
        {
            if (start >= finish) return;

            var tp = TranslatePlain(code, parent, start, finish);
            if (tp != null)
                destList.Add(tp);
        }

        private static PlainBlock TranslatePlain(IReadOnlyList<string> code, ICodeBlock parent, int start, int finish)
        {
            var translated = new List<MultiTreeNode<Value>>();
            for (var i = start; i < finish; i++)
            {
                if (Empty.IsMatch(code[i]) || Comment.IsMatch(code[i]))
                    continue;
                translated.Add(Parser.ParseLine(code[i]));
            }

            return translated.Count == 0 ? null : new PlainBlock(parent, translated);
        }

        private static ConditionBlock TranslateCondition
            (List<string> code, ICodeBlock parent, string condition, int startPos, out int endPos)
        {
            var result = new ConditionBlock(parent, Parser.ParseLine(condition), null);
            endPos = FindBlockEnd(code, startPos + 1);

            result.IfCode = Translate(code, result, startPos + 1, endPos);

            var match = ElifBranch.Match(code[endPos]);
            if (match.Success)
            {
                result.ElseCode = TranslateCondition(code, parent, match.Groups["condition"].ToString(),
                    endPos, out endPos);
            }
            else if (ElseBranch.IsMatch(code[endPos]))
            {
                var endElse = FindBlockEnd(code, endPos + 1);
                result.ElseCode = Translate(code, result, endPos + 1, endElse);
                endPos = endElse;
            }

            return result;
        }

        private static int FindBlockEnd(IReadOnlyList<string> code, int start)
        {
            var etalon = SpacesInBegin(code, start);
            while (start < code.Count)
            {
                if (SpacesInBegin(code, start) < etalon)
                    return start;
                start++;
            }

            return start;
        }

        private static int SpacesInBegin(IReadOnlyList<string> code, int line)
        {
            var count = 0;
            while (code[line].Length > count && code[line][count] == ' ')
                count++;
            return count;
        }

        public static  object ParseTypes(string rvalue)
        {
            if (Integer.IsMatch(rvalue))
                return int.Parse(Integer.Match(rvalue).Groups["value"].ToString());

            if (Double.IsMatch(rvalue))
                return double.Parse(Double.Match(rvalue).Groups["value"].ToString(), CultureInfo.InvariantCulture);

            if (Str.IsMatch(rvalue))
                return Str.Match(rvalue).Groups["value"].ToString();

            if (Bool.IsMatch(rvalue))
                return Bool.Match(rvalue).Groups["value"].ToString() == "True";

            return null;
        }
        
        public static Container Interpret(ICodeBlock context, MultiTreeNode<Value> line)
        {
            switch (line.Data.Type)
            {
                case ValueType.Function when line.Data.Val is string fname:
                    if (fname == "=" && line.PeekChild().Data.Type == ValueType.Variable)
                    {
                        context.SetVariable(line.PeekChild().Data.Val as string,
                            Interpret(context, line.PeekChild().PeekSibling()).value);
                        return null;
                    }
                    else
                    {
                        return Library.Functions[fname]
                            (line.GetChildren().Select(v => Interpret(context, v)).ToList());
                    }

                case ValueType.Value:
                    return new Container(line.Data.Val);

                case ValueType.Variable when line.Data.Val is string vname:
                    return context.GetVariable(vname);

                default:
                    return null;
            }
        }
    }
}