using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class CutsceneActionNext
    {
        private const float SHIFT_DISTANCE = 30f;

        private VisualElement _container;
        private float _direction = 1;

        public void Create(VisualElement root)
        {
            _container = root.Q<VisualElement>(LayoutNames.Cutscene.BLOCK_ACTION);

            _container.schedule
                .Execute(() =>
                {
                    _direction *= -1;

                    float shiftDistance = SHIFT_DISTANCE * _direction;
                    _container.transform.position += new Vector3(shiftDistance, 0);
                })
                .Every(500);
        }

        public void Show()
            => _container.RemoveFromClassList(LayoutNames.Cutscene.ACTION_HIDDEN_CLASS_NAME);

        public void Hide()
            => _container.AddToClassList(LayoutNames.Cutscene.ACTION_HIDDEN_CLASS_NAME);
    }
}