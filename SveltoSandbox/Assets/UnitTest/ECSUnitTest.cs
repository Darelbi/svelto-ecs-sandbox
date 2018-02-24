using Game;
using Svelto.ECS;
using Svelto.ECS.Schedulers;
using UnityEngine;

namespace GameUnitTest
{
    /// <summary>
    /// If later we want a single test runner we can search for all behaviours with this interface
    /// </summary>
    public interface IRunnableTest
    {
        void RunTest();
    }

    public abstract class ECSUnitTest : MonoBehaviour, IRunnableTest
    {
        /// <summary>
        /// Start the unit test.
        /// </summary>
        public void Start()
        {
            RunTest();
        }

        private EntitySubmissionScheduler GenerateMySubmissionScheduler()
        {
            // Here we use a different submission scheduler to make testing easy
            return new GameSubmissionScheduler(
                scheduler = 
                new InstantSubmissionScheduler());
        }

        ISubmissionScheduler scheduler;

        public void RunTest()
        {
            var ecs = new EnginesRoot( GenerateMySubmissionScheduler());

            IEntityFactory entityFactory = ecs.GenerateEntityFactory();
            IEntityFunctions entityFunctions = ecs.GenerateEntityFunctions();

            SetupEngines( ecs, entityFactory, entityFunctions);

            SetupEntities( entityFactory, entityFunctions);

            Tick();
            DoStuff();

            Tick();
            CheckPostconditions();
        }

        public void Tick()
        {
            //as you see, you don't even need a game loop for testing stuff (at least for
            // testing stuff that is totally independent of unity3D)
            scheduler.SubmitNow();
        }

        public abstract void SetupEngines( EnginesRoot enginesRoot, IEntityFactory factory, IEntityFunctions functions);

        public abstract void SetupEntities( IEntityFactory factory, IEntityFunctions functions);

        public abstract void DoStuff();

        public abstract void CheckPostconditions();
    }
}
