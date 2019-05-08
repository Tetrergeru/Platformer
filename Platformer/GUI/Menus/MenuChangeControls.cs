using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Platformer.Game;

namespace GUI.Menus
{
    /// <inheritdoc />
    /// <summary>
    ///     Меню смены управления
    /// </summary>
    internal class MenuChangeControls : Menu
    {
        /// <inheritdoc />
        /// <summary>
        ///     Конструктор, создающий экземпляр класса MenuChangeControls
        /// </summary>
        /// <param name="owner">Окно-владелец этого меню</param>
        public MenuChangeControls(Window owner) : base(owner)
        {
            //Создаём кнопку выхода
            ExitButton = new Button(200, 50)
                {Location = new Point((owner.Width - 200) / 2, 20)};
            ExitButton.Click += Exit;
            ExitButton.SetText("Exit");
            Controls.Add(ExitButton);

            //Создаём панель, в которую добавим кнопки смены управления
            ControlsPanel = new Panel
            {
                Location = new Point((owner.Width - 320) / 2, 100),
                Size = new Size(320, owner.Height - 160),
                AutoScroll = true,
                BackColor = Color.Transparent
            };
            ControlsPanel.Scroll += (sender, e) => { ControlsPanel.Invalidate(); };

            //А вот и сами кнопки
            var count = 0;
            foreach (ControlActions k in Enum.GetValues(typeof(ControlActions)))
            {
                if (k == ControlActions.None) continue;

                var b = new Button(150, 30) {Location = new Point(0, 50 * count)};
                b.SetText(k.ToString());
                ControlsPanel.Controls.Add(b);

                ChoiseButtons[k] = new Button(100, 30) {Location = new Point(200, 50 * count)};
                ChoiseButtons[k].Click += (o, e) => ChangeControls(k);
                ChoiseButtons[k].SetText(GUI.Controls.KeyFromControl(k).ToString());
                ControlsPanel.Controls.Add(ChoiseButtons[k]);

                count++;
            }

            Controls.Add(ControlsPanel);
        }

        /// <summary>
        ///     Панель, содержащая кнопки смены управления
        /// </summary>
        private Panel ControlsPanel { get; }

        /// <summary>
        ///     Кнопки смены управления. Создаётся по кнопке для каждого игрового действия (Кроме ControlActions.None).
        ///     При нажатии на кнопку начинается замена клавиши управления соответствующего действия.
        /// </summary>
        private Dictionary<ControlActions, Button> ChoiseButtons { get; } = new Dictionary<ControlActions, Button>();

        /// <summary>
        ///     Кнопка выхода из меню
        /// </summary>
        private Button ExitButton { get; }

        /// <summary>
        ///     Игровое действие, клавишу управления для которого мы меняем в данный момент (если такого нет — ControlActions.None)
        /// </summary>
        private ControlActions Choise { get; set; } = ControlActions.None;

        /// <summary>
        ///     Начинает процесс смены кнопки управления заданным действием
        /// </summary>
        /// <param name="ca">Действие, управление которым мы собираемся менять</param>
        private void ChangeControls(ControlActions ca)
        {
            Choise = ca;
            foreach (var b in ChoiseButtons)
                ChoiseButtons[b.Key].SetText(GUI.Controls.KeyFromControl(b.Key).ToString());
            ChoiseButtons[ca].SetText("<choose>");
        }

        /// <inheritdoc />
        /// <summary>
        ///     При нажатии клавиши, если происходит смена клавиши управления — меняем её на на нажатую.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        protected override void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Choise == ControlActions.None) return;

            GUI.Controls.SetControl(Choise, e.KeyCode);
            foreach (var b in ChoiseButtons)
                ChoiseButtons[b.Key].SetText(GUI.Controls.KeyFromControl(b.Key).ToString());

            Choise = ControlActions.None;
        }

        /// <inheritdoc />
        /// <summary>
        ///     При изменении размера окна выравнивает элементы управления по центру
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        protected override void OnSizeChanged(object sender, EventArgs e)
        {
            ExitButton.Location = new Point((owner.Width - 200) / 2, 20);
            ControlsPanel.Location = new Point((owner.Width - 320) / 2, 100);
            ControlsPanel.Size = new Size(320, owner.Height - 160);
        }

        /// <summary>
        ///     Выход из меню
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        private void Exit(object sender, EventArgs e)
        {
            Choise = ControlActions.None;
            ReturnControl();
        }
    }
}