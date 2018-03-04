using Game;
using Svelto.ECS;
using Svelto.ECS.Schedulers;
using UnityEngine;
using System.Collections;
using Kore.Coroutines;

namespace GameUnitTest.SystemTest
{
    public abstract class ECSSystemTest : MonoBehaviour, IRunnableTest
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
            return new GameSubmissionScheduler(
                (ISubmissionScheduler) StaticSubmissionScheduler.Instance);
        }

        ISubmissionScheduler scheduler;

        public void RunTest()
        {
            var ecs = new EnginesRoot( GenerateMySubmissionScheduler());

            IEntityFactory entityFactory = ecs.GenerateEntityFactory();
            IEntityFunctions entityFunctions = ecs.GenerateEntityFunctions();

            SetupEngines( ecs, entityFactory, entityFunctions);

            SetupEntities( entityFactory, entityFunctions);

            StartCoroutine( UpdateTest());
        }

        public IEnumerator UpdateTest()
        {
            while (true)
            {
                yield return null;
                Update();
            }
        }

        public abstract void Update();

        public abstract void SetupEngines( EnginesRoot enginesRoot, IEntityFactory factory, IEntityFunctions functions);

        public abstract void SetupEntities( IEntityFactory factory, IEntityFunctions functions);
    }
}