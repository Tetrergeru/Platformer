using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.Menus
{
    /// <inheritdoc />
    /// <summary>
    ///     Меню игровой паузы
    /// </summary>
    internal class MenuPause : Menu
    {
        /// <inheritdoc />
        /// <summary>
        ///     Конструктор, создающий экземпляр класса MenuPause
        /// </summary>
        /// <param name="owner">Окно-владелец этого меню</param>
        public MenuPause(Window owner) : base(owner)
        {
            ButtonPanel = new Panel
            {
                Size = new Size(200, 300),
                Left = (owner.Width - 200) / 2,
                Top = (owner.Height - 150) / 2,
                BackColor = Color.Transparent
            };

            ContinueButton = new PictureBox
            {
                Size = new Size(200, 50),
                Image = new Bitmap("Resources/Textures/Continue_Button_0.png"),
                BackColor = Color.Transparent
            };
            ContinueButton.Click += Exit;
            ButtonPanel.Controls.Add(ContinueButton);

            ChangeControlsButton = new PictureBox
            {
                Location = new Point(0, 100),
                Size = new Size(200, 50),
                Image = new Bitmap("Resources/Textures/Continue_Button_0.png"),
                BackColor = Color.Transparent
            };
            ChangeControlsButton.Click += ChangeControls;
            ButtonPanel.Controls.Add(ChangeControlsButton);

            Controls.Add(ButtonPanel);
        }

        /// <summary>
        ///     Панель, содержащая все кнопки меню паузы
        /// </summary>
        private Panel ButtonPanel { get; }

        /// <summary>
        ///     Кнопка выхода из меню
        /// </summary>
        private PictureBox ContinueButton { get; }

        /// <summary>
        ///     Кнопка смены управления, открывает меню смены управления
        /// </summary>
        private PictureBox ChangeControlsButton { get; }

        /// <inheritdoc />
        /// <summary>
        ///     При изменении размера окна выравнивает элементы управления по центру
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        protected override void OnSizeChanged(object sender, EventArgs e)
        {
            ButtonPanel.Left = (owner.Width - 200) / 2;
            ButtonPanel.Top = (owner.Height - 150) / 2;
        }

        /// <summary>
        ///     Открывает меню смены управления
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        private void ChangeControls(object sender, EventArgs e)
        {
            owner.ChangeControls.ReceiveControl();
        }

        /// <summary>
        ///     Выход из меню
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        private void Exit(object sender, EventArgs e)
        {
            ReturnControl();
        }
    }
}