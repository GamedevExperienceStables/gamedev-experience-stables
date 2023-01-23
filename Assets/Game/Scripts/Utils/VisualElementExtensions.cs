using UnityEngine.UIElements;

namespace Game.Utils
{
    public static class VisualElementExtensions
    {
        public static void SetDisplay(this VisualElement element, bool isDisplay)
            => element.style.display = isDisplay ? DisplayStyle.Flex : DisplayStyle.None;
        
        public static void SetOpacity(this VisualElement element, float value)
            => element.style.opacity = value;
    }
}