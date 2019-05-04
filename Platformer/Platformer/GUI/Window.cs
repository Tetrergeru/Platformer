﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Platformer.Entities;

namespace Platformer.GUI
{
    public enum State
    {
        Running,
        Pause,
    }

    /// <summary>
    /// Форма, на кторой происходит отрисовка игрового мира
    /// </summary>
    class Window : Form
    {
        public PictureBox screen;

        private State gameState = State.Running;

        public Stack<Menu> MenuStack { get; } = new Stack<Menu>();

        public MenuPause pause { get; }

        public MenuChangeControls ChangeControls { get; }

        public MenuGameOver gameOver { get; }

        private CoordinateSheet CoordSheet { get; }

        public void ChangeScale(double delta)
        {
            CoordSheet.ChangeScale(CoordSheet.Scale * delta, game.Player.hitbox);
        }

        /// <summary>
        /// Картинка, куда мы будем рисовать все необходимые изображения, перед тем, как отобразить всё на экране
        /// </summary>
        private Bitmap pict;

        /// <summary>
        /// Поверхность для рисование на картинке
        /// </summary>
        private Graphics drawer;

        /// <summary>
        /// Игра, привязанная к окну
        /// </summary>
        private Game game;

        /// <summary>
        /// Создаёт экземпляр окна, по переданной игре
        /// </summary>
        /// <param name="game"></param>
        public Window(Game game)
        {
            this.game = game;

            Width = 1000;
            Height = 500;
            
            CoordSheet = new CoordinateSheet(Width, Height);

            pict = new Bitmap(Width, Height);
            drawer = Graphics.FromImage(pict);
            drawer.InterpolationMode = InterpolationMode.NearestNeighbor;
            drawer.PixelOffsetMode = PixelOffsetMode.Half;

            screen = new PictureBox()
            {
                Width = Width,
                Height = Height,
                Left = 0,
                Top = 0,
            };
            Controls.Add(screen);

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            SizeChanged += OnSizeChanged;
            screen.MouseMove += OnMouseMove;

            pause = new MenuPause(this);
            ChangeControls = new MenuChangeControls(this);
            gameOver = new MenuGameOver(this);
        }

        public void Pause()
        {
            game.Stop();
            Thread.Sleep(10);
            gameState = State.Pause;
            pause.ReceiveControl();
        }

        public void GameOver()
        {
            game.Stop();
            Thread.Sleep(10);
            gameState = State.Pause;
            gameOver.ReceiveControl();
        }

        public void Continue()
        {
            gameState = State.Running;
            game.Start();
        }

        /// <summary>
        /// Обрабатывает нажатие клавиши на клавиатуре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (gameState != State.Running) return;

            if (ControlActions.StopTime == GUI.Controls.ControlFromKey(e.KeyCode))
            {
                Pause();
            }
            else
                game.KeysPressed.Add(GUI.Controls.ControlFromKey(e.KeyCode));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (gameState == State.Running)
                game.KeysPressed.Remove(GUI.Controls.ControlFromKey(e.KeyCode));
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            pict = new Bitmap(Width, Height);
            screen.Size = new Size(Width, Height);
            drawer = Graphics.FromImage(pict);
            drawer.InterpolationMode = InterpolationMode.NearestNeighbor;
            drawer.PixelOffsetMode = PixelOffsetMode.Half;
            CoordSheet.SetSize(Width, Height);
            CoordSheet.ChangeScale(Width / game.Player.Hitbox().Width / 50, game.Player.hitbox);
        }

        private Vector lastMouseCoords = Vector.Zero();

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (gameState == State.Pause)
                return;

            if (e.Button == MouseButtons.Left && lastMouseCoords.IsZero())
            {
                lastMouseCoords.x = e.X;
                lastMouseCoords.y = e.Y;
            }
            else if (e.Button == MouseButtons.Left)
            {
                CoordSheet.Move(lastMouseCoords + new Vector{x = e.X, y = e.Y} * (-1));
                lastMouseCoords.x = e.X;
                lastMouseCoords.y = e.Y;
            }
            else
            {
                lastMouseCoords.x = 0;
                lastMouseCoords.y = 0;
            }
        }

        /// <summary>
        /// Очищает картинку
        /// </summary>
        public void Clear(Color color)
        {
            drawer.Clear(color);
        }

        /// <summary>
        /// Рисует на картинке данное изображение в данном прямоугольнике
        /// </summary>
        /// <param name="image"></param>
        /// <param name="hitbox"></param>
        public void Draw(Bitmap image, System.Windows.Rect hitbox)
        {
            if (gameState == State.Running)
                drawer.DrawImage(image, CoordSheet.Transform(hitbox));
        }

        /// <summary>
        /// рисует на кртинке прямоугольник данного цвета
        /// </summary>
        /// <param name="color"></param>
        /// <param name="hitbox"></param>
        public void Draw(Color color, Rectangle rect)
        {
            drawer.DrawRectangle(new Pen(color, 2), rect);
        }

        public void Draw(Entity entity)
        {
            Draw(entity.Texture.Image, entity.Hitbox());
        }

        public void Draw(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities.OrderBy(e => e.DrawPriority))
            {
                Draw(entity);
            }
        }

        /// <summary>
        /// Переносит содержимое картинки на экран
        /// </summary>
        public void Flush()
        {
            //var screen = CreateGraphics();
            screen.Image = pict;//(pict, 0, 0);
        }

        public void AdjustBy(System.Windows.Rect hitbox)
            => CoordSheet.AdjustBy(hitbox);
    }
}
