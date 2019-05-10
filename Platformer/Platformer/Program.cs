using Platformer.Game;

namespace Platformer
{
    public static class Platformer
    {
        public static IGame CreateGame(ITimer timer = null)
        {
            if (timer == null)
                timer = new Timer(10);
            return new Game.Game(timer);
        }
    }
}
