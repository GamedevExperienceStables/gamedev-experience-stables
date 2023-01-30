using Game.Actors;

namespace Game.Stats
{
    public readonly struct StatModifierApplyData
    {
        public readonly CharacterStats stat;
        public readonly StatsModifierType type;
        public readonly float value;
        
        public readonly ActorController target;

        public StatModifierApplyData(CharacterStats stat, StatsModifierType type, float value,
            ActorController target)
        {
            this.stat = stat;
            this.type = type;
            this.value = value;
            this.target = target;
        }
    }
}