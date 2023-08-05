using sokoban_solver;

namespace SokobanSolver.Tests
{
	[TestClass]
	public class RouterTests
	{
		[TestMethod]
		public void RouteExists()
		{
			var state = new SokobanState(4, 4);

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
	}
}