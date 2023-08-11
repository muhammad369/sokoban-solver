

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
        SokobanPathFinder pathFinder;

        public byte GetCell(Position position)
        {
            return this.board[position.X, position.Y];
        }

        /// <summary>
        /// this method doesnt maintain boxes list, player and targetsNotFilled
        /// </summary>
        public void SetCell(Position position, byte content)
        {
            board[position.X, position.Y] = content;
        }

        public byte GetCell(int x, int y)
        {
            return this.board[x, y];
        }

        public SokobanState(int dimX, int dimY, int gCost): base(gCost)
        {
            this.board = new byte[dimX, dimY];
            pathFinder = new SokobanPathFinder(this);
            targetsNotYetFilled = 0;
            boxes = new List<Position>();
        }


        public void ClearCell(Position position)
        {
			var content = GetCell(position);
            //
            if (content == PLAYER)
            {
                player.X = player.Y = -1;
            }
            else if(content == PLAYER_IN_TARGET)
            {
				player.X = player.Y = -1;
                targetsNotYetFilled--;
			}
            else if(content == BOX)
            {
                boxes.Remove(position);
            }
            else if(content == TARGET)
            {
                targetsNotYetFilled--;
            }
            else if (content == BOX_IN_TARGET)
            {
                boxes.Remove(position);
                //targetsNotYetFilled--;
            }

            // if EMPTY or WALL nothing special needs to be done
            //
            board[position.X, position.Y] = EMPTY;
		}


        //setters
        /// Supposed to be used only when moving blocks and palyer
        public void SetEmpty(int x, int y)
        {
            SetEmpty(new Position(x, y));
        }

        public void SetEmpty(Position d)
        {
            ClearCell(d);
            this.board[d.X, d.Y] = EMPTY;
        }

        public void SetBox(int x, int y)
        {
            SetBox(new Position(x, y));
        }

        public void SetBox(Position d)
        {
            ClearCell(d);
            //
            board[d.X, d.Y] = BOX;
            boxes.Add(d);
        }


        public void SetPlayer(int x, int y)
        {
            SetPlayer(new Position(x, y));
        }


        public void SetPlayer(Position d)
        {
            ClearCell(d);
            //
            board[d.X, d.Y] = PLAYER;
            player = d;
        }

        public void SetWall(int x, int y)
        {
            SetWall(new Position(x, y));
        }

        public void SetWall(Position d)
        {
            ClearCell(d);
            //
            board[d.X, d.Y] = WALL;
        }

        public void SetTarget(int x, int y)
        {
            SetTarget(new Position(x, y));
        }

        public void SetTarget(Position d)
        {
            ClearCell(d);
            //
            this.board[d.X, d.Y] = TARGET;
            targetsNotYetFilled++;
		}

        public void SetBoxInTarget(int x, int y)
        {
            SetBoxInTarget(new Position(x, y));
        }

        public void SetBoxInTarget(Position d)
        {
            ClearCell(d);
            //
			board[d.X, d.Y] = BOX_IN_TARGET;
            boxes.Add(d);
            //targetsNotYetFilled++;
		}



        public void SetPlayerInTarget(int x, int y)
        {
            SetPlayerInTarget(new Position(x, y));
		}


        public void SetPlayerInTarget(Position d)
        {
            ClearCell(d);
            //
            board[d.X, d.Y] = PLAYER_IN_TARGET;
            player = d;
            targetsNotYetFilled++;
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
                if (!this.pathFinder.PathExists(this.player, other.player))
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
                if (pathFinder.PathExists(player, left))//right
                {
                    tmp.Add(new Position(d.X + 1, d.Y));
                }
                if (pathFinder.PathExists(player, right))//left
                {
                    tmp.Add(new Position(d.X - 1, d.Y));
                }
            }
            if (IsEmpty(up) && IsEmpty(down))//up ,down
            {
                if (pathFinder.PathExists(player, down))//up
                {
                    tmp.Add(new Position(d.X, d.Y - 1));
                }
                if (pathFinder.PathExists(player, up))//down
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
            var content = this.board[x, y];

            if (content == EMPTY)
                return true;
            else if (content == TARGET)
                return true;
            else if (content == PLAYER)
                return true;
            else if (content == PLAYER_IN_TARGET)
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
                    tmp.Add(FormState(box, move));
                }
            }
            return tmp;

        }



        //takes a box and a position to move to and form a new state
        public SokobanState FormState(Position box, Position move)
        {
            SokobanState s = this.Clone();
            s.GCost = this.GCost + 1;
            //
            //removing the player from its initial state
            if (s.GetCell(player) == PLAYER_IN_TARGET)
            {
                s.SetCell(player, TARGET);
            }
            else if (s.GetCell(player) == PLAYER)
            { 
                s.SetCell(player, EMPTY);
            }
            else throw new Exception("unexpected cell value, should contain player");
            

            //move the box
            s.boxes.Remove(box);
            s.boxes.Add(move);

            //remove box and put player
            s.player = box;
            if (s.GetCell(box) == BOX)
            {
                s.SetCell(box, PLAYER);
            }
            else if (s.GetCell(box) == BOX_IN_TARGET)
            {  //box in target
                s.SetCell(box, PLAYER_IN_TARGET);
                s.targetsNotYetFilled++;
            }
            else
            {
                throw new Exception("unexpected cell value, should contain box");
            }
			

			//adjust the position of move and put box
			if (s.GetCell(move) == TARGET)
            {//target
                s.SetCell(move, BOX_IN_TARGET);
                s.targetsNotYetFilled--;
            }
            else if (s.GetCell(move) == EMPTY)//empty
            {
                s.SetCell(move, BOX);
            }
            else throw new Exception("unexpected cell value, should be Target or Empty");

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
            if (IsBlockedState()) return 1000000;
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
                    throw new Exception("A box with no matching target");
                }
            }
            //
            return h;
		}


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
            //
            sb.AppendLine($"g+h= {GCost}+{HCost}");
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
                return ToString();
            } 
        }

	}
}
