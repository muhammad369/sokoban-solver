using SokobanSolver;
using Solver.AStar;
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
		public void SokobanState_Move()
		{
			var s = new SokobanState(4, 6, 0);
			
			s.SetTarget(2, 3);
			//
			s.SetBoxInTarget(1, 1);
			s.SetBoxInTarget(1, 2);
			s.SetBox(1, 3);
			//
			s.SetWall(1, 4);
			//
			s.SetPlayer(2, 0);
			//

			var text = s.Description;
			Assert.AreEqual(1, s.targetsNotYetFilled);

			//
			var newS = s.FormState(new Position(1, 1), new Position(1, 0));

			Assert.AreEqual(newS.boxes.Count, 3);
			Assert.IsTrue(newS.boxes.Contains(new Position(1, 0)));
			Assert.AreEqual(2, newS.targetsNotYetFilled);

		}

		[TestMethod]
		public void SokobanState_Move_1()
		{
			var s = new SokobanState(4, 6, 0);
			s.SetTarget(0, 5);
			s.SetTarget(1, 5);
			s.SetTarget(2, 5);
			//
			
			s.SetBox(0, 4);
			s.SetBox(1, 4);
			s.SetBox(2, 4);
			//
			s.SetPlayer(2, 0);
			//

			var text = s.Description;

			var newS = s.FormState(new Position(0, 4), new Position(0, 5));

			Assert.AreEqual(newS.boxes.Count, 3);
			Assert.IsTrue(newS.boxes.Contains(new Position(0, 5)));
			Assert.AreEqual(2, newS.targetsNotYetFilled);

			var newS2 = newS.FormState(new Position(1, 4), new Position(1, 5));

			Assert.AreEqual(newS2.boxes.Count, 3);
			Assert.IsTrue(newS2.boxes.Contains(new Position(1, 5)));
			Assert.AreEqual(1, newS2.targetsNotYetFilled);


			//return back
			var newS3 = newS2.FormState(new Position(1, 5), new Position(1, 4));

			Assert.AreEqual(newS3.boxes.Count, 3);
			Assert.IsTrue(newS3.boxes.Contains(new Position(1, 4)));
			Assert.AreEqual(2, newS3.targetsNotYetFilled);

			// move from target to target
			var newS4 = newS3.FormState(new Position(0, 5), new Position(1, 5));

			Assert.AreEqual(newS4.boxes.Count, 3);
			Assert.IsTrue(newS4.boxes.Contains(new Position(1, 5)));
			Assert.AreEqual(2, newS4.targetsNotYetFilled);

		}

		[TestMethod]
		public void SokobanState_GetHashCode()
		{
			var s1 = new SokobanState(5, 6, 0);
			s1.SetTarget(0, 1);
			s1.SetTarget(1, 2);
			s1.SetBox(2, 3);
			s1.SetBox(2, 2);
			s1.SetPlayer(0, 4);
			s1.SetBoxInTarget(1, 3);

			var s2 = new SokobanState(5, 6, 0);
			s2.SetTarget(0, 1);
			s2.SetTarget(1, 2);
			s2.SetBox(2, 3);
			s2.SetBox(2, 2);
			s2.SetPlayer(4, 3);
			s2.SetBox(1, 3);

			Assert.AreEqual(s1.GetHashCode(), s2.GetHashCode());
			Assert.IsTrue(s1.Equals(s2));
			Assert.IsFalse(s1 == s2);

			var s3 = new SokobanState(5, 6, 0);
			s3.SetTarget(0, 1);
			s3.SetTarget(1, 2);
			s3.SetBox(2, 4);
			s3.SetBox(2, 2);
			s3.SetPlayer(4, 3);
			s3.SetBox(1, 3);


			Assert.AreNotEqual(s1.GetHashCode(), s3.GetHashCode());
			Assert.IsFalse(s1.Equals(s3));
			Assert.IsFalse(s1 == s3);
			//
			Assert.AreNotEqual(s2.GetHashCode(), s3.GetHashCode());
			Assert.IsFalse(s2.Equals(s3));
			Assert.IsFalse(s2 == s3);
		}


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
		public void IsBlockedState_2()
		{
			var s = new SokobanState(4, 5, 0);

			s.SetBoxInTarget(1, 2);
			s.SetBox(1, 1);
			s.SetWall(1, 3);
			s.SetTarget(0, 0);
			//

			var text = s.Description;
			var blocked = s.IsBlockedState();

			Assert.IsFalse(blocked);

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


		[TestMethod]
		public void SolutionTest()
		{
			var s = new SokobanState(4, 6, 0);
			s.SetTarget(0, 5);
			s.SetTarget(1, 5);
			s.SetTarget(2, 5);
			//
			s.SetBox(1, 1);
			s.SetBox(1, 2);
			s.SetBox(1, 3);
			//
			s.SetWall(1, 4);
			//
			s.SetPlayer(2, 0);
			//

			var text = s.Description;
			Assert.AreEqual(13, s.HCost);


			var solver = new Searcher();

			var solution = solver.getSolution(s);

			Assert.IsNotNull(solution);

		}

		[TestMethod]
		public void SolutionTest_1()
		{
			var s = new SokobanState(4, 6, 0);
			s.SetBoxInTarget(0, 5);
			s.SetTarget(1, 5);
			s.SetBoxInTarget(2, 5);
			//
			//s.SetBox(1, 1);
			//s.SetBox(1, 2);
			s.SetBox(1, 3);
			//
			s.SetWall(1, 4);
			//
			s.SetPlayer(2, 0);
			//


			var text = s.Description;
			Assert.AreEqual(4, s.HCost);


			var solver = new Searcher();

			var solution = solver.getSolution(s);

			Assert.IsNotNull(solution);

		}

		[TestMethod]
		public void SolutionTest_2()
		{
			var s = new SokobanState(3, 6, 0);
			s.SetTarget(0, 5);
			s.SetTarget(1, 5);
			s.SetTarget(2, 5);
			//
			s.SetBox(1, 1);
			s.SetBox(1, 2);
			s.SetBox(1, 3);
			//
			s.SetWall(1, 4);
			//
			s.SetPlayer(2, 0);
			//

			var text = s.Description;
			Assert.AreEqual(13, s.HCost);


			var solver = new Searcher();

			var solution = solver.getSolution(s);

			Assert.IsNull(solution);

		}


	}
}
