using System;
using System.Drawing;

namespace GUI.Menus
{
    internal class MenuGameOver : Menu
    {
        private global::GUI.Button GameOverButton;

        public MenuGameOver(Window owner) : base(owner)
        {
            GameOverButton = new global::GUI.Button(200, 50)
            {
                Location = new Point((owner.Width - 200) / 2, 20)
            };
            GameOverButton.Click += (o, e) => Console.WriteLine("FUCK");
            GameOverButton.SetText("Exit");
            Controls.Add(GameOverButton);
            /*
            GameOverButton = new Platformer.GUI.Button(500, 100)
            {
                Location = new Point(200, 50),
            };
            GameOverButton.SetText("Game Over");
            Controls.Add(GameOverButton);
            */
        }
    }
}