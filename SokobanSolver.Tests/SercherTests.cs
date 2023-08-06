using sokoban_solver;
using Solver.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver.Tests
{
	[TestClass]
	public class SercherTests
	{

		[TestMethod] 
		public void heuristic() 
		{
			PositionState.Target = new Position(10,5);
			var p = new PositionState(new Position(1,1), 0);


			var h = p.HCost;

			Assert.AreEqual(13, h);

		}

		[TestMethod]
		public void SearchTest()
		{
			var searcher = new Searcher();

			PositionState.Target = new Position(5, 0);
			var startingstate = new PositionState(new Position(0, 0), 0);


			var solSteps = searcher.getSolution(startingstate);

			Assert.IsNotNull(solSteps);
			Assert.AreEqual(5, solSteps.Count);

			for (int i = 0; i < 5; i++)
			{
				var item = solSteps[i] as PositionState;
				Assert.AreEqual(i+1, item!.position.X);
				Assert.AreEqual(0, item!.position.Y);
			}

		}
	}


	class PositionState : AbsState
	{
		public static Position Target;
		public static Position[] BlockedPoints = {new Position(1,1),  new Position(2,2)};
		public Position position;

		public PositionState(Position position, int gCost): base(gCost)
		{
			this.position = position;
		}

		public override int CalculateHeuristicCost()
		{
			return Math.Abs(position.X - Target.X) + Math.Abs(position.Y - Target.Y);
		}

		public override AbsState Clone()
		{
			return new PositionState(new Position(position.X, position.Y), GCost);
		}

		public override bool IsTargetState()
		{
			return position.Y == Target.Y && position.X == Target.X;
		}

		public override List<AbsState> Next()
		{
			var next = new List<AbsState>();

			next.Add(new PositionState(new Position(position.X+1, position.Y), GCost + 1));
			next.Add(new PositionState(new Position(position.X-1, position.Y), GCost + 1));
			next.Add(new PositionState(new Position(position.X, position.Y+1), GCost + 1));
			next.Add(new PositionState(new Position(position.X, position.Y-1), GCost + 1));

			return next;
		}

		public override bool Equals(Object? obj)
		{
			return obj is PositionState && this.position.Equals(((PositionState)obj).position);
		}

		public override int GetHashCode()
		{
			return this.position.GetHashCode();
		}

		public override string ToString()
		{
			return position.ToString();
		}

	}

}
