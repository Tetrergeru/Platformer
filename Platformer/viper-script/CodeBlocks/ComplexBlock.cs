using System;
using System.Collections.Generic;

namespace viper_script.CodeBlocks
{
    public class ComplexBlock : ICodeBlock
    {
        public ICodeBlock ParentBlock { get; }

        public List<ICodeBlock> Code { get; set; }

        public Container GatVariable(string name) => ParentBlock.GatVariable(name);

        public void SetVariable(string name, object value) => ParentBlock.SetVariable(name, value);

        public ComplexBlock(ICodeBlock parentBlock, List<ICodeBlock> code = null)
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
            Console.WriteLine("ComplexBlock:");
            foreach (var block in Code)
                block.Print(offset + 1);
        }
    }
}