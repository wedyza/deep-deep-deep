using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using deep_deep_deep;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace game
{
    public interface IGameplayView
    {
        event EventHandler CycleFinished;
        event EventHandler<ControlsEventArgs> PlayerMoved;
        event EventHandler<ControlsEventArgs> PlayerAttacked;
        event EventHandler GameReseted;

        GameCycleView.GameState ActualGameState { get; set; }
        
        void LoadGameCycleParameters(Dictionary<int, IObject> objects);
        void Run();
    }

    public class ControlsEventArgs : EventArgs
    {
        public IGameplayModel.Direction Direction { get; set; }
    }
}
