using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Platformer
{
    internal class MenuChangeControls : Menu
    {
        private Dictionary<ControlActions, Button> button = new Dictionary<ControlActions, Button>();

        private Panel panel { get; }

        private Button ExitButton;

        private ControlActions Choosing = ControlActions.None;

        public MenuChangeControls(Window owner) : base(owner)
        {
            ExitButton = new Button(200, 50)
            {
                Location = new Point((owner.Width- 200)/2, 20),
            };
            ExitButton.Click += Exit;
            ExitButton.SetText("Exit");
            Controls.Add(ExitButton);

            panel = new Panel
            {
                Location = new Point((owner.Width-320) / 2, 100),
                Size = new Size(320, owner.Height - 160),
                AutoScroll = true,
                BackColor = Color.Transparent,
            };

            var count = 0;
            foreach (ControlActions k in Enum.GetValues(typeof(ControlActions)))
            {
                if (k == ControlActions.None) continue;

                var b = new Button(150, 30) {Location = new Point(0, 50 * count)};
                b.SetText(k.ToString());
                panel.Controls.Add(b);

                button[k] = new Button(100, 30) {Location = new Point(200, 50 * count)};
                button[k].Click += (o, e) => ChangeControls(0, e, k);
                button[k].SetText(Platformer.Controls.KeyFromControl(k).ToString());
                panel.Controls.Add(button[k]);

                count++;
            }
            Controls.Add(panel);
        }

        private void ChangeControls(object sender, EventArgs e, ControlActions ca)
        {
            Choosing = ca;
        }

        private void Exit(object sender, EventArgs e)
        {
            Choosing = ControlActions.None;
            ReturnControl();
        }

        protected override void OnKeyDown(object o, KeyEventArgs e)
        {
            if (Choosing == ControlActions.None) return;
            
            Platformer.Controls.SetControl(Choosing, e.KeyCode);
            foreach (var b in button)
                button[b.Key].SetText(Platformer.Controls.KeyFromControl(b.Key).ToString());

            Choosing = ControlActions.None;
        }

        protected override void OnSizeChanged(object o, EventArgs e)
        {
            ExitButton.Location = new Point((owner.Width - 200) / 2, 20);
            panel.Location = new Point((owner.Width - 320) / 2, 100);
            panel.Size = new Size(320, owner.Height - 160);
        }
    }
}