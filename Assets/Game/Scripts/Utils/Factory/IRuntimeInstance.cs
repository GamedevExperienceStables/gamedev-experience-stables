namespace Game.Utils.Factory
{
    public interface IRuntimeInstance<in TDefinition>
    {
        void OnCreate(TDefinition definition);
    }
}