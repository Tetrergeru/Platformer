﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using viper_script.CodeBlocks;

namespace viper_script
{
    public class Interpreter
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

            if (interpreted.Count == 1)
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
            var translated = new List<MultiTreeNode<string>>();
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

        public static object Interpret(ICodeBlock context, MultiTreeNode<string> line)
        {
            throw new NotImplementedException();
        }
    }
}