using Game.ECS.Controllers;
using Game.ECS.Engines.Lifter;
using Game.ECS.Engines.Movement;
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

            return SetupEngines( enginesRoot);
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
        private IEnumerator SetupEngines( EnginesRoot ecs)
        {
            IEntityFactory entityFactory = ecs.GenerateEntityFactory();
            IEntityFunctions entityFunctions = ecs.GenerateEntityFunctions();

            MovementScheduler movementPhaser = new MovementScheduler();
            Sequencer landingEventResponseSequence = new Sequencer();

            var lifterCollection = new LifterCollectionEngine();
            var lifterMovement = new LifterMovementEngine( movementPhaser);
            var movingPosition = new MovingPositionEngine( movementPhaser);
            var lifterSnap = new AnyLifterSnapEngine( movementPhaser, landingEventResponseSequence);

            ecs.AddEngine( lifterCollection);
            ecs.AddEngine( lifterMovement);
            ecs.AddEngine( movingPosition);            

            // a sequence is just a set of "labels" each label can be triggered by the owning engine
            // and cause the listed engines to receive the messages.
            landingEventResponseSequence.SetSequence
            (
                new Steps
                {
                    {// LABEL
                        // owning engine
                        lifterSnap, // when lifterSnap calls "next" the message is delivered "To"...

                        //listed engines
                        new To
                        {
                            lifterCollection //.. lifterCollection
                        }
                    }
                }
            );

            while( true)
            {
                movementPhaser.ScheduleMovement();
                yield return null;
            }
        }
    }
}
