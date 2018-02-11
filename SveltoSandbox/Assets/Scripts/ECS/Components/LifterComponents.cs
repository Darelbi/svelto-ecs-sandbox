using Game.ECS.Components.Liftable;
using Svelto.DataStructures;

/// <summary>
/// Lifter components have to do with the movement of all the stuff that can carry liftable objects
/// (ships, space stations)
/// </summary>
namespace Game.ECS.Components.Lifter
{
    public interface ILifter : IComponent
    {
        FasterList< ILiftable> CarriedThings { get;}
    }

    public interface ILifterLandingEvent
    {
        /// <summary>
        /// First index is the Id of the liftable, second index is the if of the lifter.
        /// Triggered when a liftable leave the lifter
        /// </summary>
        event System.Action< int, int> LeaveEvent;

        /// <summary>
        /// First index is the Id of the liftable, second index is the if of the lifter.
        /// Triggered when a liftable land onf the lifter
        /// </summary>
        event System.Action< int, int> LandEvent;
    }
}