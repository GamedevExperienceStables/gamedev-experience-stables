namespace Game.Level
{
    public interface ILocationPersistenceFact
    {
        string Id { get; }        
        bool IsConfirmed { get; }
        
        void Confirm();
    }
}