using sokoban_solver;

namespace SokobanSolver.Tests
{
	[TestClass]
	public class RouterTests
	{
		[TestMethod]
		public void RouteExists()
		{
			var state = new SokobanState(4, 4, 0);

			state.SetBlock(0, 0);
			state.SetTarget(3, 3);


			var router = new Router(state);

			var test = router.RouteExists(new Position(0, 0), new Position(3, 3));

			Assert.IsTrue(test);

			state.SetWall(0, 3);
			state.SetWall(1, 2);
			state.SetWall(2, 1);


			var test2 = router.RouteExists(new Position(0, 0), new Position(3, 3));

			Assert.IsTrue(test2);

			state.SetWall(3, 0);

			var test3 = router.RouteExists(new Position(0, 0), new Position(3, 3));

			Assert.IsFalse(test3);

		}


		[TestMethod]
		public void NearestWhere()
		{
			var state = new SokobanState(4, 4, 0);


			var router = new Router(state);

			var (pos1, dist1) = router.NearestWhere(new Position(0, 0), (pos, content)=> content == SokobanState.TARGET);

			Assert.IsTrue(pos1 == null && dist1 == -1);

			state.SetTarget(2, 2);
			state.SetTarget(1, 0);


			var (pos2, dist2) = router.NearestWhere(new Position(0, 0), (pos, content) => content == SokobanState.TARGET);

			Assert.IsTrue(pos2.Equals(new Position(1, 0)) && dist2 == 1);



			var (pos3, dist3) = router.NearestWhere(new Position(0, 0), (pos, content) => content == SokobanState.TARGET && !pos.Equals(new Position(1, 0)));

			Assert.IsTrue(pos3.Equals(new Position(2, 2)) && dist3 == 4);

		}

		
	}
}