using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class VersionView : MonoBehaviour
    {
        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Label>("version").text = Application.version;
            root.Q<Label>("label").text = Application.productName;
        }
    }
}