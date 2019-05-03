using System;

namespace Platformer.GUI
{
    internal class MenuGameOver : Menu
    {
        public MenuGameOver(Window owner) : base(owner)
        {
            Controls.Add(new Button(500, 100){Text = "Game Over", Left = 100, Top = 100});
        }
    }
}