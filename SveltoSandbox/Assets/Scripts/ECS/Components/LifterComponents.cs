using Svelto.DataStructures;

/// <summary>
/// Lifter components have to do with the movement of all the stuff that can carry liftable objects
/// (ships, space stations)
/// </summary>
namespace Game.ECS.Components.Lifter
{
    public interface ILifter : IComponent
    {
        FasterList< int> CarriedThings { get;}
    }

    public struct LifterLandingEvent
    {
        public int LifterID { get; set; }
        public int LiftableID { get; set; }
        public bool Landed { get; set; }
    }
}