using System;
using System.Drawing;
using Platformer.Entities;
using Platformer.Game;
using viper_script;

namespace Platformer.Files
{
    internal class WorldFile
    {
        public static Entity GetSolidEntity(Container e, World context)
        {
            var ent = e.GetList();
            var entity = EntityFile.GetSolidEntity(ent[0].GetString(), context);
            entity.Move(new Vector { x = ent[1].GetDouble(), y = ent[2].GetDouble() });
            if (ent.Count > 3)
                entity.DrawPriority = ent[3].GetDouble();
            return entity;
        }

        public static Entity GetDecoration(Container e, World context)
        {
            var ent = e.GetList();
            var entity = EntityFile.GetDecoration(ent[0].GetString(), context);
            entity.Move(new Vector { x = ent[1].GetDouble(), y = ent[2].GetDouble() });
            if (ent.Count > 3)
                entity.DrawPriority = ent[3].GetDouble();
            return entity;
        }

        public static void FillWorld(World world, string fname)
        {
            var data = Interpreter.Interpret(fname);

            var color = data.GetList("background_color");
            //world.BackGroundColor = Color.FromArgb(color[0].GetInt(), color[1].GetInt(), color[2].GetInt());

            foreach (var entity in data.GetList("blocks"))
                GetSolidEntity(entity, world);

            foreach (var entity in data.GetList("decorations"))
                GetDecoration(entity, world);
        }
    }
}