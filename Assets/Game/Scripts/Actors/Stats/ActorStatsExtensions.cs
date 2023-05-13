using Game.Stats;

namespace Game.Actors
{
    public static class ActorStatsExtensions
    {
        public static void CreateStatsHealth(this ActorStats stats)
            => stats.CreateStatsMinMax(CharacterStats.Health, CharacterStats.HealthMax);

        public static void InitStatsHealth(this ActorStats stats, float baseValue)
            => stats.InitStatsMinMax(CharacterStats.Health, CharacterStats.HealthMax, baseValue);

        public static void CreateStatsMana(this ActorStats stats)
            => stats.CreateStatsMinMax(CharacterStats.Mana, CharacterStats.ManaMax);

        public static void InitStatsMana(this ActorStats stats, float baseValue)
            => stats.InitStatsMinMax(CharacterStats.Mana, CharacterStats.ManaMax, baseValue);

        public static void CreateStatsStamina(this ActorStats stats)
            => stats.CreateStatsMinMax(CharacterStats.Stamina, CharacterStats.StaminaMax);

        public static void InitStatsStamina(this ActorStats stats, float baseValue)
            => stats.InitStatsMinMax(CharacterStats.Stamina, CharacterStats.StaminaMax, baseValue);

        public static void CreateStatsMovement(this ActorStats stats)
            => stats.CreateStat(CharacterStats.MovementSpeed);

        public static void InitStatsMovement(this ActorStats stats, float baseValue)
            => stats.InitStat(CharacterStats.MovementSpeed, baseValue);
        
        public static void CreateStatsWeight(this ActorStats stats)
            => stats.CreateStat(CharacterStats.Weight);
        
        public static void InitStatsWeight(this ActorStats stats, float baseValue)
            => stats.InitStat(CharacterStats.Weight, baseValue);

        private static void CreateStatsMinMax(this ActorStats stats, CharacterStats current, CharacterStats max)
        {
            stats.CreateStat(current);
            stats.CreateStat(max);

            stats.AddStatHandler(new StatHandlerClamp(current, max));
        }

        private static void InitStatsMinMax(this ActorStats stats, CharacterStats current, CharacterStats max,
            float baseValue)
        {
            stats.InitStat(current, baseValue);
            stats.InitStat(max, baseValue);
        }
    }
}