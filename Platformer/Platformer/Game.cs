using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace Platformer
{
    class Game
    {
        private const int TickTime = 10;

        public Window window;

        public World world;

        public Player Player
        {
            get
            {
                return world.player;
            }
        }

        private Timer gameLoop;

        private void Tick(object sender, ElapsedEventArgs e)
        {
            world.player.Move(TickTime/1000.0);
            window.Clear();
            window.Draw(Color.Red, world.player.hitbox.ToDrawing());
            foreach(var x in world.block)
                window.Draw(Color.Blue, x.hitbox.ToDrawing());
            window.Flush();
        }

        public Game()
        {
            gameLoop = new Timer(TickTime);
            gameLoop.Elapsed += Tick;

            world = new World();
        }

        public void Start()
        {
            gameLoop.Start();
        }
    }
}
