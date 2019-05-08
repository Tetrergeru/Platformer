using System.Windows.Forms;

namespace GUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = Platformer.Platformer.CreateGame();
            var gameWindow = new Window(game);
            game.Start();
            Application.Run(gameWindow);
        }
    }
}
