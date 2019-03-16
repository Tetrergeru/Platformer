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
        internal Dictionary<string, Container> Variables { get; }

        internal Data(Dictionary<string, Container> variables)
        {
            Variables = variables;
        }
        
        public string GetString(string index)
            => Variables[index].GetString();

        public int GetInt(string index)
            => Variables[index].GetInt();

        public double GetDouble(string index)
            => Variables[index].GetDouble();

        public List<Container> GetList(string index)
            => Variables[index].GetList();
    }

    public static class Interpreter
    {
        public static Data Interpret(List<string> code, Dictionary<string, object> context = null)
        {
            var translator = new Translator(code);
            var core = translator.Translate();
            if (context != null)
                foreach (var variable in context)
                    core.SetVariable(variable.Key, variable.Value);
            core.Interpret();
            if (core is NamespaceBlock block)
                return new Data(block.Variables);

            throw new Exception("Результатом трансляции был не корневой блок");
        }

        public static Data Interpret(string fname, Dictionary<string, object> context = null)
            => Interpret(File.ReadAllLines(fname).ToList(), context);

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