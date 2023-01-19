namespace Game.Scripts.Stats
{
    public abstract class StatModifier
    {
        public readonly float value;
        public readonly StatsModifierType type;
        public readonly int order;

        protected StatModifier(float value, StatsModifierType type, int order)
        {
            this.value = value;
            this.type = type;
            this.order = order;
        }
    }
}
