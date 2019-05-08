﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Platformer;
using Platformer.Game;

namespace GUI
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
            CoordSheet.ChangeScale(CoordSheet.Scale * delta, _game.GetStateSnapshot().player.body);
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
        private readonly IGame _game;

        private readonly System.Timers.Timer _timer;

        /// <inheritdoc />
        /// <summary>
        /// Создаёт экземпляр окна, по переданной игре
        /// </summary>
        /// <param name="game"></param>
        public Window(IGame game)
        {
            _game = game;

            Width = 1000;
            Height = 500;

            CoordSheet = new CoordinateSheet(Width, Height);

            pict = new Bitmap(Width, Height);
            drawer = Graphics.FromImage(pict);
            drawer.InterpolationMode = InterpolationMode.NearestNeighbor;
            drawer.PixelOffsetMode = PixelOffsetMode.Half;

            screen = new PictureBox
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

            _gameoverTool.LocationChanged += (o, e) => RealGameOver();
            Controls.Add(_gameoverTool);

            _timer = new System.Timers.Timer(20);
            _timer.Elapsed += (o, e) =>
            {
                foreach (var texture in _textures)
                    texture.Value.Tick(20 * 0.001);
            };
            _timer.Elapsed += Draw;
            _timer.Start();

            MouseWheel += (o,e) => ChangeScale(e.Delta < 0 ? 0.9 : 10.0 / 9);
        }
        
        private void Draw(object sender, ElapsedEventArgs e)
        {
            var snapshot = _game.GetStateSnapshot();
            if (snapshot.gameIsOver)
                GameOver();
            else
            {
                if (gameState != State.Running)
                    return;
                AdjustBy(snapshot.player.body);
                Clear(snapshot.currentBackgroundColor);
                Draw(snapshot.entities);
                Flush();
            }
        }

        private readonly PictureBox _gameoverTool = new PictureBox();

        public void GameOver() => _gameoverTool.Left++;

        private void RealGameOver()
        {
            _game.Stop();
            _timer.Stop();
            Thread.Sleep(10);
            gameState = State.Pause;
            gameOver.ReceiveControl();
        }

        protected override void OnLoad(EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
        
        public void Pause()
        {
            _game.Stop();
            Thread.Sleep(10);
            gameState = State.Pause;
            pause.ReceiveControl();
        }
        
        public void Continue()
        {
            gameState = State.Running;
            _game.Start();
        }

        /// <summary>
        /// Обрабатывает нажатие клавиши на клавиатуре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (gameState != State.Running) return;
            
            if (ControlActions.StopTime == global::GUI.Controls.ControlFromKey(e.KeyCode))
            {
                Pause();
            }
            else
                _game.Action($"key_down {GUI.Controls.ControlFromKey(e.KeyCode)}");//.Add(GUI.Controls.ControlFromKey(e.KeyCode));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (gameState == State.Running)
                _game.Action($"key_up {GUI.Controls.ControlFromKey(e.KeyCode)}");
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            pict = new Bitmap(Width, Height);
            screen.Size = new Size(Width, Height);
            drawer = Graphics.FromImage(pict);
            drawer.InterpolationMode = InterpolationMode.NearestNeighbor;
            drawer.PixelOffsetMode = PixelOffsetMode.Half;
            CoordSheet.SetSize(Width, Height);
            var player = _game.GetStateSnapshot().player.body;
            CoordSheet.ChangeScale(Width / player.Width / 50, player);
        }

        private Vector _lastMouseCoords = Vector.Zero();

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (gameState == State.Pause)
                return;

            if (e.Button == MouseButtons.Left && _lastMouseCoords.IsZero())
            {
                _lastMouseCoords.x = e.X;
                _lastMouseCoords.y = e.Y;
            }
            else if (e.Button == MouseButtons.Left)
            {
                CoordSheet.Move(_lastMouseCoords + new Vector{x = e.X, y = e.Y} * (-1));
                _lastMouseCoords.x = e.X;
                _lastMouseCoords.y = e.Y;
            }
            else
            {
                _lastMouseCoords.x = 0;
                _lastMouseCoords.y = 0;
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
        /// <param name="rectangle"></param>
        public void Draw(Bitmap image, IRectangle rectangle)
        {
            if (gameState == State.Running)
                drawer.DrawImage(image, CoordSheet.Transform(rectangle));
        }

        private readonly Dictionary<string, global::GUI.ITexture> _textures = new Dictionary<string, global::GUI.ITexture>();

        public void Draw(GameObject entity)
        {
            Draw(_textures[entity.texture].Image, entity.body);
        }

        private void LoadTexture(string fname)
        {
            _textures[fname] = TextureFile.GetTexture(fname);
        }

        public void Draw(IEnumerable<GameObject> entities)
        {
            foreach (var entity in entities.OrderBy(e => e.drawPriority))
            {
                if(!_textures.ContainsKey(entity.texture))
                    LoadTexture(entity.texture);
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

        public void AdjustBy(IRectangle rectangle)
            => CoordSheet.AdjustBy(rectangle);
    }
}