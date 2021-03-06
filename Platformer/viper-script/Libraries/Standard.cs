﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace viper_script.Libraries
{
    internal static class Standard
    {
        internal static Container Assign(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in = operation");
            variables[0].value = variables[1].value;
            return null;
        }

        internal static Container Equal(List<Container> variables)
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

        internal static Container Plus(List<Container> variables)
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

        internal static Container Print(List<Container> variables)
        {
            Console.WriteLine($"{string.Join(" ", variables.Select(v => v.value))}");
            return null;
        }

        internal static Container Add(List<Container> variables)
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

        internal static Container Multiply(List<Container> variables)
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
                                result = x * z;
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

        internal static Container Minus(List<Container> variables)
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

        internal static Container Divide(List<Container> variables)
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

        internal static Container Mod(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in + operation");

            var a = variables[0].value;
            var b = variables[1].value;

            if (a is int x && b is int y)
                return new Container(x % y);

            throw new Exception("Типы не совпадают");
        }

        internal static Container Lesser(List<Container> variables)
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

        internal static Container MakeList(List<Container> variables)
        {
            return new Container(variables);
        }

        internal static Container ToString(List<Container> variables)
        {
            if (variables.Count != 1)
                throw new ArgumentException("There should be one operand in str() function");

            return new Container(ToString(variables[0]));
        }

        internal static string ToString(Container variable)
        {
            switch (variable.value)
            {
                case List<Container> list:
                    return $"[{string.Join(", ", list.Select(ToString))}]";
                case string str:
                    return $"'{str}'";
                default:
                    return variable.value.ToString();
            }
        }

        internal static Container Length(List<Container> variables)
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

        internal static Container ByIndex(List<Container> variables)
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

        private static readonly Random Random = new Random();

        internal static Container MakeRandom(List<Container> variables)
        {
            switch (variables.Count)
            {
                case 0:
                    return new Container(Random.NextDouble());

                case 1:
                    switch (variables[0].value)
                    {
                        case int a:
                            return new Container(Random.Next(a));

                        case List<Container> list:
                            return new Container(list[Random.Next(list.Count)]);

                        default:
                            throw new Exception("Неправильные параметры в функции random()");
                    }

                case 2 when variables[0].value is int x && variables[1].value is int y:
                    return new Container(Random.Next(x, y));

                default:
                    throw new Exception("Неправильные параметры в функции random()");
            }
        }
    }
}