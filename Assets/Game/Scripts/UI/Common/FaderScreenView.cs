using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class FaderScreenView : LoadingScreenView, IFaderScreen
    {
    }
}