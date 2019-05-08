using System;
using System.Collections.Generic;
using System.Drawing;
using viper_script;

namespace GUI
{
    public class TextureFile
    {
        private static TextureAnimated GetTextureAnimated(Data data)
        {
            var w = data.GetInt("width");
            var h = data.GetInt("height");
            var delay = data.GetDouble("delay");
            var delayAfter = data.GetDouble("delay_after");
            var result = new TextureAnimated(w,h,delay, delayAfter);
            foreach (var sprite in data.GetList("sprites"))
            {
                var s = sprite.GetList();
                var picture = new Bitmap(s[0].GetString());
                var ft = (FillType)Enum.Parse(typeof(FillType), s[1].GetString());
                var scale = s.Count > 2 ? s[2].GetDouble() : 1;
                result.AddTexture(picture, ft, scale);
            }

            return result;
        }

        private static Texture GetTexture(Data data)
        {
            var w = data.GetInt("width");
            var h = data.GetInt("height");
            var result = new Texture(w, h);

            if (data.Is<List<Container>>("sprite"))
            {
                var s = data.GetList("sprite");
                var picture = new Bitmap(s[0].GetString());
                var ft = (FillType) Enum.Parse(typeof(FillType), s[1].GetString());
                var scale = s.Count > 2 ? s[2].GetDouble() : 1;
                result.AddTexture(picture, ft, scale);
            }
            else if (data.Is<Bitmap>("sprite"))
            {
                var picture = data.Get<Bitmap>("sprite");
                result.AddTexture(picture, FillType.Repeat);
            }

            return result;
        }

        public static ITexture GetTexture(string fname)
        {
            var data = Interpreter.Interpret(fname);

            if (data.GetString("type") == "TextureAnimated")
                return GetTextureAnimated(data);

            if (data.GetString("type") == "Texture")
                return GetTexture(data);

            throw new NotImplementedException();
        }
    }
}