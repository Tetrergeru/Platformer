using System;
using System.Drawing;
using System.Windows.Forms;

namespace Platformer
{
    internal class MenuPause : Menu
    {
        private Panel ButtonPanel { get; }

        private PictureBox PauseButton { get; }

        private PictureBox ChangeControlsButton { get; }

        public MenuPause(Window owner) : base(owner)
        {
            ButtonPanel = new Panel()
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
            ButtonPanel.Controls.Add(PauseButton);

            ChangeControlsButton = new PictureBox()
            {
                Location = new Point(0, 100),
                Size = new Size(200, 50),
                Image = new Bitmap("Resources/Textures/Continue_Button_0.png"),
                BackColor = Color.Transparent,
            };
            ChangeControlsButton.Click += ChangeControls;
            ButtonPanel.Controls.Add(ChangeControlsButton);

            Controls.Add(ButtonPanel);
        }

        private void Continue(object o, EventArgs e)
        {
            ReturnControl();
            owner.Continue();
        }

        protected override void OnSizeChanged(object o, EventArgs e)
        {
            ButtonPanel.Left = (owner.Width - 200) / 2;
            ButtonPanel.Top = (owner.Height - 150) / 2;
        }

        private void ChangeControls(object sender, EventArgs e)
        {
            owner.ChangeControls.ReceiveControl();
        }

    }
}