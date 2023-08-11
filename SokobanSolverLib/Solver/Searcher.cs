using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solver.AStar
{

    /// <summary>
    /// breadth-first sercher on the problem state graph
    /// </summary>
    public class Searcher
    {

        Dictionary<AbsState, AbsState> OpenSet = new Dictionary<AbsState, AbsState>();
		Dictionary<AbsState, AbsState> ClosedSet = new Dictionary<AbsState, AbsState>();
        
        AbsState finalState;


        /// <summary>
        /// gets the possible states from the given state removing states that have already 
        /// been produced
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private List<AbsState> getNext(AbsState state)
        {
            List<AbsState> tmp = new List<AbsState>();

            foreach (AbsState item in state.Next())
            {
                if (!ClosedSet.ContainsKey(item))
                {
                    tmp.Add(item);
                }
            }
            return tmp;
        }

        
       
        /// <summary>
        /// searches for the final state ,returning true if found false otherwise, setting the final state
        /// </summary>
        private bool search(AbsState node)
        {

            if (node.IsTargetState())
            {
                finalState = node;
                return true;
            }
            else
            {
                ClosedSet.Add(node, node);
                //
                List<AbsState> nextStates = getNext(node);
                if (nextStates.Count > 0)
                {
                    nextStates.ForEach(n => n.Parent = node);
                    //var newPath = false;

                    foreach (AbsState nextState in nextStates)
                    {
                        if (OpenSet.ContainsKey(nextState))
                        {
                            if(nextState.GCost < OpenSet[nextState].GCost)
                            {
								OpenSet[nextState].GCost = nextState.GCost;
                                OpenSet[nextState].Parent = node;
                                //newPath = true;
							}
                            // else: ignore the next state
                        }
                        else
                        {
                            OpenSet.Add(nextState, nextState);
                            //newPath = true;
                        }

                    }
                }
                //
                if (OpenSet.Count > 0)
                {
                    var winner = GetLeastCostState();

                    OpenSet.Remove(winner);

                    return search(winner);
                }
                else
                {
                    return false;
                }
            }
        }

		private AbsState GetLeastCostState()
		{
            return OpenSet.Values.Aggregate((a, b) => a.TotalCost < b.TotalCost ? a : a.TotalCost > b.TotalCost ? b : a.PenaltyCompare(b));
		}



		/// <summary>
		/// returns the entire path to solution state form the initial state, 
        /// or null if there is no solution
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public List<AbsState>? getSolution(AbsState initialState)
        {
            if (search(initialState))
            {
				List<AbsState> solutionSteps = new List<AbsState>();
                solutionSteps.Add(finalState);
                //
				for (AbsState i = finalState; i.Parent != null; i = i.Parent)
                {
                    solutionSteps.Add(i.Parent);
                }
                solutionSteps.Reverse();
                return solutionSteps;
            }
            else
            {
                return null;
            }
        }

        

    }
}