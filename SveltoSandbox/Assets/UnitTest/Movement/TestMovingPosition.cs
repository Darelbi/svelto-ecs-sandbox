using Game.ECS.Components.Movement;
using Game.ECS.Engines.Movement;
using GameUnitTest.Controllers;
using Svelto.ECS;
using UnityEngine;

namespace GameUnitTest.Movement
{
    public class MovingPositionStub: IMovement2D, IPosition2D
    {
        public Vector2 Position { get; set; }

        public Vector2 Movement { get; set; }
    }

    public class MovingPositionStubber : GenericEntityDescriptor< IMovingPositionView>
    {

    }

    public class TestMovingPosition : ECSUnitTest
    {

        public override void SetupEngines( EnginesRoot enginesRoot, IEntityFactory factory, IEntityFunctions functions)
        {
            engine = new MovingPositionEngine( new MockedMovementScheduler());
            enginesRoot.AddEngine( engine);
        }

        // We keep a reference to implementors and engines because we want to manually do some stuff
        // for seeing the results
        MovingPositionEngine engine;
        MovingPositionStub stub;

        public override void SetupEntities( IEntityFactory factory, IEntityFunctions functions)
        {
            //create the "stub implementor" (note this could actually be used already in gameplay,
            // but never mix testing stuff inside gamemplay)

            stub = new MovingPositionStub
            {
                Position = new Vector2( 2, 2),
                Movement = new Vector2( 3, 3)
            };

            factory.BuildEntity< MovingPositionStubber>( 1, new object[] { stub });
        }

        public override void CheckPreconditions()
        {

        }

        public override void DoStuff()
        {
            engine.ApplyMovement();
        }

        public override void CheckPostconditions()
        {
            Vector2 expectedMovement = Vector2.zero;
            Vector2 expectedPosition = new Vector2( 5, 5);

            if (stub.Movement == expectedMovement && stub.Position == expectedPosition)
                Debug.Log( "MovingPositionEngine - Success");
            else
                Debug.LogWarning( "MovingPositionEngine - Failure");
        }
    }
}

