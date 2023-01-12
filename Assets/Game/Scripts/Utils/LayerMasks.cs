using UnityEngine;

namespace Game.Utils
{
    public static class LayerMasks
    {
        public static readonly LayerMask Player = 1 << 3;
        public static readonly LayerMask Npc = 1 << 6;
        public static readonly LayerMask Projectile = 1 << 7;
    }
}