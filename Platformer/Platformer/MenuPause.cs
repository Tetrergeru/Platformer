using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Platformer
{
    internal class MenuPause : Menu
    {
        public MenuPause(Window owner) : base(owner)
        {
            var button = new PictureBox()
            {
                Left = 100,
                Top = 100,
                Width = 200,
                Height = 50,
                Image = new Bitmap("Resources/Textures/Continue_Button_0.png"),
                BackColor = Color.Transparent,
                ForeColor = Color.Transparent,
                
            };
            button.Click += Continue;
            Controls.Add(button);
        }

        private void Continue(object o, EventArgs e)
        {
            ReturnControl();
            owner.Continue();
        }
    }
}