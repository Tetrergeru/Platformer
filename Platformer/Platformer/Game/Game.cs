﻿using System;
using Platformer.Files;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public HashSet<ControlActions> KeysPressed { get; } = new HashSet<ControlActions>();
        
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
            _gameLoop = timer;
            _gameLoop.AddEvent(Tick);

            Player = new Player(new Vector { x = 30, y = 60});
            Player.Texture.AddTexture(new Bitmap("Resources/Textures/Player_1.png"), FillType.Stretch);
            Player.DrawPriority = 10;

            world = WorldFile.GetWorld("Resources/Worlds/simple_world.world");
            world.SetPlayer(Player, new Vector { x = 0, y = 0 });
            UpdateState();
        }

        //============    Работа с игровым временем    ============
        
        /// <summary>
        /// Основной цикл игры
        /// </summary>
        private readonly ITimer _gameLoop;

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

            UpdateState();
            
            world.Tick(deltaTime);
        }

        /// <summary>
        /// Начинает основной цикл игры
        /// </summary>
        public void Start()
            => _gameLoop.Start();
        
        /// <summary>
        /// Останавливает основной цикл игры
        /// </summary>
        public void Stop()
        {
            _gameLoop.Stop();
        }
        
        //==========================<o@o>==========================

        private StateSnapshot _currentState;

        public void UpdateState()
        {
            _currentState = new StateSnapshot
                {
                    player = new GameObject
                    {
                        body = Player.Hitbox,
                        texture = Player.Texture,
                        drawPriority = Player.DrawPriority
                    },
                    entities = world
                        .AllEntities
                        .Select(e => new GameObject{body = e.Hitbox, texture = e.Texture,drawPriority = e.DrawPriority})
                        .ToList(),
                    gameIsOver = Player.Health <= 0,
                    currentBackgroundColor = world.BackGroundColor,
            };
            
        }

        public StateSnapshot GetStateSnapshot()
        {
            return _currentState;
        }

        public void Action(string action)
        {
            var actionData = action.Split(' ');
            switch (actionData[0])
            {
                case "key_down":
                {
                    Enum.TryParse<ControlActions>(actionData[1], true, out var result);
                    KeysPressed.Add(result);
                    break;
                }
                case "key_up":
                {
                    Enum.TryParse<ControlActions>(actionData[1], true, out var result);
                    KeysPressed.Remove(result);
                    break;
                }
            }
        }

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
                    //window.Pause();
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
                    //window.GameOver();
                    //Player.Flight = !Player.Flight;
                    break;
                }
                case ControlActions.ScaleMinus:
                {
                    //window.ChangeScale(0.9);
                    break;
                }
                case ControlActions.ScalePlus:
                {
                    //window.ChangeScale(10 / 9.0);
                    break;
                }
            }
        }
    }
}
