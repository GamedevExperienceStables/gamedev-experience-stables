using DG.Tweening;
using Game.Inventory;
using Game.Utils;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    [RequireComponent(typeof(RocketContainer))]
    public class RocketContainerView : MonoBehaviour
    {
        [Header("Transfer")]
        [SerializeField]
        private float transferDuration = 1f;

        [SerializeField]
        private GameObject transferEffect;

        [Header("Activation")]
        [SerializeField]
        private MMF_Player activationEffect;

        [Header("Core")]
        [SerializeField]
        private GameObject coreObject;

        [SerializeField]
        private Light coreLight;

        [SerializeField]
        private AnimationCurve lightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField]
        private Material inactiveMaterial;

        private RocketContainerHandler _handler;
        private RocketContainer _container;
        private Renderer _coreRenderer;

        private Material _activeMaterial;
        private float _currentProgress;

        private bool _initialized;

        [Inject]
        public void Construct(RocketContainerHandler handler)
        {
            _handler = handler;
            _initialized = true;
        }

        private void Awake()
        {
            if (transferEffect)
                transferEffect.SetActive(false);

            _container = GetComponent<RocketContainer>();
            _container.TransferCompleted += OnTransferCompleted;

            _coreRenderer = coreObject.GetComponent<Renderer>();
            _activeMaterial = new Material(_coreRenderer.material);
        }

        private void OnDestroy()
            => _container.TransferCompleted -= OnTransferCompleted;

        private void Start()
        {
            if (!_initialized)
                return;

            UpdateState(0);
        }

        private void OnTransferCompleted()
            => UpdateState(transferDuration);

        private void UpdateState(float duration)
        {
            float progress = GetProgress();
            if (progress.AlmostEquals(1f))
                Activate(duration);

            UpdateState(progress, duration);
        }

        private void UpdateState(float progress, float duration)
        {
            if (duration.AlmostZero())
                SetStateImmediate(progress);
            else
                SetStateAnimate(progress, duration);
        }

        private void SetStateAnimate(float progress, float duration)
        {
            if (transferEffect)
                transferEffect.SetActive(true);

            coreLight.DOIntensity(GetLightIntensity(progress), duration)
                .SetEase(Ease.OutSine);

            DOTween
                .To(() => _currentProgress, value => _currentProgress = value, progress, duration)
                .OnUpdate(() => _coreRenderer.material.Lerp(inactiveMaterial, _activeMaterial, _currentProgress))
                .SetEase(Ease.OutSine);
        }

        private void SetStateImmediate(float progress)
        {
            _currentProgress = progress;

            _coreRenderer.material.Lerp(inactiveMaterial, _activeMaterial, progress);
            coreLight.intensity = GetLightIntensity(progress);
        }

        private void Activate(float duration)
        {
            if (!activationEffect)
                return;


            activationEffect.PlayFeedbacks();

            if (duration <= 0)
                activationEffect.SkipToTheEnd();
        }

        private float GetProgress()
        {
            MaterialDefinition targetMaterial = _container.TargetMaterial;

            float current = _handler.GetCurrentValue(targetMaterial);
            float total = _handler.GetTotalValue(targetMaterial);

            if (total.AlmostZero())
                return 0f;

            return current / total;
        }

        private float GetLightIntensity(float progress)
            => lightCurve.Evaluate(progress);
    }
}