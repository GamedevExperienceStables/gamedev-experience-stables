﻿using JetBrains.Annotations;

namespace Game.Hero
{
    [UsedImplicitly]
    public class PlayerData
    {
        public HeroStats Stats { get; } = new();
        public HeroAbilities Abilities { get; } = new();
    }
}