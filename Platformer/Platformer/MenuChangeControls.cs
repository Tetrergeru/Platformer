using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Platformer
{
    internal class MenuChangeControls : Menu
    {
        private Dictionary<ControlActions, Button> ChoiseButtons { get; } = new Dictionary<ControlActions, Button>();

        private Panel ControlsPanel { get; }

        private Button ExitButton { get; }

        private ControlActions Choise { get; set; } = ControlActions.None;

        public MenuChangeControls(Window owner) : base(owner)
        {
            ExitButton = new Button(200, 50)
            {
                Location = new Point((owner.Width- 200)/2, 20),
            };
            ExitButton.Click += Exit;
            ExitButton.SetText("Exit");
            Controls.Add(ExitButton);

            ControlsPanel = new Panel
            {
                Location = new Point((owner.Width-320) / 2, 100),
                Size = new Size(320, owner.Height - 160),
                AutoScroll = true,
                BackColor = Color.Transparent,
            };
            ControlsPanel.Scroll += (sender, e) => { ControlsPanel.Invalidate(); };

        var count = 0;
            foreach (ControlActions k in Enum.GetValues(typeof(ControlActions)))
            {
                if (k == ControlActions.None) continue;

                var b = new Button(150, 30) {Location = new Point(0, 50 * count)};
                b.SetText(k.ToString());
                ControlsPanel.Controls.Add(b);

                ChoiseButtons[k] = new Button(100, 30) {Location = new Point(200, 50 * count)};
                ChoiseButtons[k].Click += (o, e) => ChangeControls(0, e, k);
                ChoiseButtons[k].SetText(Platformer.Controls.KeyFromControl(k).ToString());
                ControlsPanel.Controls.Add(ChoiseButtons[k]);

                count++;
            }
            Controls.Add(ControlsPanel);
        }

        private void ChangeControls(object sender, EventArgs e, ControlActions ca)
        {
            ChoiseButtons[ca].SetText("<choose>");
            Choise = ca;
        }

        private void Exit(object sender, EventArgs e)
        {
            Choise = ControlActions.None;
            ReturnControl();
        }

        protected override void OnKeyDown(object o, KeyEventArgs e)
        {
            if (Choise == ControlActions.None) return;
            
            Platformer.Controls.SetControl(Choise, e.KeyCode);
            foreach (var b in ChoiseButtons)
                ChoiseButtons[b.Key].SetText(Platformer.Controls.KeyFromControl(b.Key).ToString());

            Choise = ControlActions.None;
        }

        protected override void OnSizeChanged(object o, EventArgs e)
        {
            ExitButton.Location = new Point((owner.Width - 200) / 2, 20);
            ControlsPanel.Location = new Point((owner.Width - 320) / 2, 100);
            ControlsPanel.Size = new Size(320, owner.Height - 160);
            ControlsPanel.AutoScroll = true;
        }
    }
}