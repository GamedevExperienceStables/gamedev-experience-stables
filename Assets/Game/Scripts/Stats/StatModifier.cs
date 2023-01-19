namespace Game.Stats
{
    public abstract class StatModifier
    {
        public readonly float value;
        public readonly StatsModifierType type;

        protected StatModifier(float value, StatsModifierType type)
        {
            this.value = value;
            this.type = type;
        }
    }
}
