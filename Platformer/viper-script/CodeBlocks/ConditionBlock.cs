using System;
using System.Collections.Generic;
using System.Linq;

namespace viper_script.CodeBlocks
{
    public class ConditionBlock : ICodeBlock
    {
        public ICodeBlock ParentBlock { get; }

        private MultiTreeNode<string> Condition { get; }

        private ICodeBlock IfCode { get; }

        private ICodeBlock ElseCode { get; }

        public Container GatVariable(string name) => ParentBlock.GatVariable(name);

        public void SetVariable(string name, object value) => ParentBlock.SetVariable(name, value);

        public ConditionBlock(ICodeBlock parentBlock, MultiTreeNode<string> condition,
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
    }
}