using System;
using System.Collections.Generic;

namespace viper_script.CodeBlocks
{
    public class PlainBlock : ICodeBlock
    {
        public ICodeBlock ParentBlock { get; }

        public List<MultiTreeNode<string>> Code { get; set; }

        public Container GatVariable(string name) => ParentBlock.GatVariable(name);

        public void SetVariable(string name, object value) => ParentBlock.SetVariable(name, value);

        public PlainBlock(ICodeBlock parentBlock, List<MultiTreeNode<string>> code)
        {
            ParentBlock = parentBlock;
            Code = code;
        }

        public void Interpret()
        {
            foreach (var line in Code)
                Interpreter.Interpret(this, line);
        }

        public void Print(int offset = 0)
        {
            for (int i = 0; i < offset; i++) Console.Write("   ");
            Console.WriteLine("PlainBlock: ");
            foreach (var x in Code)
            {
                for (int i = 0; i <= offset; i++) Console.Write("   ");
                Console.WriteLine(string.Join(" ", x.Traverse()));
            }
        }
    }
}