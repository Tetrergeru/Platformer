using System.Windows.Forms;

namespace GUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var game = Platformer.Platformer.CreateGame(new GameTimer());
            var gameWindow = new Window(game);
            game.Start();
            Application.Run(gameWindow);
        }
    }
}