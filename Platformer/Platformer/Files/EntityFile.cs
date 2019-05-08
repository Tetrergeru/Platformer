using System;
using Platformer.Entities;
using Platformer.Game;
using viper_script;

namespace Platformer.Files
{
    internal class EntityFile
    {
        private static Entity GetEntity(Data data, World context)
        {
            var width = data.GetInt("width");
            var height = data.GetInt("height");
            var entity = new Entity(new Vector {x = width, y = height})
            {
                Context = context,
                Texture = data.GetString("texture")
            };
            return entity;
        }

        public static Entity GetEntity(string fname, World context)
        {
            var data = Interpreter.Interpret(fname);
            if (data.GetString("type") == "Entity")
                return GetEntity(data, context);

            throw new NotImplementedException();
        }
    }
}