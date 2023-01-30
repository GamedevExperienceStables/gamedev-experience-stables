namespace Game.Stats
{
    public struct StatValueChange
    {
        public readonly CharacterStats stat;
        public readonly float newValue;
        public readonly float oldValue;

        public StatValueChange(CharacterStats stat, float oldValue, float newValue)
        {
            this.stat = stat;
            this.newValue = newValue;
            this.oldValue = oldValue;
        }
    }
}