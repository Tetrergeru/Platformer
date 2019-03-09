using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using viper_script.CodeBlocks;

namespace viper_script
{
    public class Interpreter
    {
        private static LineParser parser = new LineParser();

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
        /// Проверяет, является ли строка веткой "иначе" условного оператора
        /// </summary>
        private static Regex ElseBranch { get; } = new Regex(@"^ *else *: *$");

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

            for (var i = start; i < finish;)
            {
                if (Empty.IsMatch(code[i]) || Comment.IsMatch(code[i]))
                {
                    i++;
                    continue;
                }

                var match = Condition.Match(code[i]);
                if (match.Success)
                {
                    if (startPlainBlock < finish)
                    {
                        var tp = TranslatePlain(code, result, startPlainBlock, i);
                        if (tp != null)
                            interpreted.Add(tp);
                    }

                    interpreted.Add(TranslateCondition(code, result, match.Groups["condition"].ToString(), i, out i));

                    startPlainBlock = i;
                    continue;
                }

                i++;
            }

            if (startPlainBlock < finish)
            {
                var tp = TranslatePlain(code, result, startPlainBlock, finish);
                if (tp != null)
                    interpreted.Add(tp);
            }

            if (interpreted.Count == 1)
                result = interpreted[0];
            else if (result is ComplexBlock complexBlock)
                complexBlock.Code = interpreted;
            else
                ((NamespaceBlock)result).Code = interpreted;

            return result;
        }

        private static PlainBlock TranslatePlain(List<string> code, ICodeBlock parent, int start, int finish)
        {
            var translated = new List<MultiTreeNode<string>>();
            for (var i = start; i < finish; i++)
            {
                if (Empty.IsMatch(code[i]) || Comment.IsMatch(code[i]))
                    continue;
                translated.Add(parser.ParseLine(code[i]));
            }

            return translated.Count == 0 ? null : new PlainBlock(parent, translated);
        }

        private static ConditionBlock TranslateCondition
            (List<string> code, ICodeBlock parent, string condition, int startPos, out int endPos)
        {
            var result = new ConditionBlock(parent, parser.ParseLine(condition), null);
            endPos = FindBlockEnd(code, startPos + 1);

            result.IfCode = Translate(code, result, startPos + 1, endPos);
            
            if (ElseBranch.IsMatch(code[endPos]))
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

        public static object Interpret(ICodeBlock context, MultiTreeNode<string> line)
        {
            throw new NotImplementedException();
        }
    }
}