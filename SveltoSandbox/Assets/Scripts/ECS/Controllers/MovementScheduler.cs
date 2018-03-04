using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.ECS.Controllers
{
    public enum MovementPhase
    {
        InputGather,
        LiftersMovement,
        UpdatePosition
    }

    public interface IMovementEngine
    {
        void ExecuteMovementPhase();
    }

    public interface IMovementScheduler
    {
        void RegisterMovementEngine( MovementPhase phase, IMovementEngine engine);
    }

    /// <summary>
    /// This class is responsible for inverting control over movement engines execution,
    /// Each engine register itself for a specific movement phase. Actually this is similiar
    /// to a sequencer but simpler. There is no communication involved, just strict execution order
    /// </summary>
    public class MovementScheduler : IMovementScheduler
    {
        Dictionary< MovementPhase, IMovementEngine> engines = new Dictionary< MovementPhase, IMovementEngine>();
        IMovementEngine[] enginesFasterAccess;

        public void RegisterMovementEngine( MovementPhase phase, IMovementEngine engine)
        {
            if (engines.ContainsKey( phase))
                throw new InvalidOperationException( "please add a new MovementPhase in the enum and use it.");

            engines[ phase] = engine;
            enginesFasterAccess = engines.OrderBy( x => x.Key).Select( x => x.Value).ToArray();
        }

        public void ScheduleMovement()
        {
            foreach( var engine in enginesFasterAccess)
            {
                engine.ExecuteMovementPhase();
            }
        }
    }
}