using Game.ECS.Components.Liftable;
using Game.ECS.Components.Lifter;
using Game.ECS.Components.Movement;
using Game.ECS.Controllers;
using Svelto.ECS;
using UnityEngine;

namespace Game.ECS.Engines.Lifter
{
    public class AnyLifterSnapView: EntityView
    {
        public IPosition2D position;
    }

    public class AnyLiftableSnapView: EntityView
    {
        public IPosition2D position;
        public ILiftable   liftable;
    }

    /// <summary>
    /// This engine is responsible to invert snap/desnap all liftable objects to
    /// to one lifter, which lifter is undefined. I take just the last of the array
    /// </summary>
    public class AnyLifterSnapEngine : IQueryingEntityViewEngine, IMovementEngine
    {
        private readonly IMovementScheduler movement;
        private readonly ISequencer landSequence;

        public AnyLifterSnapEngine( IMovementScheduler movement, ISequencer landingEventResponseSequence)
        {
            this.movement = movement;
            this.landSequence = landingEventResponseSequence;
        }

        public IEntityViewsDB entityViewsDB { private get; set; }

        public void Ready()
        {
            movement.RegisterMovementEngine( MovementPhase.InputGather, this);
        }

        public void ExecuteMovementPhase()
        {
            CheckInput();
        }

        public void CheckInput()
        {
            if (Input.anyKeyDown)
                InvertSnap();
        }

        /// <summary>
        /// If a liftable is not snapped to a engine it is snapped to it, and viceversa
        /// </summary>
        public void InvertSnap()
        {
            var lifters = entityViewsDB.QueryIndexableEntityViews< AnyLifterSnapView>();
            var liftables = entityViewsDB.QueryIndexableEntityViews< AnyLiftableSnapView>();

            // This is very inefficient. It is just to setup quickly a demo.
            foreach (var liftable in liftables)
            {
                int liftableID = liftable.Key;
                var liftableView = liftable.Value;
                var lifterPos = liftableView.position.Position;

                float minDistance = float.MaxValue;
                int nearestLifterID = -1;

                foreach (var lifter in lifters)
                {
                    int lifterID = lifter.Key;
                    var lifterView = lifter.Value;
                    var position = lifterView.position.Position;

                    float tempDistance = (lifterPos - position).SqrMagnitude();

                    // find nearest lifter
                    if(tempDistance < minDistance)
                    {
                        minDistance = tempDistance;
                        nearestLifterID = lifterID;
                    }
                }

                bool isCurrentlyLanded = liftableView.liftable.Carrier != -1;

                var landingEvent = new LifterLandingEvent
                {
                    LifterID = nearestLifterID,
                    LiftableID = liftableID,
                    Landed = !isCurrentlyLanded // invert the landing state
                };

                landSequence.Next( this, ref landingEvent);
            }
        }
    }
}
