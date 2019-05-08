using Platformer.Game;

namespace Platformer
{
    public static class Platformer
    {
        public static IGame CreateGame()
        {
            return new Game.Game(new Game.Timer(10));
        }
    }
}
