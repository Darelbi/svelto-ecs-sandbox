using Game.ECS.Components.Liftable;
using Game.ECS.Components.Lifter;
using Svelto.ECS;

namespace Game.ECS.Engines.Lifter
{
    /// <summary>
    /// A Lifter for the Lifter collection engine
    /// </summary>
    public class LifterCollectionView: EntityView
    {
        public ILifter              influencedLifter;
        public ILifterLandingEvent  lifterLandingEvents;
    }

    /// <summary>
    /// A Liftable for the lifter collection engine
    /// </summary>
    public class LiftableCollectionView: EntityView
    {
        public ILiftable            liftable;
    }

    /// <summary>
    /// This engine is responsible for keeping parented all Liftable entities inside all Lifter entities
    /// When the hierarchy is correctly maintained all movements will be parented without needed for 
    /// GameObjects to be really parents of each other.
    /// </summary>
    public class LitferCollectionEngine : MultiEntityViewsEngine< LifterCollectionView, LiftableCollectionView>
    {
        private IEntityFunctions entityFunctions;

        public LitferCollectionEngine( IEntityFunctions entityFunctions)
        {
            this.entityFunctions = entityFunctions;
        }

        protected override void Add( LiftableCollectionView entityView)
        {
            throw new System.NotImplementedException();
        }

        protected override void Add( LifterCollectionView entityView)
        {
            throw new System.NotImplementedException();
        }

        protected override void Remove( LiftableCollectionView entityView)
        {
            throw new System.NotImplementedException();
        }

        protected override void Remove( LifterCollectionView entityView)
        {
            throw new System.NotImplementedException();
        }
    }
}
