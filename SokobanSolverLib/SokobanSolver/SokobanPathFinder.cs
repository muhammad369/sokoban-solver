﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SokobanSolver
{

    public class SokobanPathFinder
    {

        private SokobanState state;
        private bool[,] visited;

        public SokobanPathFinder(SokobanState state)
        {
            this.state = state;
        }

        /// <summary>
        ///  Finds if there is route from cell to another,
        /// </summary>
        public bool PathExists(Position from, Position to, Queue<Position>? q = null)
        {
            if (q == null)
            {
                visited = new bool[this.state.board.GetLength(0), this.state.board.GetLength(1)];
                q = new Queue<Position>();
            }

            if (from.Equals(to))
            {
                return true;
            }
            //
            SetVisited(from);
            List<Position> adj = GetAdjacencies(from);
            if (adj.Count > 0)
            {
                //setVisited(adj);
                foreach (Position item in adj)
                {
                    q.Enqueue(item);
                }
            }
            
            //
            if (q.Count > 0)
            {
                return PathExists(q.Dequeue(), to, q);
            }
            else
            {
                return false;
            }
        }

       

        private void SetVisited(List<Position> adjs)
        {
            foreach (Position item in adjs)
            {
                
                visited[item.X, item.Y] = true;
            }
        }

        private void SetVisited(Position pos)
        {
            visited[pos.X, pos.Y] = true;
        }

        //finding adjacencies for routing
        private List<Position> GetAdjacencies(Position d)
        {
            List<Position> tmp = new List<Position>();

            Position right = new Position(d.X + 1, d.Y);
            Position left =  new Position(d.X - 1, d.Y);
            Position up =    new Position(d.X, d.Y - 1);
            Position down =  new Position(d.X, d.Y + 1);
            //
            if (IsValidPosition(right.X, right.Y) 
                && !visited[d.X + 1, d.Y] 
                && GetCell(right) != SokobanState.WALL 
                && GetCell(right) != SokobanState.BOX 
                && GetCell(right) != SokobanState.BOX_IN_TARGET)//right
            {
                tmp.Add(new Position(d.X + 1, d.Y));
            }
            if (IsValidPosition(left.X, left.Y) 
                && !visited[d.X - 1, d.Y] 
                && GetCell(left) != SokobanState.WALL 
                && GetCell(left) != SokobanState.BOX 
                && GetCell(left) != SokobanState.BOX_IN_TARGET)//left
            {
                tmp.Add(new Position(d.X - 1, d.Y));
            }
            if (IsValidPosition(up.X, up.Y) 
                && !visited[d.X, d.Y - 1] 
                && GetCell(up) != SokobanState.WALL 
                && GetCell(up) != SokobanState.BOX 
                && GetCell(up) != SokobanState.BOX_IN_TARGET)//up
            {
                tmp.Add(new Position(d.X, d.Y - 1));
            }
            if (IsValidPosition(down.X, down.Y) 
                && !visited[d.X, d.Y + 1] 
                && GetCell(down) != SokobanState.WALL 
                && GetCell(down) != SokobanState.BOX 
                && GetCell(down) != SokobanState.BOX_IN_TARGET)//down
            {
                tmp.Add(new Position(d.X, d.Y + 1));
            }

            return tmp;
            
        }

        /// <summary>
        /// The same as GetAjacencies but treats boxes cells as possible path
        /// </summary>
		private List<Position> GetAdjacenciesForEvaluation(Position d)
		{
			List<Position> tmp = new List<Position>();

			Position right = new Position(d.X + 1, d.Y);
			Position left = new Position(d.X - 1, d.Y);
			Position up = new Position(d.X, d.Y - 1);
			Position down = new Position(d.X, d.Y + 1);
			//
			if (IsValidPosition(right.X, right.Y)
				&& !visited[d.X + 1, d.Y]
				&& GetCell(right) != SokobanState.WALL)//right
			{
				tmp.Add(new Position(d.X + 1, d.Y));
			}
            //
			if (IsValidPosition(left.X, left.Y)
				&& !visited[d.X - 1, d.Y]
				&& GetCell(left) != SokobanState.WALL)//left
			{
				tmp.Add(new Position(d.X - 1, d.Y));
			}
			if (IsValidPosition(up.X, up.Y)
				&& !visited[d.X, d.Y - 1]
				&& GetCell(up) != SokobanState.WALL)//up
			{
				tmp.Add(new Position(d.X, d.Y - 1));
			}
			if (IsValidPosition(down.X, down.Y)
				&& !visited[d.X, d.Y + 1]
				&& GetCell(down) != SokobanState.WALL)//down
			{
				tmp.Add(new Position(d.X, d.Y + 1));
			}

			return tmp;
		}

		private byte GetCell(Position p)
        {
            return state.GetCell(p);
        }

        private bool IsValidPosition(int x, int y)
        {
            return (x > -1 && x < state.board.GetLength(0) && y > -1 && y < state.board.GetLength(1));
        }


		/// <summary>
		///  Finds the nearest cell from a given position that satisfies a certain condition
		/// </summary>
        /// <returns>a tuple of the position and the distance if found, null and -1 otherwise</returns>
		public (Position?, int) NearestWhere(Position from, Func<Position, byte, bool> condition, Queue<(Position, int)>? q = null, int accumulatedDistance = 0)
		{
			if (q == null)
			{
				visited = new bool[this.state.board.GetLength(0), this.state.board.GetLength(1)];
				q = new Queue<(Position, int)>();
			}

			if (condition(from, state.GetCell(from)))
			{
				return (from, accumulatedDistance);
			}
			//
			SetVisited(from);
			List<Position> adj = GetAdjacenciesForEvaluation(from);
			if (adj.Count > 0)
			{
				
				foreach (Position item in adj)
				{
					q.Enqueue((item, accumulatedDistance+1));
				}
			}
			//
			if (q.Count > 0)
			{
                var (item, d) = q.Dequeue();
				return NearestWhere(item, condition, q, d);
			}
			else
			{
				return (null, -1);
			}
		}

		
	}
}
