

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

        public byte TargetsNotYetReached;
        public Position Ball;
        public List<Position> blocks;
        Router router;

        public byte getCell(Position position)
        {
            return this.board[position.X, position.Y];
        }
        public byte getCell(int x, int y)
        {
            return this.board[x, y];
        }

        public SokobanState(int dimX, int dimY)
        {
            this.board = new byte[dimX, dimY];
            router = new Router(this);
            TargetsNotYetReached = 0;
            blocks = new List<Position>();
        }


        //setters

        public void setEmpty(int x, int y)//supposed to be used only when moving blocks and palyer
        {
            this.board[x, y] = SokobanState.EMPTY;
        }

        public void setEmpty(Position d)
        {
            this.board[d.X, d.Y] = EMPTY;
        }

        public void setBlock(int x, int y)
        {
            board[x, y] = BLOCK;

        }

        public void setBlock(Position d)
        {
            this.board[d.X, d.Y] = BLOCK;
        }


        public void setBall(int x, int y)
        {
            board[x, y] = PLAYER;
            this.Ball.X = x;
            this.Ball.Y = y;
        }


        public void setBall(Position d)
        {
            this.board[d.X, d.Y] = PLAYER;
            this.Ball = d;
        }

        public void setWall(int x, int y)
        {
            this.board[x, y] = WALL;
        }

        public void setWall(Position d)
        {
            this.board[d.X, d.Y] = WALL;
        }

        public void setTarget(int x, int y)
        {
            this.board[x, y] = TARGET;
        }

        public void setTarget(Position d)
        {
            this.board[d.X, d.Y] = TARGET;
        }

        public void setBlockInTarget(int x, int y)
        {
            this.board[x, y] = BLOCK_IN_TARGET;

        }

        public void setBlockInTarget(Position d)
        {
            this.board[d.X, d.Y] = BLOCK_IN_TARGET;
        }



        public void setBallInTarget(int x, int y)
        {
            this.board[x, y] = PLAYER_IN_TARGET;
            this.Ball.X = x;
            this.Ball.Y = y;

        }


        public void setBallInTarget(Position d)
        {
            this.board[d.X, d.Y] = PLAYER_IN_TARGET;
            this.Ball = d;
        }



        //Methods

        public override bool Equals(Object o)
        {
            if (o.GetType() == typeof(SokobanState))
            {
                SokobanState other = (SokobanState)o;

                /*
                the state is considered the same iff:
                - blocks positions are equivalent
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
                if (!this.router.route(this.Ball, other.Ball))
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
            int hash = 0;
            foreach (Position item in this.blocks)
            {
                hash += item.GetHashCode();
            }
            //hash = 11 * hash + (this.blocks != null ? this.blocks.GetHashCode() : 0);
            return hash;

        }


        public override AbsState clone()
        {
            int x = this.board.GetLength(0);
            int y = this.board.GetLength(1);
            SokobanState s = new SokobanState(x, y);
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

            //clone targets not yet reached and ball
            s.TargetsNotYetReached = this.TargetsNotYetReached;
            s.Ball = new Position(this.Ball);

            return s;
        }

        ///<summary>
        ///gets the positions for valid moves of a block
        ///param d the position of the block to check its adjacencies
        ///</summary>
        public List<Position> blockValidMoves(Position d)
        {
            List<Position> tmp = new List<Position>();
            Position right = new Position(d.X + 1, d.Y);
            Position left = new Position(d.X - 1, d.Y);
            Position up = new Position(d.X, d.Y - 1);
            Position down = new Position(d.X, d.Y + 1);
            if (isEmpty(right) && isEmpty(left))//right ,left
            {
                if (router.route(Ball, left))//right
                {
                    tmp.Add(new Position(d.X + 1, d.Y));
                }
                if (router.route(Ball, right))//left
                {
                    tmp.Add(new Position(d.X - 1, d.Y));
                }
            }
            if (isEmpty(up) && isEmpty(down))//up ,down
            {
                if (router.route(Ball, down))//up
                {
                    tmp.Add(new Position(d.X, d.Y - 1));
                }
                if (router.route(Ball, up))//down
                {
                    tmp.Add(new Position(d.X, d.Y + 1));
                }
            }
            return tmp;
        }

        private bool isEmpty(Position d)
        {

            if (this.board[d.X, d.Y] == EMPTY)
                return true;
            else if (this.board[d.X, d.Y] == TARGET)
                return true;
            else if (this.board[d.X, d.Y] == PLAYER)
                return true;
            else if (this.board[d.X, d.Y] == PLAYER_IN_TARGET)
                return true;
            else
                return false;
        }

        private bool isEmpty(int x, int y)
        {
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


        /// <summary>
        /// valid changes in a state
        /// </summary>
        /// <returns></returns>
        public override List<AbsState> Next()
        {
            List<AbsState> tmp = new List<AbsState>();
            foreach (Position blk in this.blocks)
            {
                foreach (Position move in blockValidMoves(blk))
                {
                    tmp.Add(formState(blk, move));
                }
            }
            return tmp;

        }



        //takes a block and a position to move to and form a new state
        private SokobanState formState(Position blk, Position move)
        {
            SokobanState s = this.clone() as SokobanState;

            //removing the ball from its initial state
            if (s.board[Ball.X, Ball.Y] == PLAYER_IN_TARGET)
                s.setTarget(Ball);

            else if (s.board[Ball.X, Ball.Y] == PLAYER)  //ball
                s.setEmpty(Ball);

            //else throw new Exception("unexpected cell value");

            //move the block
            s.blocks.Remove(blk);
            s.blocks.Add(move);

            //remove blk and put ball
            if (s.board[blk.X, blk.Y] == BLOCK)
            {
                s.setBall(blk);
                s.Ball = blk;
            }
            else if (s.board[blk.X, blk.Y] == BLOCK_IN_TARGET)
            {  //block in target
                s.setBallInTarget(blk);
                s.Ball = blk;
                s.TargetsNotYetReached += 1;
            }
            else
            {
                //throw new Exception("unexpected cell value");
            }

            //adjust the position of move and put block
            if (s.board[move.X, move.Y] == TARGET)
            {//target
                s.setBlockInTarget(move);
                s.TargetsNotYetReached -= 1;
            }
            else if (s.board[move.X, move.Y] == EMPTY)//empty
            {
                s.setBlock(move);
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
                if (this.getCell(item) == SokobanState.BLOCK)//not block in target
                {

                    //
                    bool wallUP = false;
                    bool wallRight = false;
                    bool wallDown = false;
                    if (this.getCell(item.X, item.Y - 1) == SokobanState.WALL)//up
                    {
                        wallUP = true;
                    }
                    if (this.getCell(item.X + 1, item.Y) == SokobanState.WALL)//right
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
                    if (this.getCell(item.X, item.Y + 1) == SokobanState.WALL)//down
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
                    if (this.getCell(item.X - 1, item.Y) == SokobanState.WALL)//left
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
            return TargetsNotYetReached == 0;
        }


		public override int CalculateHeuristicCost()
		{
			throw new NotImplementedException();
		}
	}
}
