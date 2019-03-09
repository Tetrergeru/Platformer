namespace viper_script.CodeBlocks
{
    public interface ICodeBlock
    {
        ICodeBlock ParentBlock { get; }

        Container GatVariable(string name);

        void SetVariable(string name, object value);

        void Interpret();
    }
}