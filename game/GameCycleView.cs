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
            _textures.Add((byte)GameCycle.ObjectTypes.player, Content.Load<Texture2D>("wizardx4"));
            _textures.Add((byte)GameCycle.ObjectTypes.wall, Content.Load<Texture2D>("wallx4"));
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
                if (k == Keys.W)
                {
                    PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.forward }
                                );
                }
                if (k == Keys.S)
                {
                    PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.backward }
                                );
                }
                if (k == Keys.A)
                {
                    PlayerMoved.Invoke(
                               this,
                               new ControlsEventArgs { Direction = IGameplayModel.Direction.left }
                               );
                }
                if (k == Keys.D)
                {
                    PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.right }
                                );
                }
                if (k == Keys.Escape)
                {
                    Exit();
                }
                
            }

            base.Update(gameTime);
            CycleFinished.Invoke(this, new EventArgs());
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            base.Draw(gameTime);
            _spriteBatch.Begin();
            foreach(var o in _objects.Values)
            {
                if (o.dir == IGameplayModel.Direction.right)
                    _spriteBatch.Draw(_textures[o.ImageID], o.Pos, Color.White);
                else 
                    _spriteBatch.Draw(_textures[o.ImageID], o.Pos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
            }
            _spriteBatch.End();
        }
    }
}