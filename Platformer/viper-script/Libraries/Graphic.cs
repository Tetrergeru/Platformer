using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace viper_script.Libraries
{
    internal static class Graphic
    {
        internal static Container Load(List<Container> variables)
        {
            if (variables.Count != 1)
                throw new ArgumentException("There should be one operand in load function");

            if (variables[0].value is string fname)
                return new Container(new Bitmap(fname));

            throw new Exception("Типы не совпадают");
        }

        internal static Container Create(List<Container> variables)
        {
            if (variables.Count != 2)
                throw new ArgumentException("There should be two operands in create function");

            if (variables[0].value is int width && variables[1].value is int height)
                return new Container(new Bitmap(width, height));

            throw new Exception("Типы не совпадают");
        }

        internal static Container Width(List<Container> variables)
        {
            if (variables.Count != 1)
                throw new Exception("There should be one operands in get_width function");
            if (variables[0].value is Bitmap image)
                return new Container(image.Width);
            throw new Exception("Типы не соврадают");
        }

        internal static Container Height(List<Container> variables)
        {
            if (variables.Count != 1)
                throw new Exception("There should be one operands in get_height function");
            if (variables[0].value is Bitmap image)
                return new Container(image.Height);
            throw new Exception("Типы не соврадают");
        }

        internal static Container Draw(List<Container> variables)
        {
            if (variables.Count != 5 || variables.Count != 4)
                throw new ArgumentException("There should be five operands in draw function");

            float scale = 1;
            if (variables.Count == 4)
            {
                switch (variables[4].value)
                {
                    case double sD:
                        scale = (float) sD;
                        break;
                    case int sI:
                        scale = sI;
                        break;
                    default:
                        throw new Exception("Тип данных не совпадает");
                }
            }

            if (!(variables[0].value is Bitmap image0) ||
                !(variables[1].value is Bitmap image1) ||
                !(variables[2].value is int x) ||
                !(variables[3].value is int y))
                return null;

            var graphics = Graphics.FromImage(image0);
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = PixelOffsetMode.Half;
            graphics.DrawImage(image1,new RectangleF(x,y,(image1.Width * scale), image1.Height*scale));

            return null;
        }
    }
}