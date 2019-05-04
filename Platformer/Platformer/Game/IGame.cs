namespace Platformer.Game
{
    public interface IGame
    {
        StateSnapshot GetStateSnapshot();

        void Action(string actionData);

        void Stop();

        void Start();
    }
}