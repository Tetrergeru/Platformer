using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override string ToString()
        {
            return $"<{Val}: {Type.ToString()}({(Type == ValueType.Value ? Val.GetType() : null)})>";
        }
    }
}
