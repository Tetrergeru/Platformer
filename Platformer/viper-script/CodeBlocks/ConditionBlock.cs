using System;

namespace viper_script.CodeBlocks
{
    internal class ConditionBlock : ICodeBlock
    {
        public ICodeBlock ParentBlock { get; set; }

        public MultiTreeNode<Value> Condition { get; set; }

        public ICodeBlock IfCode { get; set; }

        public ICodeBlock ElseCode { get; set; }

        public Container GetVariable(string name) => ParentBlock.GetVariable(name);

        public void SetVariable(string name, object value) => ParentBlock.SetVariable(name, value);

        public ConditionBlock(ICodeBlock parentBlock, MultiTreeNode<Value> condition,
            ICodeBlock ifCode, ICodeBlock elseCode = null)
        {
            ParentBlock = parentBlock;
            Condition = condition;
            IfCode = ifCode;
            ElseCode = elseCode;
        }

        public void Interpret()
        {
            if (Interpreter.Interpret(this, Condition) is bool condition)
            {
                if (condition)
                    IfCode.Interpret();
                else
                    ElseCode.Interpret();
            }
            else
            {
                throw new ArgumentException("Logical expression was expected");
            }
        }

        public void Print(int offset = 0)
        {
            for (int i = 0; i < offset; i++) Console.Write("   ");

            Console.WriteLine($"ConditionBlock {{{string.Join("  ", Condition.Traverse())}}}:");

            IfCode.Print(offset + 1);

            if (ElseCode != null)
            {
                for (var i = 0; i < offset; i++) Console.Write("   ");
                Console.WriteLine("ElseCode:");
                ElseCode.Print(offset + 1);
            }
        }
    }
}