using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Platformer
{
    static class Controls
    {
        public enum Control { Jump, Right, Left, Stop, StopTime, RunTime, Debug };

        public static readonly Dictionary<Keys, Control> KeyToControl = new Dictionary<Keys, Control>
        {
            { Keys.W, Control.Jump },
            { Keys.D, Control.Right },
            { Keys.A, Control.Left },
            { Keys.S, Control.Stop },
            { Keys.Z, Control.StopTime },
            { Keys.X, Control.RunTime },
            { Keys.P, Control.Debug }
        };
    }
}
