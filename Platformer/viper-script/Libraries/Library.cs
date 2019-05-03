using System;
using System.Collections.Generic;
using System.Linq;

namespace viper_script.Libraries
{
    internal class Library
    {
        public static Dictionary<string, int> Operators { get; } = new Dictionary<string, int>
        {
            {"(", -100},
            {"[", -100},
            {"=", -50},
            {"+=", -50},
            {"<", 0},
            {",", 0},
            {"+", 1},
            {"==", 1},
            { "-",1},
            { "*",2},
            { "/",2},
            { "%",2},
            { "ByIdx",5},
        };
        
        public static Dictionary<string, Func<List<Container>, Container>> Functions { get; }
            = new Dictionary<string, Func<List<Container>, Container>>
            {
                //Standard
                {"+=", Standard.Add},
                {"=", Standard.Assign},
                {"==", Standard.Equal},
                {"+", Standard.Plus},
                {"*", Standard.Multiply},
                {"-", Standard.Minus},
                {"<", Standard.Lesser},
                {"/", Standard.Divide},
                {"%", Standard.Mod},
                {"print", Standard.Print},
                {"MkList", Standard.MakeList},
                {"str", Standard.ToString},
                {"len", Standard.Length},
                {"ByIdx", Standard.ByIndex},
                {"random", Standard.MakeRandom},
                //Graphic
                { "load_img", Graphic.Load},
                { "create_img", Graphic.Create},
                { "get_width", Graphic.Width},
                { "get_height", Graphic.Height},
                { "draw", Graphic.Draw},

            };

        public static bool IsOperator(string token)
        {
            return Operators.ContainsKey(token);
        }

        public static bool IsFunction(string token)
        {
            return Functions.ContainsKey(token) && !IsOperator(token);
        }
    }
}