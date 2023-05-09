using DG.Tweening;
using UnityEngine;

namespace Game.Hero
{
    public class HeroStaffView : MonoBehaviour
    {
        private const float COLOR_BLEND_DURATION = 0.2f;
        
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        [SerializeField]
        private Renderer staff;

        private Color _defaultColor;

        private void Awake()
            => _defaultColor = staff.material.GetColor(EmissionColor);

        public void SetColor(Color color) 
            => AnimateColor(color);

        public void ResetColor()
            => AnimateColor(_defaultColor);

        private void AnimateColor(Color color)
        {
            staff.material.DOBlendableColor(color, EmissionColor, COLOR_BLEND_DURATION)
                .SetEase(Ease.OutSine);
        }
    }
}