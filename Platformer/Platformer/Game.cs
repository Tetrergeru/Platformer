using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Platformer
{
    /// <summary>
    /// основной класс, управляющий игровой механикой
    /// </summary>
    class Game
    {
        private static bool DrawDebug = false;
        
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

            Player = new Player(new Vector { x = 30, y = 60});
            Player.Texture.AddTexture(new Bitmap("Resources/Textures/Player_1.png"), Texture.FillType.Stretch);
            
            world = new World();
            world.SetPlayer(Player, new Vector { x = 200, y = 40 });
            
            //grassImg = ;
        }

        //============    Работа с игровым временем    ============

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

            window.AdjustBy(Player.Hitbox);

            window.Clear();

            window.Draw(world.block);
            window.Draw(Player);
            
            if (DrawDebug)
            {
                foreach (var x in world.block)
                    window.Draw(Color.Blue, x.Hitbox);
                window.Draw(Color.Red, world.player.Hitbox);
            }

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

        //==========================<o@o>==========================


        public void OnControlTrigger(Controls.Control action)
        {
            switch (action)
            {
                case Controls.Control.Right:
                    {
                        Player.Run(Actor.Direction.Right);
                        break;
                    }
                case Controls.Control.Left:
                    {
                        Player.Run(Actor.Direction.Left);
                        break;
                    }
                case Controls.Control.Jump:
                    {
                        Player.Jump();
                        break;
                    }
                case Controls.Control.Stop:
                    {
                        Player.TryToStop();
                        break;
                    }
                case Controls.Control.StopTime:
                    {
                        Stop();
                        break;
                    }
                case Controls.Control.RunTime:
                    {
                        Start();
                        break;
                    }
                case Controls.Control.Debug:
                    {
                        Player.Hitbox.MoveTo(new Vector { x = 40, y = 40 });
                        break;
                    }
            }
        }
    }
}
