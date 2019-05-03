using System;
using System.Windows.Forms;
using Platformer.GUI;

namespace Platformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            var gameWindow = new Window(game);
            game.window = gameWindow;
            game.Start();
            Application.Run(gameWindow);
        }
    }
}
