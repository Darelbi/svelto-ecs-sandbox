using Game.ECS.Controllers;

namespace GameUnitTest.Controllers
{
    public class MockedMovementScheduler : IMovementScheduler
    {
        public void RegisterMovementEngine( MovementPhase phase, IMovementEngine engine)
        {
        }
    }
}

