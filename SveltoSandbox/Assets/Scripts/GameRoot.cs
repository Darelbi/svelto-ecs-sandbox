using Kore.Coroutines;
using Svelto.ECS;
using Svelto.ECS.Schedulers;
using System.Collections;
using UnityEngine;

/// <summary>
/// Because using namespaces in a nice way is good
/// </summary>
namespace Game
{
    /// <summary>
    /// The GameRoot creates the game context and run the game task.
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        private readonly GameContext game;

        /// <summary>
        /// Create the game context.
        /// </summary>
        public GameRoot()
        {
            game = new GameContext();
        }

        /// <summary>
        /// Start the game task.
        /// </summary>
        public void Start()
        {
            Koroutine.Run( game.Task(), Method.Update);
        }
    }

    /// <summary>
    /// Setup the ECS framework and keep alive it
    /// </summary>
    public class GameContext
    {
        /// <summary>
        /// Game Task, the IEnumerator keeps the variables alive thanks to the Koroutine manager.
        /// </summary>
        /// <returns></returns>
        public IEnumerator Task()
        {
            var enginesRoot = new EnginesRoot( GenerateMySubmissionScheduler());

            SetupEngines( enginesRoot);

            while (true)
                yield return null;
        }

        /// <summary>
        /// Submission scheduler, it has to be game-specific, here's mine copied from Svelto.
        /// </summary>
        private EntitySubmissionScheduler GenerateMySubmissionScheduler()
        {
            return new GameSubmissionScheduler(
                (ISubmissionScheduler) StaticSubmissionScheduler.Instance );
        }


        /// <summary>
        /// Wire all the game logic and setup the engines
        /// </summary>
        private void SetupEngines( EnginesRoot ecs)
        {
            IEntityFactory entityFactory = ecs.GenerateEntityFactory();
            IEntityFunctions entityFunctions = ecs.GenerateEntityFunctions();

            //ecs.AddEngine( new Engine(blabla));
        }
    }
}
