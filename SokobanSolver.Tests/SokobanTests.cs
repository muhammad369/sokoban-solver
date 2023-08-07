using SokobanSolver;
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
			s.SetBox(2, 3);
			s.SetBox(2, 2);
			s.SetPlayer(0, 4);
			s.SetBoxInTarget(1, 3);//1, 3
			s.SetWall(0, 0);
			s.SetWall(1, 0);

			var text = s.Description;

			var blocked = s.IsBlockedState();
			Assert.IsFalse(blocked);
		}

		[TestMethod]
		public void IsBlockedPosition()
		{
			var s = new SokobanState(4, 5, 0);
			s.SetTarget(0, 0);
			s.SetWall(0, 1);
			s.SetTarget(0, 2);
			s.SetWall(0, 3);
			s.SetBoxInTarget(0, 4);
			//
			s.SetBox(1, 2);
			s.SetBox(1, 4);
			//
			s.SetBox(2, 2);
			s.SetWall(2, 3);
			//
			s.SetPlayer(2, 0);

			var text = s.Description;

			Assert.IsFalse(new Position(1, 2).IsBlocked(s.board));
			Assert.IsTrue(new Position(0, 4).IsBlocked(s.board));
			Assert.IsTrue(new Position(1, 4).IsBlocked(s.board));
			Assert.IsFalse(new Position(2, 2).IsBlocked(s.board));
			Assert.IsFalse(new Position(2, 0).IsBlocked(s.board));
			Assert.IsTrue(new Position(3, 0).IsBlocked(s.board));
		}

		[TestMethod]
		public void IsBlockedState()
		{
			var s = new SokobanState(4, 5, 0);
			s.SetTarget(0, 0);
			s.SetWall(0, 1);
			s.SetTarget(0, 2);
			s.SetWall(0, 3);
			s.SetBoxInTarget(0, 4);
			//
			s.SetBox(1, 2);
			//
			s.SetBox(2, 2);
			s.SetWall(2, 3);
			//
			s.SetPlayer(2, 0);
			//

			var text = s.Description;
			var blocked = s.IsBlockedState();

			Assert.IsFalse(blocked);

			//
			s = s.Clone();
			s.SetBox(1, 4);

			text = s.Description;
			blocked = s.IsBlockedState();

			Assert.IsTrue(blocked);
		}

		[TestMethod]
		public void HCost()
		{
			var s = new SokobanState(4, 5, 0);
			s.SetTarget(0, 0);
			s.SetWall(0, 1);
			s.SetTarget(0, 2);
			s.SetWall(0, 3);
			s.SetBoxInTarget(0, 4);
			//
			s.SetBox(1, 2);
			//
			s.SetBox(2, 2);
			s.SetWall(2, 3);
			//
			s.SetPlayer(2, 0);
			//

			var text = s.Description;
			Assert.AreEqual(5, s.HCost);

		}

		[TestMethod]
		public void HCost1()
		{
			var s = new SokobanState(4, 6, 0);
			s.SetTarget(0, 0);
			s.SetTarget(0, 4);
			s.SetTarget(0, 5);
			s.SetTarget(0, 3);
			//
			s.SetBox(1, 2);
			s.SetBox(2, 1);
			s.SetBox(1, 1);
			s.SetBox(2, 2);
			//
			s.SetPlayer(2, 0);
			//

			var text = s.Description;
			Assert.AreEqual(int.MaxValue, s.HCost);

		}


		[TestMethod]
		public void HCost2()
		{
			var s = new SokobanState(4, 6, 0);
			s.SetTarget(0, 0);
			s.SetTarget(0, 4);
			s.SetTarget(0, 5);
			s.SetTarget(0, 3);
			//
			s.SetBox(1, 2);
			s.SetBox(2, 1);
			s.SetBox(1, 1);
			s.SetBox(3, 2);
			//
			s.SetPlayer(2, 0);
			//

			var text = s.Description;
			Assert.AreEqual(15, s.HCost);

		}


	}
}
