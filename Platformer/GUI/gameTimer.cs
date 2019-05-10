using System;
using System.Threading;
using Platformer.Game;

namespace GUI
{
    class GameTimer : ITimer
    {
        private bool _running;

        readonly Thread _thread;

        private Action<double> _eventActions;
        public GameTimer()
        {
            _thread = new Thread(TimerThread);
            _thread.Start();
        }

        private void TimerThread()
        {
            double deltatime = 0;
            while (true)
            {
                var startTime = DateTime.Now;
                if (_running)
                {
                    _eventActions(deltatime / 1000);
                }
                deltatime = DateTime.Now.Subtract(startTime).TotalMilliseconds;
            }
        }

        public void Start()
        {
            _running = true;
        }

        public void Stop()
        {
            _running = false;
        }

        public void AddEvent(Action<double> eventAction)
        {
            _eventActions += eventAction;
        }
    }
}
