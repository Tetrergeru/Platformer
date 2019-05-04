using System;
using Platformer.Files;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using Platformer.Entities;
using Platformer.GUI;
using Timer = System.Timers.Timer;

namespace Platformer.Game
{
    /// <summary>
    /// основной класс, управляющий игровой механикой
    /// </summary>
    internal class Game : IGame
    {
        public HashSet<ControlActions> KeysPressed = new HashSet<ControlActions>();
        
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
        public Game(ITimer timer)
        {
            gameLoop = timer;
            gameLoop.AddEvent(Tick);

            Player = new Player(new Vector { x = 30, y = 60}, this);
            Player.Texture.AddTexture(new Bitmap("Resources/Textures/Player_1.png"), FillType.Stretch);
            Player.DrawPriority = 10;

            world = WorldFile.GetWorld("Resources/Worlds/simple_world.world");
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
        private readonly ITimer gameLoop;

        /// <summary>
        /// Игровой тик
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="deltaTime"></param>
        private void Tick(double deltaTime)
        {
            foreach (var x in KeysPressed)
                OnControlTrigger(x);
            try
            {
                world.Tick(deltaTime);
                window.AdjustBy(Player.Hitbox);
                window.Clear(world.BackGroundColor);
                window.Draw(world.AllEntities);
                window.Flush();
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }

        /// <summary>
        /// Начинает основной цикл игры
        /// </summary>
        public void Start()
        {
            gameLoop.Start();
        }

        public StateSnapshot GetStateSnapshot()
        {
            throw new NotImplementedException();
        }

        public void Action(string actionData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Останавливает основной цикл игры
        /// </summary>
        public void Stop()
        {
            gameLoop.Stop();
        }
        
        public void GameOver()
        {
            window.GameOver();
        }

        //==========================<o@o>==========================
        
        public void OnControlTrigger(ControlActions action)
        {
            switch (action)
            {
                case ControlActions.Right:
                {
                    Player.Run(Actor.Direction.Right);
                    break;
                }
                case ControlActions.Left:
                {
                    Player.Run(Actor.Direction.Left);
                    break;
                }
                case ControlActions.Jump:
                {
                    Player.Jump();
                    break;
                }
                case ControlActions.Stop:
                {
                    Player.TryToStop();
                    break;
                }
                case ControlActions.StopTime:
                {
                    Stop();
                    window.Pause();
                    break;
                }
                case ControlActions.RunTime:
                {
                    Start();
                    break;
                }
                case ControlActions.Debug:
                {
                    Player.Hitbox.MoveTo(new Vector {x = 0, y = 0});
                    break;
                }
                case ControlActions.Fly:
                {
                    Stop();
                    GameOver();
                    //Player.Flight = !Player.Flight;
                    break;
                }
                case ControlActions.ScaleMinus:
                {
                    window.ChangeScale(0.9);
                    break;
                }
                case ControlActions.ScalePlus:
                {
                    window.ChangeScale(10 / 9.0);
                    break;
                }
            }
        }
    }
}
