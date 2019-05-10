using System;
using Platformer.Entities;
using Platformer.Game;
using viper_script;

namespace Platformer.Files
{
    internal class EntityFile
    {
        private static Entity GetSolidEntity(Data data, World context)
        {
            var width = data.GetDouble("width");
            var height = data.GetDouble("height");

            var entity = context.CreateSolidEntity(new HitBox(0, 0, width, height));
            entity.Texture = data.GetString("texture");
            return entity;
        }

        private static Entity GetDecoration(Data data, World context)
        {
            var width = data.GetDouble("width");
            var height = data.GetDouble("height");

            var entity = context.CreateDecoration(new HitBox(0, 0, width, height));
            entity.Texture = data.GetString("texture");
            return entity;
        }

        public static Entity GetSolidEntity(string fname, World context)
        {
            var data = Interpreter.Interpret(fname);
            if (data.GetString("type") == "Entity")
                return GetSolidEntity(data, context);

            throw new NotImplementedException();
        }

        public static Entity GetDecoration(string fname, World context)
        {
            var data = Interpreter.Interpret(fname);
            if (data.GetString("type") == "Entity")
                return GetDecoration(data, context);

            throw new NotImplementedException();
        }
    }
}