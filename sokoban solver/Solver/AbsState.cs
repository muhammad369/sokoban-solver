using System;
using System.Collections.Generic;


namespace Solver.AStar
{
    public abstract class AbsState
    {

        public AbsState(int fCost = 0)
        {
            this.GCost = fCost;
            this.HCost = CalculateHeuristicCost();
        }

        public abstract AbsState clone();

        public abstract List<AbsState> Next();

        public abstract bool IsTargetState();

        public abstract int CalculateHeuristicCost();

        public AbsState Parent { get; set; }

        /// <summary>
        /// the counted number of steps from start to reach this state
        /// </summary>
        public int GCost { get; set; }
        /// <summary>
        /// the expected number of steps to reach the final state
        /// </summary>
        public int HCost { get; set; }

        public int TotalCost { get { return GCost + HCost; } }
    }
}
