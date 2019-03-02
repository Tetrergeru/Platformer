using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Platformer
{
    internal class MenuPause : Menu
    {
        private PictureBox PauseButton { get; }

        private PictureBox ChangeControlsButton { get; }


        public MenuPause(Window owner) : base(owner)
        {
            var panel = new Panel()
            {
                Size = new Size(200,300),
                Left = (owner.Width - 200) / 2,
                Top = (owner.Height - 150) / 2,
                BackColor = Color.Transparent,
            };

            PauseButton = new PictureBox()
            {
                Size = new Size(200,50),
                Image = new Bitmap("Resources/Textures/Continue_Button_0.png"),
                BackColor = Color.Transparent,
            };
            PauseButton.Click += Continue;
            panel.Controls.Add(PauseButton);

            ChangeControlsButton = new PictureBox()
            {
                Location = new Point(0, 100),
                Size = new Size(200, 50),
                Image = new Bitmap("Resources/Textures/Continue_Button_0.png"),
                BackColor = Color.Transparent,
            };
            ChangeControlsButton.Click += ChangeControls;
            panel.Controls.Add(ChangeControlsButton);

            Controls.Add(panel);
        }

        private void Continue(object o, EventArgs e)
        {
            ReturnControl();
            owner.Continue();
        }

        protected override void OnSizeChanged(object o, EventArgs e)
        {
            PauseButton.Left = (owner.Width - 200) / 2;
            PauseButton.Top = (owner.Height - 50) / 2;
        }

        private void ChangeControls(object sender, EventArgs e)
        {
            owner.ChangeControls.ReceiveControl();
        }

    }
}