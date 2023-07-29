using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sokoban_solver
{

    public class Router
    {

        private SokobanState state;
        private bool[,] visited;
        private Queue<Position> queue;

        public Router(SokobanState state)
        {
            this.state = state;
        }

        //finding route from cell to other,
        public bool route(Position from, Position to)
        {
            visited = new bool[this.state.board.GetLength(0), this.state.board.GetLength(1)];
            queue = new Queue<Position>();
            if (from.Equals(to))
            {
                return true;
            }
            else
            {
                setVisited(from);
                List<Position> adj = getAdjacencies(from);
                if (adj.Count > 0)
                {
                    //setVisited(adj);
                    foreach (Position item in adj)
                    {
                        queue.Enqueue(item);
                    }
                    //
                    if (queue.Count > 0)
                    {
                        return Route(queue.Dequeue(), to);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        // used by route method
        private bool Route(Position from, Position to)
        {
            if (from.Equals(to))
            {
                return true;
            }
            else
            {
                setVisited(from);
                List<Position> adj = getAdjacencies(from);
                if (adj.Count > 0)  
                {
                    //setVisited(adj);
                    foreach (Position item in adj)
                    {
                        queue.Enqueue(item);
                    }
                }
                //
                if (queue.Count > 0)
                {
                    return Route(queue.Dequeue(), to);
                }
                else
                {
                    return false;
                }
            }
        }

        private void setVisited(List<Position> adjs)
        {
            foreach (Position item in adjs)
            {
                
                visited[item.X, item.Y] = true;
            }
        }

        private void setVisited(Position pos)
        {
            visited[pos.X, pos.Y] = true;
        }

        //finding adjacencies for routing
        List<Position> getAdjacencies(Position d)
        {
            List<Position> tmp = new List<Position>();
            byte right = this.state.board[d.X + 1, d.Y];
            byte left = this.state.board[d.X - 1, d.Y];
            byte up = this.state.board[d.X, d.Y - 1];
            byte down = this.state.board[d.X, d.Y + 1];
            if (!visited[d.X + 1, d.Y] && right != SokobanState.WALL && right != SokobanState.BLOCK && right != SokobanState.BLOCK_IN_TARGET)//right
            {
                tmp.Add(new Position(d.X + 1, d.Y));
            }
            if (!visited[d.X - 1, d.Y] && left != SokobanState.WALL && left != SokobanState.BLOCK && left != SokobanState.BLOCK_IN_TARGET)//left
            {
                tmp.Add(new Position(d.X - 1, d.Y));
            }
            if (!visited[d.X, d.Y - 1] && up != SokobanState.WALL && up != SokobanState.BLOCK && up != SokobanState.BLOCK_IN_TARGET)//up
            {
                tmp.Add(new Position(d.X, d.Y - 1));
            }
            if (!visited[d.X, d.Y + 1] && down != SokobanState.WALL && down != SokobanState.BLOCK && down != SokobanState.BLOCK_IN_TARGET)//down
            {
                tmp.Add(new Position(d.X, d.Y + 1));
            }

            return tmp;
            
        }


    }
}
