using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using viper_script.CodeBlocks;

namespace viper_script
{
    public static class Interpreter
    {
        public static Dictionary<string, Container> Interpret(List<string> code)
        {
            var translator = new Translator(code);
            var core = translator.Translate();
            core.Interpret();
            if (core is NamespaceBlock block)
                return block.Variables;

            throw new Exception("Результатом трансляции был не корневой блок");
        }

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