using Game.Stats;

namespace Game.Enemies
{
    public class EnemyStats : IMovableStats, IDamageableStats
    {
        public CharacterStatWithMax Health { get; } = new();
        public CharacterStat Movement { get; } = new();

        public void InitStats(EnemyDefinition definition)
        {
            Health.Init(definition.Health);
            Movement.Init(definition.MovementSpeed);
        }
    }
}