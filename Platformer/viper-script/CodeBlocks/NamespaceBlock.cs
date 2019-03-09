using System;
using System.Collections.Generic;

namespace viper_script.CodeBlocks
{
    public class NamespaceBlock : ICodeBlock
    {
        private Dictionary<string, Container> Variables { get; } = new Dictionary<string, Container>();

        public ICodeBlock ParentBlock { get; }

        public List<ICodeBlock> Code { get; set; }

        public Container GatVariable(string name)
        {
            if (Variables.ContainsKey(name))
                return Variables[name];

            if (ParentBlock != null)
                return ParentBlock.GatVariable(name);

            throw new ArgumentException($"Variable <{name}> not in scope");
        }

        public void SetVariable(string name, object value)
        {
            if (Variables.ContainsKey(name))
                Variables[name].value = value;
            else
                Variables[name] = new Container(value);
        }

        public NamespaceBlock(ICodeBlock parentBlock, List<ICodeBlock> code = null)
        {
            ParentBlock = parentBlock;
            Code = code;
        }

        public void Interpret()
        {
            foreach (var line in Code)
                line.Interpret();
        }

        public void Print(int offset = 0)
        {
            for (int i = 0; i < offset; i++) Console.Write("   ");
            Console.WriteLine("NamespaceBlock: ");
            foreach (var block in Code)
                block.Print(offset + 1);
        }
    }
}