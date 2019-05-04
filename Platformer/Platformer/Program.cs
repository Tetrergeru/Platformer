using System;
using System.Windows.Forms;
using Platformer.GUI;

namespace Platformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game.Game(new Game.Timer(10));
            var gameWindow = new Window(game);
            game.Start();
            Application.Run(gameWindow);
        }
    }
}
