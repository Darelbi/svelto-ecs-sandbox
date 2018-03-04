using Game.ECS.Components.Lifter;
using Game.ECS.Components.Movement;
using Game.ECS.Controllers;
using Svelto.ECS;

namespace Game.ECS.Engines.Lifter
{
    /// <summary>
    /// A Lifter for the Lifter collection engine
    /// </summary>
    public class LifterMovementView: EntityView
    {
        public ILifter              lifter;
        public IMovement2D          movement;
    }

    /// <summary>
    /// A Liftable for the lifter collection engine
    /// </summary>
    public class LiftableMovementView: EntityView
    {
        public IMovement2D          movement;
    }

    /// <summary>
    /// This engine is responsible for transmitting movement of a lifter to all the carried things
    /// </summary>
    public class LifterMovementEngine : IQueryingEntityViewEngine, IMovementEngine
    {
        private readonly IMovementScheduler movement;

        public LifterMovementEngine( IMovementScheduler movement)
        {
            this.movement = movement;
        }

        public void Ready()
        {
            movement.RegisterMovementEngine( MovementPhase.LiftersMovement, this);
        }

        public void ExecuteMovementPhase()
        {
            TransmitMovement();
        }

        // Allow us to query for entities.
        public IEntityViewsDB entityViewsDB { private get; set; }

        public void TransmitMovement()
        {
            var list = entityViewsDB.QueryEntityViews< LifterMovementView>();
            foreach( var lifter in list)
            {
                TransmitMovement( lifter);
                
            }
        }

        public void TransmitMovement( LifterMovementView lifter)
        {
            LiftableMovementView liftableView = null;

            foreach (var liftableID in lifter.lifter.CarriedThings)
            {                
                if (entityViewsDB.TryQueryEntityView( liftableID, out liftableView))
                {
                    liftableView.movement.Movement += lifter.movement.Movement;
                    int entityID = liftableID;
                }
            }
        }
    }
}
