namespace Game.Level
{
    public interface ILocationPoint : ILocationPointKeyOwner
    {
        ILocationDefinition Location { get; }
    }
}