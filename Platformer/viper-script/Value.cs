using System;
using System.Collections.Generic;

namespace viper_script
{
    internal enum ValueType
    {
        Variable,
        Function,
        Value,
    }

    internal class Value
    {
        public ValueType Type { get; }

        public object Val { get; set; }
        
        public Value(ValueType type, object value)
        {
            Type = type;
            Val = value;
        }

        private static readonly Dictionary<Type, string> Typenames = new Dictionary<Type, string>
        {
            { typeof(int), "int"},
            { typeof(string), "str"},
            { typeof(List<>), "lst"},
            { typeof(bool), "bool"},
            { typeof(double), "double"},
        };

        public override string ToString()
        {
            return $"<{Val}: {(Type == ValueType.Function ? "Func" : Typenames[Val.GetType()])}>";
        }
    }
}
