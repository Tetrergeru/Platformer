using System;
using System.Windows.Forms;

namespace Platformer
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            var gameWindow = new Window(game);
            new System.Threading.Thread(() => Application.Run(gameWindow)).Start();

            game.window = gameWindow;
            game.Start();
        }
    }
}
