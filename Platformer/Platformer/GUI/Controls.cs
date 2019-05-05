using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Platformer.Game;

namespace Platformer.GUI
{
    public static class Controls
    {
        public static readonly Dictionary<Keys, ControlActions> KeyToControl = new Dictionary<Keys, ControlActions>
        {
            { Keys.W, ControlActions.Jump },

            { Keys.D, ControlActions.Right },

            { Keys.A, ControlActions.Left },

            { Keys.S, ControlActions.Stop },

            { Keys.Z, ControlActions.StopTime },
            { Keys.X, ControlActions.RunTime },
            { Keys.P, ControlActions.Debug },
            { Keys.F, ControlActions.Fly },
            { Keys.I, ControlActions.ScalePlus },
            { Keys.K, ControlActions.ScaleMinus },
        };

        public static ControlActions ControlFromKey(Keys key)
        {
            return KeyToControl.ContainsKey(key) ? KeyToControl[key] : ControlActions.None;
        }

        public static Keys KeyFromControl(ControlActions controlActions)
        {
            return KeyToControl.ContainsValue(controlActions)
                ? KeyToControl.First(kv => kv.Value == controlActions).Key
                : Keys.None;
        }

        public static void SetControl(ControlActions controlAction, Keys key)
        {
            if (KeyToControl.ContainsValue(controlAction))
                KeyToControl.Remove(KeyToControl.First(kv => kv.Value == controlAction).Key);

            KeyToControl[key] = controlAction;
        }
    }
}
