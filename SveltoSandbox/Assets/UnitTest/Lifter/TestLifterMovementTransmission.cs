using Game.ECS.Components.Lifter;
using Game.ECS.Components.Movement;
using Game.ECS.Engines.Lifter;
using GameUnitTest.Controllers;
using Svelto.DataStructures;
using Svelto.ECS;
using UnityEngine;

namespace GameUnitTest.Lifter
{
    public class TestLifterMovementTransmission : ECSUnitTest
    {
        public override void SetupEngines( EnginesRoot enginesRoot, IEntityFactory factory, IEntityFunctions functions)
        {
            // add the lifter engine, it uses a fake MovementScheduler (mocked)
            movementEngine = new LifterMovementEngine( new MockedMovementScheduler());
            enginesRoot.AddEngine( movementEngine);
        }

        public class LifterMovementStub : IMovement2D, ILifter
        {
            public FasterList< int> CarriedThings { get; set; } = new FasterList< int>();

            public Vector2 Movement { get; set; }
        }

        public class LiftableMovementStub : IMovement2D
        {
            public Vector2 Movement { get; set; }
        }

        public class LifterStubber : GenericEntityDescriptor< LifterMovementView>
        {

        }

        public class LifterChildStubber : GenericEntityDescriptor< LiftableMovementView>
        {

        }

        LifterMovementEngine movementEngine = null;
        LifterMovementStub stubParent;
        LiftableMovementStub stubCarriedChild;
        LiftableMovementStub stubAloneChild;

        public override void SetupEntities( IEntityFactory factory, IEntityFunctions functions)
        {
            stubParent = new LifterMovementStub
            {
                Movement = new Vector2( 2, 2)
            };

            stubParent.CarriedThings.Add( 20);

            stubCarriedChild = new LiftableMovementStub
            {
                Movement = new Vector2( 3, 3)
            };

            stubAloneChild = new LiftableMovementStub
            {
                Movement = new Vector2( 3, 3)
            };

            factory.BuildEntity< LifterStubber>( 10, new object[] { stubParent });
            factory.BuildEntity< LifterChildStubber>( 20, new object[] { stubCarriedChild });
            factory.BuildEntity< LifterChildStubber>( 30, new object[] { stubAloneChild });
        }

        public override void CheckPreconditions()
        {

        }

        public override void DoStuff()
        {
            movementEngine.TransmitMovement();
        }

        public override void CheckPostconditions()
        {
            Vector2 lifterMovement = new Vector2( 2, 2);
            Vector2 transmittedMovement = new Vector2( 5, 5);
            Vector2 nonTransmittedMovement = new Vector2( 3, 3);

            bool lifterMovementNotChanged = stubParent.Movement == lifterMovement;
            bool childMovementChanged = stubCarriedChild.Movement == transmittedMovement;
            bool aloneMovementNotChanged = stubAloneChild.Movement == nonTransmittedMovement;

            if (lifterMovementNotChanged && childMovementChanged && aloneMovementNotChanged)
                Debug.Log(" LifterMovementEngine - Success");
            else
                Debug.Log(" LifterMovementEngine - Failure");
        }
    }
}