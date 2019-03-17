﻿using Platformer.Files;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Platformer
{
    /// <summary>
    /// основной класс, управляющий игровой механикой
    /// </summary>
    class Game
    {
        public HashSet<Controls.Control> KeysPressed = new HashSet<Controls.Control>();

        private bool DrawDebug = false;
        
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
            Player.Texture.AddTexture(new Bitmap("Resources/Textures/Player_1.png"), FillType.Stretch);
            Player.DrawPriority = 10;

            world = WorldFile.GetWorld("Resources/Worlds/test_1.world");
            world.SetPlayer(Player, new Vector { x = 0, y = 0 });
            
            //grassImg = ;
        }

        //============    Работа с игровым временем    ============

        /// <summary>
        /// Интервал таймера, вызывающего тики игры
        /// </summary>
        private const int TickTime = 20;

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
            foreach (var x in KeysPressed)
                OnControlTrigger(x);

            world.Tick(TickTime / 1000.0);

            window.AdjustBy(Player.Hitbox);

            window.Clear(world.BackGroundColor);

            window.Draw(world.AllEntities);

            if (DrawDebug)
            {
                foreach (var x in world.Decorations)
                    window.Draw(Color.DarkGreen, x.Hitbox);
                window.Draw(Color.Red, world.Player.Hitbox);
                foreach (var x in world.Blocks)
                    window.Draw(Color.Blue, x.Hitbox);
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
                    window.Pause();
                    break;
                }
                case Controls.Control.RunTime:
                {
                    Start();
                    break;
                }
                case Controls.Control.Debug:
                {
                    Player.Hitbox.MoveTo(new Vector {x = 0, y = 0});
                    DrawDebug = !DrawDebug;
                    break;
                }
                case Controls.Control.Fly:
                {
                    Player.Flight = !Player.Flight;
                    break;
                }
                case Controls.Control.ScaleMinus:
                {
                    window.ChangeScale(0.9);
                    break;
                }
                case Controls.Control.ScalePlus:
                {
                    window.ChangeScale(10/9.0);
                    break;
                }
            }
        }
    }
}
