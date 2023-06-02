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
        private Texture2D grayBox;
        private Texture2D greenBox;
        private Texture2D yellowBox;
        
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
        private Dictionary<(int, int), IRooms> _rooms = new Dictionary<(int, int), IRooms>();
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
            _textures.Add((byte)GameCycle.ObjectTypes.undead, Content.Load<Texture2D>("undeadx4"));
            _textures.Add((byte)GameCycle.ObjectTypes.light, Content.Load<Texture2D>("lightingx4"));
            
            _textures.Add((byte)GameCycle.ObjectTypes.goblin, Content.Load<Texture2D>("goblinx4z"));
            _textures.Add((byte)GameCycle.ObjectTypes.boss, Content.Load<Texture2D>("bossx6"));

            _textures.Add((byte)GameCycle.ObjectTypes.book, Content.Load<Texture2D>("kniggerx2"));
            _textures.Add((byte)GameCycle.ObjectTypes.taproot, Content.Load<Texture2D>("taprootx2"));
            _textures.Add((byte)GameCycle.ObjectTypes.boots, Content.Load<Texture2D>("bootsx2"));
            _textures.Add((byte)GameCycle.ObjectTypes.claw, Content.Load<Texture2D>("clawx2"));
            _textures.Add((byte)GameCycle.ObjectTypes.cane, Content.Load<Texture2D>("canex2"));
            
            _textures.Add((byte)GameCycle.ObjectTypes.tutorial, Content.Load<Texture2D>("tutorial"));

            greenBox = Content.Load<Texture2D>("greenBox");
            grayBox = Content.Load<Texture2D>("grayBox");
            yellowBox = Content.Load<Texture2D>("yellowBox");
            
            defaultFont = Content.Load<SpriteFont>("File");
        }

        public void LoadGameCycleParameters(Dictionary<int, IObject> objects, Dictionary<(int, int), IRooms> rooms)
        {
            _objects= objects;
            _rooms = rooms;
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
            Game,
            Map
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
                                        new SpellsEventArgs{ id = 0}
                                        );
                                    break;
                                case Keys.D2:
                                    ChangeSpell.Invoke(
                                        this,
                                        new SpellsEventArgs{ id = 1}
                                        );
                                    break;
                                case Keys.D3:
                                    ChangeSpell.Invoke(
                                        this,
                                        new SpellsEventArgs{id = 2});
                                    break;
                                case Keys.D4:
                                    ChangeSpell.Invoke(
                                        this,
                                        new SpellsEventArgs{id = 3});
                                    break;
                                case Keys.Escape:
                                    _actualGameState = GameState.Menu;
                                    break;
                            }

                            if (_currentKey.IsKeyDown(Keys.M) && _previousKey.IsKeyUp(Keys.M))
                                _actualGameState = GameState.Map;
                            
                            if (cooldownTime > 500)
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
                    case GameState.Map:
                        if (_currentKey.IsKeyDown(Keys.M) && _previousKey.IsKeyUp(Keys.M))
                            _actualGameState = GameState.Game;
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
                        if (o is Player p)
                        {
                            for (var i = 1; i <= p.Spells.Count; i++)
                            {
                                var spell = p.Spells[i-1] as IObject;
                                var vector = new Vector2(_graphics.PreferredBackBufferWidth * (i * 0.1f),
                                    _graphics.PreferredBackBufferHeight * .05f);
                                _spriteBatch.DrawString(
                                    defaultFont,
                                    i.ToString(),
                                    vector,
                                    Color.Black);
                                if (p.Spells[i-1]._castType == ISpell.CastType.projectTile)
                                    _spriteBatch.Draw(
                                        _textures[spell.ImageID],
                                        vector,
                                        Color.White);
                                else 
                                    _spriteBatch.Draw(_textures[spell.ImageID], vector, null, color, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
                                _spriteBatch.DrawString(
                                    defaultFont,
                                    i.ToString(),
                                    vector,
                                    Color.White);
                            }
                            _spriteBatch.DrawString(
                                defaultFont,
                                o.HP.ToString(),
                                new Vector2(o.Pos.X + 5, o.Pos.Y - 25),
                                Color.Red);
                        }
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
                case GameState.Map:
                    GraphicsDevice.Clear(Color.DarkBlue);
                    (int min, int max) X = (0, 0);
                    (int min, int max) Y = (0, 0);
                    foreach (var k in _rooms.Keys)
                    {
                        if (_rooms[k] is Room r)
                            _spriteBatch.Draw(
                                r.PlayerInside ? yellowBox : greenBox,
                                new Vector2(_graphics.PreferredBackBufferWidth * .5f + (k.Item1 * 32), _graphics.PreferredBackBufferHeight*.5f - (k.Item2*32)),
                                    Color.White);
                        else
                            _spriteBatch.Draw(
                                grayBox,
                                new Vector2(_graphics.PreferredBackBufferWidth * .5f + (k.Item1 * 32), _graphics.PreferredBackBufferHeight*.5f - (k.Item2*32)),
                                Color.White);
                    }
                    break;
            }
            _spriteBatch.End();
        }
    }
}