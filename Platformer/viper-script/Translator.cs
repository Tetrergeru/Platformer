using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using viper_script.CodeBlocks;

namespace viper_script
{
    internal class Translator
    {
        private static readonly LineParser Parser = new LineParser();

        private List<string> Code { get; }
        
        #region Грёбаные регексы

        /// <summary>
        /// Проверяет строку на пустоту
        /// </summary>
        private static Regex Empty { get; } = new Regex(@"^\s*$");

        /// <summary>
        /// Проверяет, является ли строка комментарием
        /// </summary>
        private static Regex Comment { get; } = new Regex(@"^\s*(//)|(#)|(--).*$");

        /// <summary>
        /// Проверяет, является ли строка условным оператором.
        /// Если является -- записывает условие в группу condition
        /// </summary>
        private static Regex Condition { get; } = new Regex(@"^\s*if (?<condition>.*):\s*$");

        /// <summary>
        /// Проверяет, является ли строка веткой "иначе если".
        /// Если является -- записывает условие в группу condition
        /// </summary>
        private static Regex ElifBranch { get; } = new Regex(@"^\s*elif (?<condition>.*):\s*$");

        /// <summary>
        /// Проверяет, является ли строка веткой "иначе" условного оператора
        /// </summary>
        private static Regex ElseBranch { get; } = new Regex(@"^\s*else\s*:\s*$");

        private static Regex WhileCycle { get; } = new Regex(@"^\s*while (?<condition>.*):\s*$");

        /// <summary>
        /// Стандартный тип Integer
        /// </summary>
        private static Regex Integer { get; } = new Regex(@"^\s*(?<value>-?[0-9]+)\s*$");

        private static Regex Double { get; } = new Regex(@"^\s*(?<value>-?[0-9]+\.[0-9]+)\s*$");

        private static Regex Str { get; } = new Regex(@"^\s*'(?<value>.*)'\s*$");

        private static Regex Bool { get; } = new Regex(@"^\s*(?<value>(True)|(False))\s*$");

        #endregion

        public Translator(List<string> code)
        {
            Code = code;
        }

        public ICodeBlock Translate(ICodeBlock parent = null, int start = 0, int finish = -1)
        {
            if (finish == -1) finish = Code.Count;

            ICodeBlock result;

            if (parent != null)
                result = new ComplexBlock(parent);
            else
                result = new NamespaceBlock(null);

            var interpreted = new List<ICodeBlock>();

            var startPlainBlock = start;

            for (var i = start; i < finish; i++)
            {
                if (Empty.IsMatch(Code[i]) || Comment.IsMatch(Code[i]))
                    continue;

                var match = Condition.Match(Code[i]);
                if (match.Success)
                {
                    CheckForPlain(result, startPlainBlock, i, interpreted);
                    interpreted.Add(TranslateCondition(result, match.Groups["condition"].ToString(), i, out i));

                    startPlainBlock = i;
                    continue;
                }

                match = WhileCycle.Match(Code[i]);
                if (match.Success)
                {
                    CheckForPlain(result, startPlainBlock, i, interpreted);
                    interpreted.Add(TranslateWhile(result, match.Groups["condition"].ToString(), i, out i));

                    startPlainBlock = i + 1;
                    continue;
                }

            }

            CheckForPlain(result, startPlainBlock, finish, interpreted);

            if (interpreted.Count == 1 && parent != null)
            {
                result = interpreted[0];
                result.ParentBlock = parent;
            }
            else if (result is ComplexBlock complexBlock)
                complexBlock.Code = interpreted;
            else
                ((NamespaceBlock) result).Code = interpreted;

            return result;
        }

        private void CheckForPlain(ICodeBlock parent, int start, int finish, ICollection<ICodeBlock> destList)
        {
            if (start >= finish) return;

            var tp = TranslatePlain(parent, start, finish);
            if (tp != null)
                destList.Add(tp);
        }

        private PlainBlock TranslatePlain(ICodeBlock parent, int start, int finish)
        {
            var translated = new List<MultiTreeNode<Value>>();
            for (var i = start; i < finish; i++)
            {
                if (Empty.IsMatch(Code[i]) || Comment.IsMatch(Code[i]))
                    continue;
                translated.Add(Parser.ParseLine(Code[i]));
            }

            return translated.Count == 0 ? null : new PlainBlock(parent, translated);
        }

        private ConditionBlock TranslateCondition(ICodeBlock parent, string condition, int startPos, out int endPos)
        {
            var result = new ConditionBlock(parent, Parser.ParseLine(condition), null);
            endPos = FindBlockEnd(startPos + 1);

            result.IfCode = Translate(result, startPos + 1, endPos);

            if (endPos >= Code.Count)
                return result;

            var match = ElifBranch.Match(Code[endPos]);
            if (match.Success)
            {
                result.ElseCode = TranslateCondition(parent, match.Groups["condition"].ToString(),
                    endPos, out endPos);
            }
            else if (ElseBranch.IsMatch(Code[endPos]))
            {
                var endElse = FindBlockEnd(endPos + 1);
                result.ElseCode = Translate(result, endPos + 1, endElse);
                endPos = endElse;
            }

            return result;
        }

        private WhileBlock TranslateWhile(ICodeBlock parent, string condition, int startPos, out int endPos)
        {
            var result = new WhileBlock(parent, Parser.ParseLine(condition), null);
            endPos = FindBlockEnd(startPos + 1) - 1;
            result.Code = Translate(result, startPos + 1, endPos + 1);

            return result;
        }

        private int FindBlockEnd(int start)
        {
            var etalon = SpacesInBegin(start);
            while (start < Code.Count)
            {
                if (SpacesInBegin(start) < etalon)
                    return start;
                start++;
            }

            return start;
        }

        private int SpacesInBegin(int line)
        {
            var count = 0;
            var i = 0;
            while (Code[line].Length > i && (Code[line][i] == ' ' || Code[line][i] == '\t'))
            {
                count += (Code[line][i] == '\t') ? 4 : 1;
                i++;
            }
            return count;
        }

        public static object ParseTypes(string rvalue)
        {
            if (Integer.IsMatch(rvalue))
                return int.Parse(Integer.Match(rvalue).Groups["value"].ToString());

            if (Double.IsMatch(rvalue))
                return double.Parse(Double.Match(rvalue).Groups["value"].ToString(), CultureInfo.InvariantCulture);

            if (Str.IsMatch(rvalue))
                return Str.Match(rvalue).Groups["value"].ToString().Replace("\\n", "\n");

            if (Bool.IsMatch(rvalue))
                return Bool.Match(rvalue).Groups["value"].ToString() == "True";

            return null;
        }
    }
}