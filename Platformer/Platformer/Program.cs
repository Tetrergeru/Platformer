using Platformer.Game;
using Platformer.Physics;

namespace Platformer
{
    public static class Platformer
    {
        public static IGame CreateGame(ITimer timer = null, IPhysics physics = null)
        {
            if (timer == null)
                timer = new Timer(10);
            return new Game.Game(timer, new Physics.Physics());
        }
    }
}
