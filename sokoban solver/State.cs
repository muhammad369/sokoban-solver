

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sokoban_solver
{


public class State {

    //constants representing what in a cell
    public static byte  ball = 1;
    public static byte empty = 0;
    public static byte wall = 2;
    public static byte block = 3;
    public static byte target = 4;
    public static byte blockInTarget = 5;
    public static byte ballInTarget = 6;

    public byte[,] board;

    public byte TargetsNotYetReached;
    public Position Ball;
    public List<Position> blocks;
    Router router;

    public byte getCell(Position position)
    {
        return this.board[position.X,position.Y];
    }
    public byte getCell(int x,int y)
    {
        return this.board[x,y];
    }

    public State(int dimX,int dimY)
    {
        this.board=new byte[dimX,dimY];
        router=new Router(this);
        TargetsNotYetReached=0;
        blocks=new List<Position>();
    }


    //setters

    public void setEmpty(int x,int y)//supposed to be used only when moving blocks and ball
    {
        this.board[x,y]=State.empty;
    }
    public void setEmpty(Position d)
    {
        this.board[d.X,d.Y]=empty;
    }
    public void setBlock(int x,int y)
    {
        board[x,y]=block;
        
    }
    public void setBlock(Position d)
    {
        this.board[d.X,d.Y]=block;
    }
    

    public void setBall(int x,int y)
    {
        board[x,y]=ball;
        this.Ball.X=x;
        this.Ball.Y=y;
    }
    

    public void setBall(Position d)
    {
        this.board[d.X,d.Y]=ball;
        this.Ball=d;
    }
    public void setWall(int x,int y)
    {
        this.board[x,y]=wall;
    }

    public void setWall(Position d)
    {
        this.board[d.X,d.Y]=wall;
    }

    public void setTarget(int x,int y)
    {
        this.board[x,y]=target;
    }

    public void setTarget(Position d)
    {
        this.board[d.X,d.Y]=target;
    }

    public void setBlockInTarget(int x,int y)
    {
        this.board[x,y]=blockInTarget;

    }
    public void setBlockInTarget(Position d)
    {
        this.board[d.X,d.Y]=blockInTarget;
    }



    public void setBallInTarget(int x,int y)
    {
        this.board[x,y]=ballInTarget;
        this.Ball.X=x;
        this.Ball.Y=y;

    }
   

    public void setBallInTarget(Position d)
    {
        this.board[d.X,d.Y]=ballInTarget;
        this.Ball=d;
    }
    


    //Methods
    
    public override bool Equals(Object o)
    {
        if (o.GetType() == typeof(State))
        {
            State other = (State)o;

            /*
            the state is considered the same iff:
            - blocks positions are equivalent
            - ball position in one of them has route a to the other
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

    
    public State clone()
    {
        int x=this.board.GetLength(0);
        int y=this.board.GetLength(1);
        State s=new State(x, y);
        //clone board
        for(int i=0;i<x;i++)
        {
            for(int j=0;j<y;j++)
            {
                s.board[i,j]=this.board[i,j];
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

        if(this.board[d.X,d.Y] == empty)
            return true;
        else if(this.board[d.X,d.Y] == target)
            return true;
        else if(this.board[d.X,d.Y] == ball)
            return true;
        else if (this.board[d.X, d.Y] == ballInTarget)
            return true;
        else
            return false;
    }

    private bool isEmpty(int x,int y)
    {
        if(this.board[x,y] == empty)
            return true;
        else if(this.board[x,y] == target)
            return true;
        else if(this.board[x,y] == ball)
            return true;
        else if(this.board[x,y] == ballInTarget)
            return true;
        else
            return false;
    }


    /// <summary>
    /// valid changes in a state
    /// </summary>
    /// <returns></returns>
    public List<State> Next()
    {
        List<State> tmp = new List<State>();
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
    private State formState(Position blk, Position move)
    {
        State s = this.clone();

        //removing the ball from its initial state
        if (s.board[Ball.X, Ball.Y] == ballInTarget)
            s.setTarget(Ball);

        else if (s.board[Ball.X, Ball.Y] == ball)  //ball
            s.setEmpty(Ball);

        //else throw new Exception("unexpected cell value");

        //move the block
        s.blocks.Remove(blk);
        s.blocks.Add(move);

        //remove blk and put ball
        if (s.board[blk.X, blk.Y] == block)
        {
            s.setBall(blk);
            s.Ball = blk;
        }
        else if (s.board[blk.X, blk.Y] == blockInTarget)
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
        if (s.board[move.X, move.Y] == target)
        {//target
            s.setBlockInTarget(move);
            s.TargetsNotYetReached -= 1;
        }
        else if (s.board[move.X, move.Y] == empty)//empty
        {
            s.setBlock(move);
        }
        //else throw new Exception("unexpected cell value");

        //now return the state
        return s;

    }

}
}
