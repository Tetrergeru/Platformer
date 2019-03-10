using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using viper_script.CodeBlocks;

namespace viper_script
{
    internal class Library
    {
        public static Dictionary<string, int> Operators { get; } = new Dictionary<string, int>
        {
            {"(", -100},
            {"=", -1},
            {",", 0},
            {"+", 1},
            { "-",1},
            { "*",2},
            { "/",2},
            { "ByIdx",5},
        };

        public static Dictionary<string, Func<List<Container>, Container>> Functions
            = new Dictionary<string, Func<List<Container>, Container>>
            {
                {"=", Assign},
                {"+", Plus},
                {"print", Print},
            };

        private static Container Assign(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in assign operation");
            variables[0].value = variables[1].value;
            return null;
        }

        private static Container Print(List<Container> variables)
        {
            foreach (var var in variables)
                Console.Write($"{var.value} ");
            return null;
        }

        private static Container Plus(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in + operation");
            return new Container((int) variables[0].value + (int) variables[1].value);
        }
    }
}