using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using viper_script.CodeBlocks;

namespace viper_script
{
    public class Data
    {
        private Dictionary<string, Container> Variables { get; }

        internal Data(Dictionary<string, Container> variables)
        {
            Variables = variables;
        }

        public string GetString(string index)
        {
            if (Variables[index].value is string str)
                return str;
            throw new Exception("Значение было не string");
        }

        public int GetInt(string index)
        {
            if (Variables[index].value is int x)
                return x;
            throw new Exception("Значение было не int");
        }
    }

    public static class Interpreter
    {
        public static Data Interpret(List<string> code)
        {
            var translator = new Translator(code);
            var core = translator.Translate();
            core.Interpret();
            if (core is NamespaceBlock block)
                return new Data(block.Variables);

            throw new Exception("Результатом трансляции был не корневой блок");
        }

        public static Data Interpret(string fname)
            => Interpret(File.ReadAllLines(fname).ToList());

        internal static Container Interpret(ICodeBlock context, MultiTreeNode<Value> line)
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