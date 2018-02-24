using UnityEngine;

/// <summary>
/// Lifter components have to do with the movement of all the stuff that can carry liftable objects
/// (ships, space stations)
/// </summary>
namespace Game.ECS.Components.Movement
{
    /// <summary>
    /// Position of the entity in 2d space.
    /// </summary>
    public interface IPosition2D: IComponent
    {
        Vector2 Position { get; set; }
    }

    /// <summary>
    /// We do not want speed, but movement
    /// </summary>
    public interface IMovement2D: IComponent
    {
        Vector2 Movement { get; set; }
    }

    /// <summary>
    /// Position of the entity in sectors
    /// </summary>
    public interface ISector2D: IComponent
    {
        int SectorX { get; set; }
        int SectorY { get; set; }
    }
}