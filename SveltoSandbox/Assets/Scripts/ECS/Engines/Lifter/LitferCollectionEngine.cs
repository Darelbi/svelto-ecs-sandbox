using Game.ECS.Components;
using Game.ECS.Components.Liftable;
using Game.ECS.Components.Lifter;
using Svelto.ECS;

namespace Game.ECS.Engines.Lifter
{
    /// <summary>
    /// A Lifter for the Lifter collection engine
    /// </summary>
    public class LifterParentView: EntityView
    {
        public ILifter              lifter;
        public ILifterLandingEvent  lifterLandingEvents;
    }

    /// <summary>
    /// A Liftable for the lifter collection engine
    /// </summary>
    public class LifterChildView: EntityView
    {
        public ILiftable            liftable;
        public IIdentifier          identifer;
    }

    /// <summary>
    /// This engine is responsible for keeping parented all Liftable entities inside all Lifter entities
    /// When the hierarchy is correctly maintained all movements will be parented without need for 
    /// GameObjects to be really parents of each other. For simplicity we allow only a 2 level hierarchy,
    /// so we have either a lifter or unparanted liftable, or a lifter with some liftables inside.
    /// </summary>
    public class LitferCollectionEngine : MultiEntityViewsEngine< LifterParentView, LifterChildView>
    {
        // Allow us to query for entities.
        public IEntityViewsDB entityViewsDB { private get; set; }

        /// <summary>
        /// Register events for when a lifter is added to engines.
        /// </summary>
        /// <param name="entityView"></param>
        protected override void Add( LifterParentView entityView)
        {
            entityView.lifterLandingEvents.LandEvent += LandEvent;
            entityView.lifterLandingEvents.LeaveEvent += LeaveEvent;
        }

        /// <summary>
        /// Deregister events for when a lifter is removed from engines
        /// </summary>
        /// <param name="entityView"></param>
        protected override void Remove( LifterParentView entityView)
        {
            entityView.lifterLandingEvents.LandEvent -= LandEvent;
            entityView.lifterLandingEvents.LeaveEvent -= LeaveEvent;

            foreach( var child in entityView.lifter.CarriedThings)
            {
                LifterChildView liftableVew = null;

                entityViewsDB.TryQueryEntityView( child, out liftableVew);
                liftableVew.liftable.Carrier = -1;
            }
        }

        /// <summary>
        /// Events to notify to game something landed on a lifter
        /// </summary>
        /// <param name="lifterID"></param>
        /// <param name="liftableID"></param>
        public void LandEvent( int lifterID, int liftableID)
        {
            LifterParentView lifterView = entityViewsDB.QueryEntityView< LifterParentView>( lifterID);
            LifterChildView liftableVew = null;

            if(entityViewsDB.TryQueryEntityView( liftableID, out liftableVew))
                CreateParentRelationship( lifterView.lifter, liftableVew.liftable, lifterID, liftableID);
        }

        public void CreateParentRelationship( ILifter lifter, ILiftable liftable, int lifterID, int liftableID)
        {
            liftable.Carrier = lifterID;
            lifter.CarriedThings.Add( liftableID);
        }

        /// <summary>
        /// Events to notify to game something left the lifter
        /// </summary>
        /// <param name="lifterID"></param>
        /// <param name="liftableID"></param>
        public void LeaveEvent( int lifterID, int liftableID)
        {
            LifterParentView lifterView = entityViewsDB.QueryEntityView< LifterParentView>( lifterID);
            LifterChildView liftableView = null;

            if(entityViewsDB.TryQueryEntityView( liftableID, out liftableView))
                RemoveParentRelationship( lifterView.lifter, liftableView.liftable, liftableID);
        }

        public void RemoveParentRelationship( ILifter lifter, ILiftable liftable, int liftableID)
        {
            liftable.Carrier = -1;
            lifter.CarriedThings.Remove( liftableID);
        }

        protected override void Add( LifterChildView entityView)
        {
            entityView.liftable.Carrier = -1;
        }

        /// <summary>
        /// When a lifted object leaves the game we make sure to remove it also from the lifter
        /// </summary>
        /// <param name="liftableView"></param>
        protected override void Remove( LifterChildView liftableView)
        {
            LifterParentView lifterView = null;

            if (entityViewsDB.TryQueryEntityView( liftableView.liftable.Carrier, out lifterView))
            {
                liftableView.liftable.Carrier = -1;
                lifterView.lifter.CarriedThings.Remove( liftableView.identifer.Id);
            }
        }
    }
}
