using System;
using System.Collections.Generic;

namespace viper_script.CodeBlocks
{
    internal class PlainBlock : ICodeBlock
    {
        public ICodeBlock ParentBlock { get; set; }

        public List<MultiTreeNode<Value>> Code { get; set; }

        public Container GetVariable(string name) => ParentBlock.GetVariable(name);

        public void SetVariable(string name, object value) => ParentBlock.SetVariable(name, value);

        public PlainBlock(ICodeBlock parentBlock, List<MultiTreeNode<Value>> code)
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