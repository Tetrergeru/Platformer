﻿using Platformer.Entities;
using Platformer.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using Platformer.Physics;

namespace Platformer.Game
{
    public enum ControlActions { None, Jump, Right, Left, Stop, StopTime, RunTime, Debug, Fly, ScalePlus, ScaleMinus };

    /// <summary>
    /// основной класс, управляющий игровой механикой
    /// </summary>
    internal class Game : IGame
    {
        private HashSet<ControlActions> KeysPressed { get; } = new HashSet<ControlActions>();

        /// <summary>
        /// Игровой мир
        /// (Впоследствие будет перепилено в сторону динамического подгружения миров из файликов)
        /// </summary>
        private World World { get; set; }

        /// <summary>
        /// Объект персонажа игрока
        /// </summary>
        private Player Player { get; }

        /// <summary>
        /// Конструктор, инициализирующий игру параметрами по умолчанию
        /// </summary>
        public Game(ITimer timer, IPhysics physics)
        {
            _gameLoop = timer;
            _gameLoop.AddEvent(Tick);

            World = new World(physics);
            WorldFile.FillWorld(World, "Resources/Worlds/test.world");
            
            Player = World.CreatePlayer(new HitBox(0, -10, 0.25, 0.5));
            
            Player.Texture = "Resources/TextureAssets/player.texture";
            Player.DrawPriority = 10;
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
        /// <param name="deltaTime"></param>
        private void Tick(double deltaTime)
        {
            try
            {
                foreach (var x in KeysPressed)
                    OnControlTrigger(x);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            UpdateState();

            World.Tick(deltaTime);
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

        private void UpdateState()
        {
            _currentState = new StateSnapshot
                {
                    player = new GameObject
                    {
                        body = Player.Hitbox,
                        texture = Player.Texture,
                        drawPriority = Player.DrawPriority
                    },
                    entities = World
                        .AllEntities
                        .Select(e => new GameObject{body = e.Hitbox, texture = e.Texture,drawPriority = e.DrawPriority})
                        .ToList(),
                    gameIsOver = Player.Health <= 0,
                    //currentBackgroundColor = World.BackGroundColor,
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

        private void OnControlTrigger(ControlActions action)
        {
            switch (action)
            {
                case ControlActions.Right:
                {
                    Player.RunRight();
                    break;
                }
                case ControlActions.Left:
                {
                    Player.RunLeft();
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
                    Player.MoveTo(new Vector {x = 0, y = 0});
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