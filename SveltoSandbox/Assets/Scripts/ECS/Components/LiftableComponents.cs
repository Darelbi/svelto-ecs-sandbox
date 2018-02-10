using Game.ECS.Components.Lifter;
using Svelto.ECS;

/// <summary>
/// Liftable components are all those components that can move freely in the space or can
/// be carried inside space stations and space ships. A liftable object is typically 1 square size
/// </summary>
namespace Game.ECS.Components.Liftable
{
    /// <summary>
    /// Allow to query the carrier for the current liftable object
    /// </summary>
    public interface ILiftable: IComponent
    {
        ILifter Carrier { get; set; }
    }
}
