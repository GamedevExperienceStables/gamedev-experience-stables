namespace Game.Stats
{
    public abstract class StatHandler
    {
        public virtual bool OnModifierApplying(IStats stats, StatModifierApplyData data) 
            => true;

        public virtual void OnModifierApplied(IStats stats, StatModifierApplyData data)
        {
        }

        public virtual float OnStatChanging(IStats stats, CharacterStats key, float newValue)
            => newValue;

        public virtual float OnStatBaseValueChanging(IStats stats, CharacterStats key, float newValue)
            => newValue;

        public virtual void OnStatValueChanged(IStats stats, StatValueChange change)
        {
        }
    }
}