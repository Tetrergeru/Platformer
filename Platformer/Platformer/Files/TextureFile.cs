using System;
using viper_script;

namespace Platformer.Files
{
    public class TextureFile
    {
        private static TextureAnimated GetTextureAnimated(Data data)
        {
            throw new NotImplementedException();
        }

        public static ITexture GetTexture(string fname)
        {
            var data = Interpreter.Interpret(fname);
            if (data.GetString("type") == "TextureAnimated")
                return GetTextureAnimated(data);

            throw new NotImplementedException();
        }
    }
}