using Game.Inventory;
using UnityEngine;

namespace Game.UI
{
    public struct RuneSlotDragEvent
    {
        public int pointerId;
        public Vector2 elementPosition;
        public Vector2 pointerPosition;
        public RuneDefinition definition;
    }
}