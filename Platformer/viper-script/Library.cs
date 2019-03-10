using System;
using System.Collections.Generic;
using System.Linq;

namespace viper_script
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

        public static bool IsOperator(string token)
        {
            return Operators.ContainsKey(token);
        }

        public static bool IsFunction(string token)
        {
            return Functions.ContainsKey(token) && !IsOperator(token);
        }

        public static Dictionary<string, Func<List<Container>, Container>> Functions { get; }
            = new Dictionary<string, Func<List<Container>, Container>>
            {
                {"+=", Add},
                {"=", Assign},
                {"==", Equal},
                {"+", Plus},
                {"*", Multiply},
                {"-", Minus},
                {"<", Lesser},
                {"/", Divide},
                {"%", Mod},
                {"print", Print},
                {"MkList", MakeList},
                {"str", ToString},
                {"len", Length},
                {"ByIdx", ByIndex},
            };

        private static Container Assign(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in = operation");
            variables[0].value = variables[1].value;
            return null;
        }

        private static Container Equal(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in == operation");

            var a = variables[0].value;
            var b = variables[1].value;
            object result;

            switch (a)
            {
                case int x:
                {
                    switch (b)
                    {
                        case int y:
                            result = x == y;
                            break;
                        case double z:
                            result = Math.Abs(x - z) < 1e-10;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }

                    break;
                }
                case double x:
                {
                    switch (b)
                    {
                        case int y:
                            result = Math.Abs(x - y) < 1e-10;
                            break;
                        case double z:
                            result = Math.Abs(x - z) < 1e-10;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }

                    break;
                }
                case string s1:
                {
                    if (b is string s2)
                        result = s1 == s2;
                    else
                        throw new Exception("Типы не совпадают");
                    break;
                }
                default:
                    throw new Exception("Типы не совпадают");
            }

            return new Container(result);
        }

        private static Container Plus(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in += operation");

            var a = variables[0].value;
            var b = variables[1].value;
            object result;

            switch (a)
            {
                case int x:
                    {
                        switch (b)
                        {
                            case int y:
                                result = x + y;
                                break;
                            case double z:
                                result = x + z;
                                break;
                            default:
                                throw new Exception("Типы не совпадают");
                        }
                        break;
                    }
                case double x:
                    {
                        switch (b)
                        {
                            case int y:
                                result = x + y;
                                break;
                            case double z:
                                result = x + z;
                                break;
                            default:
                                throw new Exception("Типы не совпадают");
                        }
                        break;
                    }
                case string s1:
                    {
                        if (b is string s2)
                            result = string.Concat(s1, s2);
                        else
                            throw new Exception("Типы не совпадают");
                        break;
                    }
                case List<Container> list:
                    {
                        switch (b)
                        {
                            case List<Container> l2:
                                {
                                    var newList = new List<Container>();
                                    newList.AddRange(list);
                                    newList.AddRange(l2);
                                    result = newList;
                                    break;
                                }
                            case object obj:
                                {
                                    var newList = new List<Container>();
                                    newList.AddRange(list);
                                    newList.Add(new Container(obj));
                                    result = newList;
                                    break;
                                }
                            default:
                                throw new Exception("Типы не совпадают");
                        }
                        break;
                    }
                default:
                    throw new Exception("Типы не совпадают");
            }

            return new Container(result);
        }

        private static Container Print(List<Container> variables)
        {
            Console.Write($"{string.Join(" ", variables.Select(v => v.value))}");
            return null;
        }

        private static Container Add(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in += operation");

            var a = variables[0].value;
            var b = variables[1].value;

            switch (a)
            {
                case int x:
                {
                    switch (b)
                    {
                        case int y:
                            variables[0].value = x + y;
                            break;
                        case double z:
                            variables[0].value = x + z;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }
                    break;
                }
                case double x:
                    {
                        switch (b)
                        {
                            case int y:
                                variables[0].value = x + y;
                                break;
                            case double z:
                                variables[0].value = x + z;
                                break;
                            default:
                                throw new Exception("Типы не совпадают");
                        }

                        break;
                    }
                case string s1:
                {
                    if (b is string s2)
                        variables[0].value = string.Concat(s1, s2);
                    else
                        throw new Exception("Типы не совпадают");
                    break;
                }
                case List<Container> list:
                {
                    switch (b)
                    {
                        case List<Container> l2:
                        {
                            list.AddRange(l2);
                            break;
                        }
                        case object obj:
                        {
                            list.Add(new Container(obj));
                            break;
                        }
                        default:
                            throw new Exception("Типы не совпадают");
                    }
                    break;
                }
                default:
                    throw new Exception("Типы не совпадают");
            }

            return null;
        }

        private static Container Multiply(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in + operation");

            var a = variables[0].value;
            var b = variables[1].value;
            object result;

            switch (a)
            {
                case int x:
                {
                    switch (b)
                    {
                        case int y:
                            result = x * y;
                            break;
                        case double z:
                            result =x * z;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }
                    break;
                }
                case double x:
                {
                    if (b is double z)
                        result = x * z;
                    else
                        throw new Exception("Типы не совпадают");
                    break;
                }
                default:
                    throw new Exception("Типы не совпадают");
            }
            return new Container(result);
        }

        private static Container Minus(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in - operation");

            var a = variables[0].value;
            var b = variables[1].value;
            object result;

            switch (a)
            {
                case int x:
                {
                    switch (b)
                    {
                        case int y:
                            result = x - y;
                            break;
                        case double z:
                            result = x - z;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }

                    break;
                }
                case double x:
                {
                    switch (b)
                    {
                        case int y:
                            result = x - y;
                            break;
                        case double z:
                            result = x - z;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }

                    break;
                }

                default:
                    throw new Exception("Типы не совпадают");
            }

            return new Container(result);
        }

        private static Container Divide(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in / operation");

            var a = variables[0].value;
            var b = variables[1].value;
            object result;

            switch (a)
            {
                case int x:
                {
                    switch (b)
                    {
                        case int y:
                            result = x / y;
                            break;
                        case double z:
                            result = x / z;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }

                    break;
                }
                case double x:
                {
                    if (b is double z)
                        result = x / z;
                    else
                        throw new Exception("Типы не совпадают");
                    break;
                }
                default:
                    throw new Exception("Типы не совпадают");
            }

            return new Container(result);
        }

        private static Container Mod(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in + operation");

            var a = variables[0].value;
            var b = variables[1].value;

            if (a is int x && b is int y)
                return new Container(x % y);

            throw new Exception("Типы не совпадают");
        }

        private static Container Lesser(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in < operation");

            var a = variables[0].value;
            var b = variables[1].value;
            object result;

            switch (a)
            {
                case int x:
                {
                    switch (b)
                    {
                        case int y:
                            result = x < y;
                            break;
                        case double z:
                            result = x < z;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }

                    break;
                }
                case double x:
                {
                    switch (b)
                    {
                        case int y:
                            result = x < y;
                            break;
                        case double z:
                            result = x < z;
                            break;
                        default:
                            throw new Exception("Типы не совпадают");
                    }

                    break;
                }

                default:
                    throw new Exception("Типы не совпадают");
            }

            return new Container(result);
        }

        private static Container MakeList(List<Container> variables)
        {
            return new Container(variables);
        }

        private static Container ToString(List<Container> variables)
        {
            if (variables.Count != 1)
                throw new ArgumentException("There should be one operand in str() function");
            
            return new Container(ToString(variables[0]));
        }

        private static string ToString(Container variable)
        {
            switch (variable.value)
            {
                case List<Container> list:
                    return $"[{string.Join(", ",list.Select(ToString))}]";
                case string str:
                    return $"'{str}'";
                default:
                    return variable.value.ToString();
            }
        }

        private static Container Length(List<Container> variables)
        {
            if (variables.Count != 1)
                throw new ArgumentException("There should be one operand in len() function");

            switch (variables[0].value)
            {
                case List<Container> list:
                {
                    return new Container(list.Count);
                }
                case string str:
                {
                    return new Container(str.Length);
                }
                default:
                    throw new Exception("Типы не совпадают");
            }
        }

        private static Container ByIndex(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in ByIdx operator");

            if (!(variables[1].value is int idx))
                throw new Exception("Index should be int value");

            switch (variables[0].value)
            {
                case List<Container> list:
                {
                    return list[idx];
                }
                case string str:
                {
                    return new Container($"{str[idx]}");
                }
                default:
                    throw new Exception("Типы не совпадают");
            }
        }
    }
}