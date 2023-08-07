

using Solver.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SokobanSolver
{


    public class SokobanState : AbsState
    {

        //constants representing what in a cell
        public const byte PLAYER = 1;
        public const byte EMPTY = 0;
        public const byte WALL = 2;
        public const byte BOX = 3;
        public const byte TARGET = 4;
        public const byte BOX_IN_TARGET = 5;
        public const byte PLAYER_IN_TARGET = 6;

        public byte[,] board;

        public byte targetsNotYetFilled;
        public Position player;
        public List<Position> boxes;
        SokobanPathFinder router;

        public byte GetCell(Position position)
        {
            return this.board[position.X, position.Y];
        }

        public byte GetCell(int x, int y)
        {
            return this.board[x, y];
        }

        public SokobanState(int dimX, int dimY, int gCost): base(gCost)
        {
            this.board = new byte[dimX, dimY];
            router = new SokobanPathFinder(this);
            targetsNotYetFilled = 0;
            boxes = new List<Position>();
        }


        //setters
        /// Supposed to be used only when moving blocks and palyer
        public void SetEmpty(int x, int y)
        {
            this.board[x, y] = SokobanState.EMPTY;
        }

        public void SetEmpty(Position d)
        {
            this.board[d.X, d.Y] = EMPTY;
        }

        public void SetBox(int x, int y)
        {
            SetBox(new Position(x, y));
        }

        public void SetBox(Position d)
        {
            board[d.X, d.Y] = BOX;
            boxes.Add(d);
        }


        public void SetPlayer(int x, int y)
        {
            board[x, y] = PLAYER;
            this.player.X = x;
            this.player.Y = y;
        }


        public void SetPlayer(Position d)
        {
            SetPlayer(d.X, d.Y);
        }

        public void SetWall(int x, int y)
        {
            this.board[x, y] = WALL;
        }

        public void SetWall(Position d)
        {
            this.board[d.X, d.Y] = WALL;
        }

        public void SetTarget(int x, int y)
        {
            this.board[x, y] = TARGET;
        }

        public void SetTarget(Position d)
        {
            this.board[d.X, d.Y] = TARGET;
        }

        public void SetBoxInTarget(int x, int y)
        {
            SetBoxInTarget(new Position(x, y));
        }

        public void SetBoxInTarget(Position d)
        {
			board[d.X, d.Y] = BOX_IN_TARGET;
			boxes.Add(d);
		}



        public void SetPlayerInTarget(int x, int y)
        {
            this.board[x, y] = PLAYER_IN_TARGET;
            this.player.X = x;
            this.player.Y = y;

        }


        public void SetPlayerInTarget(Position d)
        {
            SetPlayerInTarget(d.X, d.Y);
        }



        //Methods

        public override bool Equals(Object o)
        {
            if (o.GetType() == typeof(SokobanState))
            {
                SokobanState other = (SokobanState)o;

                /*
                the state is considered the same iff:
                - boxes positions are equivalent (not neccessarily in order)
                - player position in one of them has route a to the other
                */

                foreach (Position item in this.boxes)
                {
                    if (!other.boxes.Contains(item))
                    {
                        //if any key doesn't exist in other return false
                        return false;
                    }
                }
                //
                if (!this.router.PathExists(this.player, other.player))
                {
                    return false;
                }
                //reach here only if it satisfies the two conditions
                return true;
            }
            else { return false; }
        }


        public override int GetHashCode()
        {
            // avoid the effect of boxes order, by ordering them
            return string.Join("", boxes.Select(b => $"{b.X}{b.Y}").OrderBy(b=> b)).GetHashCode();
        }


        public SokobanState Clone()
        {
            int x = this.board.GetLength(0);
            int y = this.board.GetLength(1);
            SokobanState s = new SokobanState(x, y, GCost);
            //clone board
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    s.board[i, j] = this.board[i, j];
                }
            }
            //clone boxes

            foreach (Position item in this.boxes)
            {
                s.boxes.Add(item);
            }

            //clone targets not yet reached and player
            s.targetsNotYetFilled = this.targetsNotYetFilled;
            s.player = new Position(this.player);

            return s;
        }

        ///<summary>
        ///gets the positions for valid moves of a box
        ///param d the position of the box to check its adjacencies
        ///</summary>
        public List<Position> GetBoxValidMoves(Position d)
        {
            List<Position> tmp = new List<Position>();
            Position right = new Position(d.X + 1, d.Y);
            Position left = new Position(d.X - 1, d.Y);
            Position up = new Position(d.X, d.Y - 1);
            Position down = new Position(d.X, d.Y + 1);
            if (IsEmpty(right) && IsEmpty(left))//right ,left
            {
                if (router.PathExists(player, left))//right
                {
                    tmp.Add(new Position(d.X + 1, d.Y));
                }
                if (router.PathExists(player, right))//left
                {
                    tmp.Add(new Position(d.X - 1, d.Y));
                }
            }
            if (IsEmpty(up) && IsEmpty(down))//up ,down
            {
                if (router.PathExists(player, down))//up
                {
                    tmp.Add(new Position(d.X, d.Y - 1));
                }
                if (router.PathExists(player, up))//down
                {
                    tmp.Add(new Position(d.X, d.Y + 1));
                }
            }
            return tmp;
        }

        private bool IsEmpty(Position d)
        {
            return IsEmpty(d.X, d.Y);
        }

        private bool IsEmpty(int x, int y)
        {
            if(!IsValidPosition(x, y)) return false;
            //
            if (this.board[x, y] == EMPTY)
                return true;
            else if (this.board[x, y] == TARGET)
                return true;
            else if (this.board[x, y] == PLAYER)
                return true;
            else if (this.board[x, y] == PLAYER_IN_TARGET)
                return true;
            else
                return false;
        }

		private bool IsValidPosition(int x, int y)
		{
			return (x > -1 && x < board.GetLength(0) && y > -1 && y < board.GetLength(1));
		}


		/// <summary>
		/// gets valid changes in a state
		/// </summary>
		/// <returns></returns>
		public override List<AbsState> Next()
        {
            if(IsBlockedState()) return new List<AbsState>();
            //
            List<AbsState> tmp = new List<AbsState>();
            foreach (Position box in this.boxes)
            {
                foreach (Position move in GetBoxValidMoves(box))
                {
                    tmp.Add(formState(box, move));
                }
            }
            return tmp;

        }



        //takes a box and a position to move to and form a new state
        private SokobanState formState(Position box, Position move)
        {
            SokobanState s = this.Clone() as SokobanState;
            s.GCost = this.GCost + 1;

            //removing the ball from its initial state
            if (s.board[player.X, player.Y] == PLAYER_IN_TARGET)
                s.SetTarget(player);

            else if (s.board[player.X, player.Y] == PLAYER)  //ball
                s.SetEmpty(player);

            //else throw new Exception("unexpected cell value");

            //move the block
            s.boxes.Remove(box);
            s.boxes.Add(move);

            //remove blk and put ball
            if (s.board[box.X, box.Y] == BOX)
            {
                s.SetPlayer(box);
                s.player = box;
            }
            else if (s.board[box.X, box.Y] == BOX_IN_TARGET)
            {  //block in target
                s.SetPlayerInTarget(box);
                s.player = box;
                s.targetsNotYetFilled += 1;
            }
            else
            {
                //throw new Exception("unexpected cell value");
            }

            //adjust the position of move and put block
            if (s.board[move.X, move.Y] == TARGET)
            {//target
                s.SetBoxInTarget(move);
                s.targetsNotYetFilled -= 1;
            }
            else if (s.board[move.X, move.Y] == EMPTY)//empty
            {
                s.SetBox(move);
            }
            //else throw new Exception("unexpected cell value");

            //now return the state
            return s;

        }

        #region blocked state

        bool? _isBlockedState = null;

        public bool IsBlockedState()
        {
            if(_isBlockedState != null) { return (bool)_isBlockedState; }
            //
            foreach (Position p in this.boxes)
            {
                if (this.GetCell(p) == SokobanState.BOX && p.IsBlocked(board))//not block in target
                {
                    _isBlockedState = true;
                    return true;
                }
            }
            //reaching here means no boxes is in bad position
            _isBlockedState = false;
            return false;
        }
        #endregion

        public override bool IsTargetState()
        {
            return targetsNotYetFilled == 0;
        }


		public override int CalculateHeuristicCost()
		{
            if (IsBlockedState()) return int.MaxValue;
            //
            var router = new SokobanPathFinder(this);
            var matchedTargets = new List<Position>();
            var h = 0;
            //
            foreach (var box in boxes)
            {
                if (GetCell(box) == BOX_IN_TARGET)
                {
                    continue;
                }
                //
                var (p, d) = router.NearestWhere(box, (p, c) => (c == TARGET || c == PLAYER_IN_TARGET) && (!matchedTargets.Contains(p)));

                if(p != null)
                {
                    h += d;
                    matchedTargets.Add((Position)p);
                }
                else
                {
                    Console.WriteLine("A case that shouldn't happen, box with no matching target");
                    h += board.GetLength(0);
                }
            }
            //
            return h;
		}


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
            //
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    sb.Append(boardChar( board[i, j]));
                    sb.Append(' ');
                }
                sb.Append("\n");
            }
            //
            return sb.ToString();
		}

		private char boardChar(byte v)
		{
			switch (v)
            {
                case WALL: return '■';
                case BOX: return 'O';
				case PLAYER: return 'Ω';
				case TARGET: return 'X';
				case BOX_IN_TARGET: return 'ф';
				case PLAYER_IN_TARGET: return 'Ω';
				case EMPTY: return '□';
                default: return '\0';
			}
		}

        public string Description 
        { 
            get
            {
                return ToString() + $"\n g+h= {GCost}+{HCost}";
            } 
        }

	}
}
