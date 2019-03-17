using System;
using System.Drawing;
using viper_script;

namespace Platformer.Files
{
    internal class WorldFile
    {
        public static Entity GetEntity(Container e, World context)
        {
            var ent = e.GetList();
            var entity = EntityFile.GetEntity(ent[0].GetString(), context);
            entity.Move(new Vector { x = ent[1].GetDouble(), y = ent[2].GetDouble() });
            return entity;
        }

        public static  World GetWorld(string fname)
        {
            var data = Interpreter.Interpret(fname);
            var world = new World();

            var color = data.GetList("background_color");
            world.BackGroundColor = Color.FromArgb(color[0].GetInt(), color[1].GetInt(), color[2].GetInt());

            foreach (var entity in data.GetList("blocks"))
                world.Block.Add(GetEntity(entity, world));

            foreach (var entity in data.GetList("decorations"))
                world.Background.Add(GetEntity(entity, world));

            return world;
        }
    }
}