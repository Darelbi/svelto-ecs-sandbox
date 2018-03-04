using Game.ECS.Components;
using Game.ECS.Components.Liftable;
using Game.ECS.Components.Lifter;
using Game.ECS.Engines.Lifter;
using Svelto.DataStructures;
using Svelto.ECS;
using UnityEngine;

namespace GameUnitTest.Lifter
{
    public class TestLifterCollection_LandEvent : ECSUnitTest
    {
        public override void SetupEngines( EnginesRoot enginesRoot, IEntityFactory factory, IEntityFunctions functions)
        {
            lifterCollectionEngine = new LifterCollectionEngine();
            enginesRoot.AddEngine( lifterCollectionEngine);
        }

        public class LifterParentStub : ILifter
        {
            public FasterList< int> CarriedThings { get; set; } = new FasterList< int>();
        }

        public class LiftableChildStub : ILiftable, IIdentifier
        {
            public int Carrier { get; set; }

            public int Id { get; set; }
        }

        public class LifterStubber : GenericEntityDescriptor< LifterParentView>
        {

        }

        public class LifterChildStubber : GenericEntityDescriptor< LifterChildView>
        {

        }

        LifterCollectionEngine lifterCollectionEngine;
        LifterParentStub stubParent;
        LiftableChildStub stubChild;

        public override void SetupEntities( IEntityFactory factory, IEntityFunctions functions)
        {
            stubParent = new LifterParentStub
            {

            };

            stubChild = new LiftableChildStub
            {
                Id = 20
            };

            factory.BuildEntity< LifterStubber>( 10, new object[] { stubParent });
            factory.BuildEntity< LifterChildStubber>( 20, new object[] { stubChild });
        }

        public override void CheckPreconditions()
        {

        }

        public override void DoStuff()
        {
            LifterLandingEvent landEvent = new LifterLandingEvent
            {
                LifterID = 10,
                LiftableID = 20,
                Landed = true
            };

            lifterCollectionEngine.Step( ref landEvent, 0);
        }

        public override void CheckPostconditions()
        {
            bool thingsCountOk = stubParent.CarriedThings.Count == 1;
            bool lifterChildOk = stubParent.CarriedThings.Contains( 20);
            bool liftableCarrierOk = stubChild.Carrier == 10;

            if (thingsCountOk && lifterChildOk && liftableCarrierOk)
                Debug.Log( "LifterCollection_Landing - Success");
            else
                Debug.LogWarning( "LifterCollection_Landing - Failure");
        }
    }
}