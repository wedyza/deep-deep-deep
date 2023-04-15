using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace game
{
	public interface IGameplayModel
	{
		event EventHandler<GameplayEventArgs> Updated;

		void Update();
		void MovePlayer(Direction dir);

		public enum Direction : byte
		{
			forward,
			backward,
			right,
			left
		}
	}

	public class GameplayEventArgs : EventArgs
	{
		public Vector2 PlayerPos { get; set; }
	}
}