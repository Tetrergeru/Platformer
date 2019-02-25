using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Platformer
{
    static class Controls
    {
        public enum Control { None, Jump, Right, Left, Stop, StopTime, RunTime, Debug };

        public static readonly Dictionary<Keys, Control> KeyToControl = new Dictionary<Keys, Control>
        {
            { Keys.W, Control.Jump },
            { Keys.Up, Control.Jump },

            { Keys.D, Control.Right },
            { Keys.Right, Control.Right },

            { Keys.A, Control.Left },
            { Keys.Left, Control.Left },

            { Keys.S, Control.Stop },
            { Keys.Down, Control.Stop },

            { Keys.Z, Control.StopTime },
            { Keys.X, Control.RunTime },
            { Keys.P, Control.Debug }
        };

        public static Control ControlFromKey(Keys key)
        {
            if (KeyToControl.ContainsKey(key))
                return KeyToControl[key];
            else return Control.None;
        }
    }
}
