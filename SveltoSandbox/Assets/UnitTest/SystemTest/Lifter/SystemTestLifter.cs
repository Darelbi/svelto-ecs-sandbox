using Game.ECS.Components.Lifter;
using Game.ECS.Controllers;
using Game.ECS.Engines.Lifter;
using Game.ECS.Engines.Movement;
using Svelto.ECS;
using UnityEngine;

namespace GameUnitTest.SystemTest.Lifter
{
    public class SystemTestLifter : ECSSystemTest
    {
        bool Snap = true;
        bool OldSnap = true;

        MovementScheduler movementPhaser;
        LifterCollectionEngine lifterCollectionEngine;
        LifterMovementEngine lifterMovementEngine;
        MovingPositionEngine movingPositionEngine;
        SystemTestLiftableImplementor liftableStub;
        SystemTestLifterImplementor lifterStub;

        public override void Update()
        {
            //seems actually I found a bug in unity. GetMousebuttonDown returns true for 2 consecutive frames.
            //weird
            if (Input.GetMouseButtonDown(0))
                Snap = true;

            if (Input.GetMouseButtonDown(1))
                Snap = false;

            if(Snap!=OldSnap)
            {
                OldSnap = Snap;
                LifterLandingEvent landEvent = new LifterLandingEvent
                {
                    LifterID = 1,
                    LiftableID = 2,
                    Landed = Snap
                };

                // Simulate the Sequencer by just calling "Step"
                lifterCollectionEngine.Step( ref landEvent, 0);
            }

            movementPhaser.ScheduleMovement();
        }

        public override void SetupEngines( EnginesRoot enginesRoot, IEntityFactory factory, IEntityFunctions functions)
        {
            movementPhaser = new MovementScheduler();

            lifterCollectionEngine = new LifterCollectionEngine();
            lifterMovementEngine = new LifterMovementEngine( movementPhaser);
            movingPositionEngine = new MovingPositionEngine( movementPhaser);

            enginesRoot.AddEngine( lifterCollectionEngine);
            enginesRoot.AddEngine( lifterMovementEngine);
            enginesRoot.AddEngine( movingPositionEngine);
        }

        // Now we put all views togheter, basically we are telling which engines to use
        public class LifterStubber: GenericEntityDescriptor
            < LifterParentView, LifterMovementView, IMovingPositionView>
        {

        }

        // Now we put all views togheter, basically we are telling which engines to use
        public class LiftableStubber: GenericEntityDescriptor
            <LifterChildView, LiftableMovementView, IMovingPositionView>
        {

        }

        public override void SetupEntities( IEntityFactory factory, IEntityFunctions functions)
        {
            // another way to create entities it to search for existing Game Objects,
            // as you see it is perfectly valid for Implementors to be MonoBehaviours 
            // attached to some gameobject

            liftableStub = FindObjectOfType< SystemTestLiftableImplementor>();
            lifterStub = FindObjectOfType< SystemTestLifterImplementor>();

            // we start the game with the liftable parented to the lifter

            liftableStub.Id = 2;
            liftableStub.Carrier = 1;
            lifterStub.CarriedThings.Add( 2);

            factory.BuildEntity< LifterStubber>( 1, new object[] { lifterStub });
            factory.BuildEntity< LiftableStubber>( 2, new object[] { liftableStub });
        }
    }
}
