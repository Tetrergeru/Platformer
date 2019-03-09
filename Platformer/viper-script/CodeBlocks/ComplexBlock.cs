using System.Collections.Generic;

namespace viper_script.CodeBlocks
{
    public class ComplexBlock : ICodeBlock
    {
        public ICodeBlock ParentBlock { get; }

        private List<ICodeBlock> Code { get; }

        public Container GatVariable(string name) => ParentBlock.GatVariable(name);

        public void SetVariable(string name, object value) => ParentBlock.SetVariable(name, value);

        public ComplexBlock(ICodeBlock parentBlock, List<ICodeBlock> code)
        {
            ParentBlock = parentBlock;
            Code = code;
        }

        public void Interpret()
        {
            foreach (var line in Code)
                line.Interpret();
        }
    }
}