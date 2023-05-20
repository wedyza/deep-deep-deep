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
        
        private double cooldownTime;
        
        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerAttacked = delegate {  };

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
            _textures.Add((byte)GameCycle.ObjectTypes.door, Content.Load<Texture2D>("doorx4"));
            _textures.Add((byte)GameCycle.ObjectTypes.fire, Content.Load<Texture2D>("firex2"));
        }

        public void LoadGameCycleParameters(Dictionary<int, IObject> objects)
        {
            _objects= objects;
        }

        protected override void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            cooldownTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (keys.Length > 0)
            {
                foreach (var k in keys)
                {
                    switch (k)
                    {
                        case Keys.W:
                            PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.forward }
                            );
                            break;
                        case Keys.A:
                            PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.left }
                            );
                            break;
                        case Keys.D:
                            PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.right }
                            );
                            break;
                        case Keys.S:
                            PlayerMoved.Invoke(
                                this,
                                new ControlsEventArgs { Direction = IGameplayModel.Direction.backward }
                            );
                            break;
                        case Keys.Escape:
                            Exit();
                            break;
                    }

                    if (cooldownTime > 1500)
                    {
                        if (k == Keys.Left)
                        {
                            PlayerAttacked.Invoke(
                                this,
                                new ControlsEventArgs{Direction = IGameplayModel.Direction.left});
                            cooldownTime = 0;
                        }
                        else if (k == Keys.Right)
                        {
                            PlayerAttacked.Invoke(
                                this,
                                new ControlsEventArgs{Direction = IGameplayModel.Direction.right});
                            cooldownTime = 0;
                        }
                        else if (k == Keys.Up)
                        {
                            PlayerAttacked.Invoke(
                                this,
                                new ControlsEventArgs{Direction = IGameplayModel.Direction.forward});
                            cooldownTime = 0;
                        }
                        else if (k == Keys.Down)
                        {
                            PlayerAttacked.Invoke(
                                this,
                                new ControlsEventArgs{Direction = IGameplayModel.Direction.backward});
                            cooldownTime = 0;
                        }
                    }
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
                else if (o.dir == IGameplayModel.Direction.left)
                    _spriteBatch.Draw(_textures[o.ImageID], o.Pos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
                else if (o.dir == IGameplayModel.Direction.forward)
                    _spriteBatch.Draw(_textures[o.ImageID], o.Pos, null, Color.White, (float)-Math.PI/2f,new Vector2(64, 0), 1f, SpriteEffects.None, 0);
                else if (o.dir == IGameplayModel.Direction.backward)
                    _spriteBatch.Draw(_textures[o.ImageID], o.Pos, null, Color.White, (float)Math.PI/2f,new Vector2(0, 64), 1f, SpriteEffects.None, 0);
            }
            _spriteBatch.End();
        }
    }
}