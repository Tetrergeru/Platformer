using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace Platformer
{
    /// <summary>
    /// основной класс, управляющий игровой механикой
    /// </summary>
    class Game
    {
        /// <summary>
        /// Графическое окно, в котором отрисовывается игра
        /// </summary>
        public Window window;

        /// <summary>
        /// Игровой мир
        /// (Впоследствие будет перепилено в сторону динамического подгружения миров из файликов)
        /// </summary>
        public World world;

        /// <summary>
        /// Объект персонажа игрока
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор, инициализирующий игру параметрами по умолчанию
        /// </summary>
        public Game()
        {
            gameLoop = new Timer(TickTime);
            gameLoop.Elapsed += Tick;

            Player = new Player(new Vector { x = 20, y = 30});

            world = new World();
            world.SetPlayer(Player, new Vector { x = 40, y = 40 });
        }

        //============    Работа с игровым временем    =============

        /// <summary>
        /// Интервал таймера, вызывающего тики игры
        /// </summary>
        private const int TickTime = 10;

        /// <summary>
        /// Основной цикл игры
        /// </summary>
        private Timer gameLoop;

        /// <summary>
        /// Игровой тик
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tick(object sender, ElapsedEventArgs e)
        {
            world.player.Move(TickTime / 1000.0);
            window.Clear();

            window.Draw(Color.Red, world.player.Hitbox.ToDrawing());
            foreach (var x in world.block)
                window.Draw(Color.Blue, x.Hitbox.ToDrawing());

            window.Flush();
        }

        /// <summary>
        /// Начинает основной цикл игры
        /// </summary>
        public void Start()
        {
            gameLoop.Start();
        }

        /// <summary>
        /// Останавливает основной цикл игры
        /// </summary>
        public void Stop()
        {
            gameLoop.Stop();
        }
    }
}
