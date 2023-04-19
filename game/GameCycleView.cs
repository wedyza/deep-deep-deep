using game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace deep_deep_deep
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };

        private Dictionary<int, IObject> _objects = new Dictionary<int, IObject>();
        private Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _textures.Add(1, Content.Load<Texture2D>("ayanami"));
            _textures.Add(2, Content.Load<Texture2D>("wall"));
        }

        public void LoadGameCycleParameters(Dictionary<int, IObject> objects)
        {
            _objects= objects;
        }

        protected override void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            if (keys.Length > 0)
            {
                var k = keys[0];
                switch (k)
                {
                    case Keys.W:
                        {
                            PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.forward }
                                );break;
                        }
                    case Keys.S:
                        {
                            PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.backward }
                                );break;
                        }
                    case Keys.A:
                        {
                            PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.left }
                                );break;
                        }
                    case Keys.D:
                        {
                            PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction= IGameplayModel.Direction.right }
                                );break;    
                        }
                    case Keys.Escape:
                        {
                            Exit();
                            break;
                        }
                }
            }

            base.Update(gameTime);
            CycleFinished.Invoke(this, new EventArgs());
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            _spriteBatch.Begin();
            foreach(var o in _objects.Values)
            {
                _spriteBatch.Draw(_textures[o.ImageID], o.Pos, Color.White);
            }
            _spriteBatch.End();
        }
    }
}