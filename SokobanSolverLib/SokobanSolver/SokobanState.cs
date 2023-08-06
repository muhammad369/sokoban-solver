

using Solver.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sokoban_solver
{


    public class SokobanState : AbsState
    {

        //constants representing what in a cell
        public const byte PLAYER = 1;
        public const byte EMPTY = 0;
        public const byte WALL = 2;
        public const byte BLOCK = 3;
        public const byte TARGET = 4;
        public const byte BLOCK_IN_TARGET = 5;
        public const byte PLAYER_IN_TARGET = 6;

        public byte[,] board;

        public byte targetsNotYetFilled;
        public Position player;
        public List<Position> blocks;
        Router router;

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
            router = new Router(this);
            targetsNotYetFilled = 0;
            blocks = new List<Position>();
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

        public void SetBlock(int x, int y)
        {
            SetBlock(new Position(x, y));
        }

        public void SetBlock(Position d)
        {
            board[d.X, d.Y] = BLOCK;
            blocks.Add(d);
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

        public void SetBlockInTarget(int x, int y)
        {
            SetBlockInTarget(new Position(x, y));
        }

        public void SetBlockInTarget(Position d)
        {
			board[d.X, d.Y] = BLOCK_IN_TARGET;
			blocks.Add(d);
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
                - blocks positions are equivalent (not neccessarily in order)
                - player position in one of them has route a to the other
                */

                foreach (Position item in this.blocks)
                {
                    if (!other.blocks.Contains(item))
                    {
                        //if any key doesn't exist in other return false
                        return false;
                    }
                }
                //
                if (!this.router.RouteExists(this.player, other.player))
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
            // avoid the effect of blocks order, by ordering them
            return string.Join("", blocks.Select(b => $"{b.X}{b.Y}").OrderBy(b=> b)).GetHashCode();
        }


        public override AbsState Clone()
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
            //clone blocks

            foreach (Position item in this.blocks)
            {
                s.blocks.Add(item);
            }

            //clone targets not yet reached and player
            s.targetsNotYetFilled = this.targetsNotYetFilled;
            s.player = new Position(this.player);

            return s;
        }

        ///<summary>
        ///gets the positions for valid moves of a block
        ///param d the position of the block to check its adjacencies
        ///</summary>
        public List<Position> GetBlockValidMoves(Position d)
        {
            List<Position> tmp = new List<Position>();
            Position right = new Position(d.X + 1, d.Y);
            Position left = new Position(d.X - 1, d.Y);
            Position up = new Position(d.X, d.Y - 1);
            Position down = new Position(d.X, d.Y + 1);
            if (IsEmpty(right) && IsEmpty(left))//right ,left
            {
                if (router.RouteExists(player, left))//right
                {
                    tmp.Add(new Position(d.X + 1, d.Y));
                }
                if (router.RouteExists(player, right))//left
                {
                    tmp.Add(new Position(d.X - 1, d.Y));
                }
            }
            if (IsEmpty(up) && IsEmpty(down))//up ,down
            {
                if (router.RouteExists(player, down))//up
                {
                    tmp.Add(new Position(d.X, d.Y - 1));
                }
                if (router.RouteExists(player, up))//down
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
            List<AbsState> tmp = new List<AbsState>();
            foreach (Position blk in this.blocks)
            {
                foreach (Position move in GetBlockValidMoves(blk))
                {
                    tmp.Add(formState(blk, move));
                }
            }
            return tmp;

        }



        //takes a block and a position to move to and form a new state
        private SokobanState formState(Position blk, Position move)
        {
            SokobanState s = this.Clone() as SokobanState;

            //removing the ball from its initial state
            if (s.board[player.X, player.Y] == PLAYER_IN_TARGET)
                s.SetTarget(player);

            else if (s.board[player.X, player.Y] == PLAYER)  //ball
                s.SetEmpty(player);

            //else throw new Exception("unexpected cell value");

            //move the block
            s.blocks.Remove(blk);
            s.blocks.Add(move);

            //remove blk and put ball
            if (s.board[blk.X, blk.Y] == BLOCK)
            {
                s.SetPlayer(blk);
                s.player = blk;
            }
            else if (s.board[blk.X, blk.Y] == BLOCK_IN_TARGET)
            {  //block in target
                s.SetPlayerInTarget(blk);
                s.player = blk;
                s.targetsNotYetFilled += 1;
            }
            else
            {
                //throw new Exception("unexpected cell value");
            }

            //adjust the position of move and put block
            if (s.board[move.X, move.Y] == TARGET)
            {//target
                s.SetBlockInTarget(move);
                s.targetsNotYetFilled -= 1;
            }
            else if (s.board[move.X, move.Y] == EMPTY)//empty
            {
                s.SetBlock(move);
            }
            //else throw new Exception("unexpected cell value");

            //now return the state
            return s;

        }

        #region blocked state optimization

        public bool isBlockedState()
        {
            foreach (Position item in this.blocks)
            {
                if (this.GetCell(item) == SokobanState.BLOCK)//not block in target
                {

                    //
                    bool wallUP = false;
                    bool wallRight = false;
                    bool wallDown = false;
                    if (this.GetCell(item.X, item.Y - 1) == SokobanState.WALL)//up
                    {
                        wallUP = true;
                    }
                    if (this.GetCell(item.X + 1, item.Y) == SokobanState.WALL)//right
                    {
                        if (wallUP)
                        {
                            return true;
                        }
                        else
                        {
                            wallRight = true;
                        }

                    }
                    if (this.GetCell(item.X, item.Y + 1) == SokobanState.WALL)//down
                    {
                        if (wallRight)
                        {
                            return true;
                        }
                        else
                        {
                            wallDown = true;
                        }
                    }
                    if (this.GetCell(item.X - 1, item.Y) == SokobanState.WALL)//left
                    {
                        if (wallDown || wallUP)
                        {
                            return true;
                        }

                    }

                }
            }
            //reaching here means no block is in bad position
            return false;
        }
        #endregion

        public override bool IsTargetState()
        {
            return targetsNotYetFilled == 0;
        }


		public override int CalculateHeuristicCost()
		{
            if (isBlockedState()) return int.MaxValue;
            //
            var router = new Router(this);
            var matchedTargets = new List<Position>();
            var h = 0;
            //
            foreach (var block in blocks)
            {
                if (GetCell(block) == BLOCK_IN_TARGET)
                {
                    continue;
                }
                //
                var (p, d) = router.NearestWhere(block, (p, c) => (c == TARGET || c == PLAYER_IN_TARGET) && (!matchedTargets.Contains(p)));

                if(p != null)
                {
                    h += d;
                    matchedTargets.Add((Position)p);
                }
                else
                {
                    h += board.Length;
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
                case BLOCK: return 'O';
				case PLAYER: return 'Ω';
				case TARGET: return 'X';
				case BLOCK_IN_TARGET: return 'ф';
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
