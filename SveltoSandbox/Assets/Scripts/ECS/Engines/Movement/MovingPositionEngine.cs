using Game.ECS.Components.Movement;
using Game.ECS.Controllers;
using Svelto.ECS;
using UnityEngine;

/// <summary>
/// Lifter components have to do with the movement of all the stuff that can carry liftable objects
/// (ships, space stations)
/// </summary>
namespace Game.ECS.Engines.Movement
{
    /// <summary>
    /// A Lifter for the Lifter collection engine
    /// </summary>
    public class IMovingPositionView : EntityView
    {
        public IMovement2D movement;
        public IPosition2D position;
    }

    /// <summary>
    /// This engine is responsible for transmitting movement of a lifter to all the carried things
    /// </summary>
    public class MovingPositionEngine : IQueryingEntityViewEngine, IMovementEngine
    {
        private readonly IMovementScheduler movement;

        public MovingPositionEngine( IMovementScheduler movement)
        {
            this.movement = movement;
        }

        public void Ready()
        {
            movement.RegisterMovementEngine( MovementPhase.UpdatePosition, this);
        }

        public void ExecuteMovementPhase()
        {
            ApplyMovement();
        }

        // Allow us to query for entities.
        public IEntityViewsDB entityViewsDB { private get; set; }

        public readonly Vector2 Zero = Vector2.zero;

        public void ApplyMovement()
        {
            var list = entityViewsDB.QueryEntityViews< IMovingPositionView>();

            foreach(var movingPosition in list)
            {
                ApplyMovement( movingPosition);
            }
        }

        private void ApplyMovement( IMovingPositionView movingPosition)
        {
            movingPosition.position.Position += movingPosition.movement.Movement;
            movingPosition.movement.Movement = Zero;
        }
    }
}