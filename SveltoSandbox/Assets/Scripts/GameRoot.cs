using Game.ECS.Engines.Lifter;
using Kore.Coroutines;
using Svelto.ECS;
using System.Collections;
using UnityEngine;

/// <summary>
/// Game namespace, for reference on the composition root see the vanilla example:
/// https://github.com/sebas77/Svelto.ECS.Examples.Vanilla/blob/master/Svelto.ECS.Example/src/Svelto-ECS-Simplest-Example-Ever/Example/MainContextSimple.cs
/// </summary>
namespace Game
{
    /// <summary>
    /// We just need on of theese in scene to start the game
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        private readonly GameContext game;

        /// <summary>
        /// Create the game context
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
    /// Class holding the game logic, not static to allow using some variables inside
    /// </summary>
    public class GameContext
    {
        /// <summary>
        /// The task to be runned
        /// </summary>
        /// <returns></returns>
        public IEnumerator Task()
        {
            SetupGame();

            while (true)
                yield return null;
        }

        /// <summary>
        /// Wire all the game logic and setup the engines
        /// </summary>
        private void SetupGame()
        {
            var scheduler = new GameSubmissionScheduler();
            var root = new EnginesRoot( scheduler);

            IEntityFactory entityFactory = root.GenerateEntityFactory();
            IEntityFunctions entityFunctions = root.GenerateEntityFunctions();

            root.AddEngine( new LitferCollectionEngine( entityFunctions));
        }
    }
}
