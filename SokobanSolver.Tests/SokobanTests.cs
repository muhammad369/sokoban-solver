using sokoban_solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanSolver.Tests
{
	[TestClass]
	public class SokobanTests
	{


		[TestMethod]
		public void SokobanState_Description()
		{
			var s= new SokobanState(5,6,0);
			s.SetTarget(0, 1);
			s.SetTarget(1, 2);
			s.SetBlock(2, 3);
			s.SetBlock(2, 2);
			s.SetPlayer(0, 4);
			s.SetBlockInTarget(1, 3);//1, 3
			s.SetWall(0, 0);
			s.SetWall(1, 0);

			var text = s.Description;

			var blocked = s.isBlockedState();
			Assert.IsFalse(blocked);
		}

		[TestMethod]
		public void HCost()
		{
			var s = new SokobanState(5, 5, 0);
			s.SetTarget(0, 1);
			s.SetTarget(1, 2);
			s.SetBlock(2, 3);
			s.SetBlock(2, 2);
			s.SetPlayer(0, 4);

			var text = s.Description;
		}

	}
}
