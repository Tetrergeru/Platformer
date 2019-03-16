using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
using System.Windows.Forms;
using viper_script;

namespace Platformer.Files
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

        public static ITexture GetTexture(string fname, int width = -1, int height = -1)
        {
            var data = Interpreter.Interpret(fname,
                new Dictionary<string, object> {{"width", width}, {"height", height}});

            if (data.GetString("type") == "TextureAnimated")
                return GetTextureAnimated(data);

            throw new NotImplementedException();
        }
    }
}