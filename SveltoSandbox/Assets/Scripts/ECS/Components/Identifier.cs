namespace Game.ECS.Components
{
    public interface IIdentifier: IComponent
    {
        int Id { get; set; }
    }
}