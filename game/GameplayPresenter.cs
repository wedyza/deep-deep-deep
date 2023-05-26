using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game
{
    public class GameplayPresenter
    {
        private IGameplayView _gameplayView = null;
        private IGameplayModel _gameplayModel = null;

        public GameplayPresenter(
          IGameplayView gameplayView,
          IGameplayModel gameplayModel
        )
        {
            _gameplayView = gameplayView;
            _gameplayModel = gameplayModel;

            _gameplayView.CycleFinished += ViewModelUpdate;
            _gameplayView.PlayerMoved += ViewModelMovePlayer;
            _gameplayView.PlayerAttacked += ViewModelPlayerAttack;
            _gameplayModel.Updated += ModelViewUpdate;
            _gameplayView.GameReseted += ResetGame;
            
            _gameplayModel.Initialize();
        }

        private void ResetGame(object sender, EventArgs e)
        {
            _gameplayModel.Initialize();
        }
        
        private void ViewModelPlayerAttack(object sender, ControlsEventArgs e)
        {
            _gameplayModel.PlayerAttack(e.Direction);
        }
        
        private void ViewModelMovePlayer(object sender, ControlsEventArgs e)
        {
            _gameplayModel.MovePlayer(e.Direction);
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            _gameplayView.LoadGameCycleParameters(e.Objects);
        }

        private void ViewModelUpdate(object sender, EventArgs e)
        {
            _gameplayModel.Update();
        }

        public void LaunchGame()
        {
            _gameplayView.Run();
        }
    }
}
