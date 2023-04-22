namespace Game.Level
{
    public interface ISwitchObject
    {
        bool IsDirty { get; set; }

        bool IsActive { get; set; }
    }
}