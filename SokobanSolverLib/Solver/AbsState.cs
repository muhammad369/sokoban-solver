using System;
using System.Collections.Generic;


namespace Solver.AStar
{
    public abstract class AbsState
    {

        public AbsState(int gCost)
        {
            this.GCost = gCost;
        }

        //public abstract AbsState Clone();

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
        public int HCost { get { if (_hCost == -1) { _hCost = CalculateHeuristicCost(); } return _hCost; } }
        private int _hCost = -1;

        public int TotalCost { get { return GCost + HCost; } }

        /// <summary>
        /// Compare by any kind of Penalties to make preference on the states with equal total costs
        /// </summary>
        public abstract AbsState PenaltyCompare(AbsState other);
       
    }
}
