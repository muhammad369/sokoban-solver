using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver
{
	public static class PositionExtensions
	{
		public static Position Right(this Position position)
		{
			return new Position(position.X + 1, position.Y);
		}

		public static Position Left(this Position position)
		{
			return new Position(position.X - 1, position.Y);
		}

		public static Position Above(this Position position)
		{
			return new Position(position.X, position.Y - 1);
		}

		public static Position Below(this Position position)
		{
			return new Position(position.X, position.Y + 1);
		}

		public static bool IsValid(this Position position, byte[,] board)
		{
			return position.X > -1 && position.X < board.GetLength(0)
				&& position.Y > -1 && position.Y < board.GetLength(1);
		}

		/// <summary>
		/// a position is blocking for adjacencies if it is edge of the board, a wall, or a box that is blocked
		/// </summary>
		public static bool IsBlocking(this Position position, byte[,] board, List<Position>? givenBlockings = null)
		{
			if (!position.IsValid(board)) return true;
			//
			var content = board[position.X, position.Y];
			if (content == SokobanState.WALL) return true;
			//
			if ((content == SokobanState.BOX || content == SokobanState.BOX_IN_TARGET) && position.IsBlocked(board, givenBlockings))
			{
				return true;
			}
			// else
			return false;
		}

		/// <summary>
		/// a position is blocked iff there are blocking positions from two consecutive directions (above, right, below, left)
		/// </summary>
		public static bool IsBlocked(this Position position, byte[,] board, List<Position>? givenBlockings = null)
		{
			if(givenBlockings == null) givenBlockings = new List<Position>();
			givenBlockings.Add(position);
			//
			var above = position.Above(); 
			var blockedAbove = above.InList(givenBlockings) || above.IsBlocking(board, givenBlockings);
			//
			var right = position.Right();
			var blockedRight = right.InList(givenBlockings) || right.IsBlocking(board, givenBlockings);

			if (blockedAbove && blockedRight) return true;

			var below = position.Below();
			var blockedBelow = below.InList(givenBlockings) || below.IsBlocking(board, givenBlockings);

			if (blockedRight && blockedBelow) return true;

			var left = position.Left();
			var blockedLeft = left.InList(givenBlockings) || left.IsBlocking(board, givenBlockings);
			//
			if (blockedBelow && blockedLeft) return true;
			if (blockedLeft && blockedAbove) return true;
			//
			return false;

		}

		public static bool InList(this Position position, List<Position>? list)
		{
			return list != null && list.Contains(position);
		}

	}
}
