using System;

namespace viper_script.CodeBlocks
{
    internal class WhileBlock : ICodeBlock
    {
        public ICodeBlock ParentBlock { get; set; }

        public MultiTreeNode<Value> Condition { get; set; }

        public ICodeBlock Code { get; set; }

        public Container GetVariable(string name) => ParentBlock.GetVariable(name);

        public void SetVariable(string name, object value) => ParentBlock.SetVariable(name, value);

        public WhileBlock(ICodeBlock parentBlock, MultiTreeNode<Value> condition, ICodeBlock code)
        {
            ParentBlock = parentBlock;
            Condition = condition;
            Code = code;
        }

        public void Interpret()
        {
            while (Interpreter.Interpret(this, Condition).value is bool condition && condition)
            {
                Code.Interpret();
            }
        }

        public void Print(int offset = 0)
        {
            for (var i = 0; i < offset; i++)
                Console.Write("   ");

            Console.WriteLine($"WhileBlock {{{string.Join("  ", Condition.Traverse())}}}:");

            Code.Print(offset + 1);
        }
    }
}