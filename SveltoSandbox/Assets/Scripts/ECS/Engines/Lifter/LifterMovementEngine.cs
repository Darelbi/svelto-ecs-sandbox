using Game.ECS.Components.Lifter;
using Game.ECS.Components.Movement;
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
    public class LitferMovementEngine : IQueryingEntityViewEngine
    {
        // Allow us to query for entities.
        public IEntityViewsDB entityViewsDB { private get; set; }

        public void Ready()
        {
            
        }

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
            foreach (var liftableID in lifter.lifter.CarriedThings)
            {
                LiftableMovementView liftableView = null;
                if (entityViewsDB.TryQueryEntityView( liftableID, out liftableView))
                {
                    liftableView.movement.Movement += lifter.movement.Movement;
                }
            }
        }
    }
}
