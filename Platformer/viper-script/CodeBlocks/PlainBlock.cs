using System.Collections.Generic;
using System.Linq;

namespace viper_script.CodeBlocks
{
    public class PlainBlock : ICodeBlock
    {
        public ICodeBlock ParentBlock { get; }

        private List<MultiTreeNode<string>> Code { get; }

        public Container GatVariable(string name) => ParentBlock.GatVariable(name);

        public void SetVariable(string name, object value) => ParentBlock.SetVariable(name, value);

        public PlainBlock(ICodeBlock parentBlock, IEnumerable<string> code)
        {
            ParentBlock = parentBlock;
            var lp = new LineParser();
            Code = code.Select(s => lp.ParseLine(s)).ToList();
        }

        public void Interpret()
        {
            foreach (var line in Code)
                Interpreter.Interpret(this, line);
        }
    }
}