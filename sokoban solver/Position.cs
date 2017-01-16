using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sokoban_solver
{
    public struct Position
    {

        public int X;
        public int Y;

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public Position(Position p)
        {
            this.X = p.X;
            this.Y = p.Y;
        }

        public override int GetHashCode()
        {
            return this.X ^ 2 - this.Y;
        }
        
    }
}
