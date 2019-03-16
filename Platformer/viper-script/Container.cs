using System;
using System.Collections.Generic;

namespace viper_script
{
    public class Container
    {
        public object value;

        public Container(object value)
        {
            this.value = value;
        }

        public string GetString()
        {
            if (value is string str)
                return str;
            throw new Exception("Значение было не string");
        }

        public int GetInt()
        {
            if (value is int x)
                return x;
            throw new Exception("Значение было не int");
        }

        public double GetDouble()
        {
            switch (value)
            {
                case int x:
                    return x;

                case double d:
                    return d;

                default:
                    throw new Exception("Значение было не double");
            }
        }

        public List<Container>  GetList()
        {
            if (value is List<Container> list)
                return list;
            throw new Exception("Значение было не List");
        }
    }
}