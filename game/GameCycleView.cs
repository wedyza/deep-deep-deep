using game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace deep_deep_deep
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState _actualGameState;
        private double cooldownTime;
        
        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerAttacked = delegate {  };
        public event EventHandler GameReseted = delegate {  };
        public event EventHandler<SpellsEventArgs> ChangeSpell = delegate {  };
        public GameState ActualGameState 
        {
            get
            {
                return _actualGameState;
            }
            set
            {
                _actualGameState = value;
            }
        }

        private KeyboardState _currentKey;
        private KeyboardState _previousKey;
        
        private Dictionary<int, IObject> _objects = new Dictionary<int, IObject>();
        private Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();
        private SpriteFont defaultFont;

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _actualGameState = GameState.Menu;
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
            _textures.Add((byte)GameCycle.ObjectTypes.ice, Content.Load<Texture2D>("icex2"));
            
            _textures.Add((byte)GameCycle.ObjectTypes.goblin, Content.Load<Texture2D>("goblinx4z"));
            defaultFont = Content.Load<SpriteFont>("File");
        }

        public void LoadGameCycleParameters(Dictionary<int, IObject> objects)
        {
            _objects= objects;
        }
        
        
        public enum MenuStates
        {
            NewGame,
            ResumeGame,
            ExitGame
        }
        
        public enum GameState
        {
            Menu,
            Game
        }

        public int _menuCounter = 1;
        public MenuStates option = MenuStates.NewGame;
        public int MenuCounter
        {
            get
            {
                return _menuCounter;
            }
            set
            {
                if (value > 3)
                    _menuCounter = 3;
                else if (value < 1)
                    _menuCounter = 1;
                else
                    _menuCounter = value;

                if (_menuCounter == 1)
                    option = MenuStates.NewGame;
                else if (_menuCounter == 2)
                    option = MenuStates.ResumeGame;
                else if (_menuCounter == 3)
                    option = MenuStates.ExitGame;
            }
        }

        
        protected override void Update(GameTime gameTime)
        {
            _previousKey = _currentKey;
            var keys = Keyboard.GetState().GetPressedKeys();
            cooldownTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            _currentKey = Keyboard.GetState();
            if (keys.Length > 0)
            {
                switch (_actualGameState)
                {
                    case GameState.Game:
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
                                case Keys.D1:
                                    ChangeSpell.Invoke(
                                        this,
                                        new SpellsEventArgs{ spell = new Fire()}
                                        );
                                    break;
                                case Keys.D2:
                                    ChangeSpell.Invoke(
                                        this,
                                        new SpellsEventArgs{ spell = new Ice()}
                                        );
                                    break;
                                case Keys.Escape:
                                    _actualGameState = GameState.Menu;
                                    break;
                            }

                            if (cooldownTime > 1000)
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
                        }break;
                    case GameState.Menu:
                        if (_currentKey.IsKeyDown(Keys.Down) && _previousKey.IsKeyUp(Keys.Down))
                            MenuCounter++;
                        else if (_currentKey.IsKeyDown(Keys.Up) && _previousKey.IsKeyUp(Keys.Up))
                            MenuCounter--;
                        else if (_currentKey.IsKeyDown(Keys.Enter) && _previousKey.IsKeyUp(Keys.Enter))
                        {
                            switch (option)
                            {
                                case MenuStates.NewGame:
                                    GameReseted.Invoke(this, new EventArgs());
                                    _actualGameState = GameState.Game;
                                    break;
                                case MenuStates.ResumeGame:
                                    _actualGameState = GameState.Game;
                                    break;
                                case MenuStates.ExitGame:
                                    Exit();
                                    break;
                            }
                        }
                        break;
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
            var color = Color.White;
            switch (_actualGameState)
            {
                case GameState.Game:
                    foreach(var o in _objects.Values)
                    {
                        switch (o.UnderEffect)
                        {
                            case ISpell.MagicType.fire:
                                color = Color.Red;
                                break;
                            case ISpell.MagicType.death:
                                color = Color.DarkOliveGreen;
                                break;
                            case ISpell.MagicType.ice:
                                color = Color.DarkBlue;
                                break;
                            case ISpell.MagicType.light:
                                color = Color.LightBlue;
                                break;
                            case ISpell.MagicType.none:
                                color = Color.White;
                                break;
                        }
                            GraphicsDevice.Clear(Color.DarkGray);
                        if (o.dir == IGameplayModel.Direction.right)
                            _spriteBatch.Draw(_textures[o.ImageID], o.Pos, color);
                        else if (o.dir == IGameplayModel.Direction.left)
                            _spriteBatch.Draw(_textures[o.ImageID], o.Pos, null, color, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
                        else if (o.dir == IGameplayModel.Direction.forward)
                            _spriteBatch.Draw(_textures[o.ImageID], o.Pos, null, color, (float)-Math.PI/2f,new Vector2(64, 0), 1f, SpriteEffects.None, 0);
                        else if (o.dir == IGameplayModel.Direction.backward)
                            _spriteBatch.Draw(_textures[o.ImageID], o.Pos, null, color, (float)Math.PI/2f,new Vector2(0, 64), 1f, SpriteEffects.None, 0);
                    }
                    break;
                case GameState.Menu:
                    GraphicsDevice.Clear(Color.DarkBlue);
                    _spriteBatch.DrawString(
                        defaultFont,
                        "deep deep deep",
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.45f,
                            _graphics.PreferredBackBufferHeight * 0.1f),
                        Color.White);
                    _spriteBatch.DrawString(
                        defaultFont,
                        "New game",
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.45f, _graphics.PreferredBackBufferHeight * 0.4f),
                        option == MenuStates.NewGame ? Color.Black : Color.White);
                    _spriteBatch.DrawString(
                        defaultFont,
                        "Resume game",
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.45f, _graphics.PreferredBackBufferHeight * 0.5f),
                        option == MenuStates.ResumeGame ? Color.Black : Color.White);
                    _spriteBatch.DrawString(
                        defaultFont,
                        "Exit game",
                        new Vector2(_graphics.PreferredBackBufferWidth * 0.45f, _graphics.PreferredBackBufferHeight * 0.6f),
                        option == MenuStates.ExitGame ? Color.Black : Color.White);
                    break;
            }
            _spriteBatch.End();
        }
    }
}