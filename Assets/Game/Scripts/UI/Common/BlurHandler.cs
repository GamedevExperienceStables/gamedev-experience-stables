using System;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.UI
{
    public class BlurHandler
    {
        private readonly DepthOfField _dof;
        private readonly float _initialFocalLenght;

        public BlurHandler(Volume globalVolume)
        {
            globalVolume.profile.TryGet(out _dof);

            _initialFocalLenght = _dof.focalLength.value;
        }

        public void HideImmediate()
        {
            _dof.focalLength.value = 0f;
            _dof.active = false;
        }

        public void FadeIn(TimeSpan duration)
        {
            _dof.active = true;

            DOTween.To(GetFocalLenght, SetFocalLenght, _initialFocalLenght, (float)duration.TotalSeconds)
                .SetUpdate(true);
        }

        public void FadeOut(TimeSpan duration)
        {
            DOTween.To(GetFocalLenght, SetFocalLenght, 0f, (float)duration.TotalSeconds)
                .SetUpdate(true)
                .OnComplete(() => _dof.active = false);
        }

        private float GetFocalLenght()
            => _dof.focalLength.value;

        private void SetFocalLenght(float value)
            => _dof.focalLength.value = value;
    }
}