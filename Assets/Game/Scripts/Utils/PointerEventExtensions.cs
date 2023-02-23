using UnityEngine.UIElements;

namespace Game.Utils
{
    public static class PointerEventExtensions
    {
        public static bool IsLeftButton(this IPointerEvent evt) 
            => evt.button == 0;
        
        public static bool IsRightButton(this IPointerEvent evt) 
            => evt.button == 1;
    }
}