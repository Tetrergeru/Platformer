using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Platformer
{
    internal class MenuPause : Menu
    {
        private PictureBox PauseButton { get; }

        public MenuPause(Window owner) : base(owner)
        {
            PauseButton = new PictureBox()
            {
                Left = (owner.Width - 200) / 2,
                Top = (owner.Height - 50) / 2,
                Width = 200,
                Height = 50,
                Image = new Bitmap("Resources/Textures/Continue_Button_0.png"),
                BackColor = Color.Transparent,
            };
            PauseButton.Click += Continue;
            Controls.Add(PauseButton);
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
    }
}