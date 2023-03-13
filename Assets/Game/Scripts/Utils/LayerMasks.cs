using UnityEngine;

namespace Game.Utils
{
    public static class LayerMasks
    {
        public static readonly LayerMask Default = 1 << 0;
        public static readonly LayerMask Player = 1 << 3;
        public static readonly LayerMask Npc = 1 << 6;
        public static readonly LayerMask Projectile = 1 << 7;
        public static readonly LayerMask Ground = 1 << 8;
        public static readonly LayerMask Environment = 1 << 9;
        public static readonly LayerMask Enemy = 1 << 11;
    }
}