using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GUI.Menus
{
    /// <summary>
    ///     Базовый класс для создания игровых меню и графических интерфейсов
    /// </summary>
    internal abstract class Menu
    {
        /// <summary>
        ///     Игровое окно, в котором находится это меню
        /// </summary>
        protected Window owner;

        /// <summary>
        ///     Конструктор, создающий экземпляр класса Menu
        /// </summary>
        /// <param name="owner">Окно-владелец этого меню</param>
        protected Menu(Window owner)
        {
            this.owner = owner;
            Controls = new List<Control>();
            owner.SizeChanged += OnSizeChanged;
        }

        /// <summary>
        ///     Элементы управления, которые есть в нашем меню
        /// </summary>
        protected List<Control> Controls { get; }

        /// <summary>
        ///     Выводит меню на окно и подключает события
        /// </summary>
        public void Connect()
        {
            owner.KeyDown += OnKeyDown;
            owner.KeyUp += OnKeyUp;
            owner.MouseMove += OnMouseMove;
            owner.MouseClick += OnMouseClick;
            foreach (var ctrl in Controls)
            {
                Console.WriteLine(owner.screen.Controls.Count);
                owner.screen.Controls.Add(ctrl);
                ctrl.BringToFront();
            }
        }

        /// <summary>
        ///     Меню получает контроль (отключая меню лежащее на верхушке стэка) и само ложится на вершину стэка
        /// </summary>
        public void ReceiveControl()
        {
            if (owner.MenuStack.Count != 0)
                owner.MenuStack.Peek().Disconnect();
            owner.MenuStack.Push(this);

            Connect();
        }

        /// <summary>
        ///     Отключает события и убирает меню с экрана
        /// </summary>
        public void Disconnect()
        {
            owner.KeyDown -= OnKeyDown;
            owner.KeyUp -= OnKeyUp;
            owner.MouseMove -= OnMouseMove;
            owner.MouseClick -= OnMouseClick;
            foreach (var ctrl in Controls)
                owner.screen.Controls.Remove(ctrl);
        }

        /// <summary>
        ///     Меню отдаёт контроль и снимается с верхушки стэка
        /// </summary>
        public void ReturnControl()
        {
            Disconnect();

            owner.MenuStack.Pop();
            if (owner.MenuStack.Count != 0)
                owner.MenuStack.Peek().Connect();
            else
                owner.Continue();
        }

        /// <summary>
        ///     Срабатывает при нажатии на клавишу
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        protected virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        ///     Срабатывает при отпускании клавиши
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        protected virtual void OnKeyUp(object sender, KeyEventArgs e)
        {
        }

        /// <summary>
        ///     Срабатывет при перемещении мыши
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        ///     Срабатывет при нажатии на кнопку мыши
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Параметры события</param>
        protected virtual void OnMouseClick(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        ///     Срабатывает при изменении размера окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSizeChanged(object sender, EventArgs e)
        {
        }
    }
}