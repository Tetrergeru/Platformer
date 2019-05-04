using System;
using System.Threading;

namespace Platformer.Game
{
    public class Timer : ITimer
    {
        private readonly int _delay;

        private readonly System.Threading.Timer _timer;
        
        private Action<double> _timerCallbacks;

        private void TimerCallback(object o)
        {
            _timerCallbacks(_delay/1000.0);
        }

        public Timer(int delay)
        {
            _delay = delay;
            _timer = new System.Threading.Timer(TimerCallback, null,Timeout.Infinite, delay);
        }

        public void Start()
        {
            _timer.Change(0, _delay);
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, _delay);
        }

        public void AddEvent(Action<double> eventAction)
        {
            _timerCallbacks += eventAction;
        }
    }
}