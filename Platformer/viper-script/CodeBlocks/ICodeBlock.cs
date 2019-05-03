namespace viper_script.CodeBlocks
{
    public interface ICodeBlock
    {
        ICodeBlock ParentBlock { get; set; }

        Container GetVariable(string name);

        void SetVariable(string name, object value);

        void Interpret();

        void Print(int offset = 0);
    }
}