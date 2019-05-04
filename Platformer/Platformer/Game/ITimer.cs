using System;

namespace Platformer.Game
{
    public interface ITimer
    {
        void Start();

        void Stop();

        void AddEvent(Action<double> eventAction);
    }
}