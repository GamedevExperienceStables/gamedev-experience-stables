using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class HudView : MonoBehaviour
    {
        [SerializeField]
        private float showDuration = 0.15f;

        [SerializeField]
        private float hideDuration = 0.1f;

        private VisualElement _root;
        private Button _buttonMenu;
        private GameplayViewModel _viewModel;
        private bool _isVisible;

        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;

        private Label _hp;
        private Label _hpMax;
        private Label _mp;
        private Label _mpMax;
        private Label _sp;
        private Label _spMax;

        [Inject]
        public void Construct(GameplayViewModel viewModel)
            => _viewModel = viewModel;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _buttonMenu = _root.Q<Button>(LayoutNames.Hud.BUTTON_MENU);
            _buttonMenu.clicked += PauseGame;

            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);

            var hpWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_HP);
            hpWidget.Q<Label>(LayoutNames.Hud.TEXT_LABEL).text = "HP";
            _hp = hpWidget.Q<Label>(LayoutNames.Hud.TEXT_CURRENT);
            _hpMax = hpWidget.Q<Label>(LayoutNames.Hud.TEXT_MAX);

            var mpWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_MP);
            mpWidget.Q<Label>(LayoutNames.Hud.TEXT_LABEL).text = "MP";
            _mp = mpWidget.Q<Label>(LayoutNames.Hud.TEXT_CURRENT);
            _mpMax = mpWidget.Q<Label>(LayoutNames.Hud.TEXT_MAX);

            var spWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_SP);
            spWidget.Q<Label>(LayoutNames.Hud.TEXT_LABEL).text = "SP";
            _sp = spWidget.Q<Label>(LayoutNames.Hud.TEXT_CURRENT);
            _spMax = spWidget.Q<Label>(LayoutNames.Hud.TEXT_MAX);

            SubscribeStats();
        }

        private void OnDestroy()
        {
            _buttonMenu.clicked -= PauseGame;

            UnSubscribeStats();
        }

        public void HideImmediate()
            => _root.SetDisplay(false);

        public void Show()
            => FadeIn(_showDuration);

        public void Hide()
            => FadeOut(_hideDuration);

        public UniTask ShowAsync()
        {
            FadeIn(_showDuration);
            return UniTask.Delay(_showDuration, true);
        }

        public UniTask HideAsync()
        {
            FadeOut(_hideDuration);
            return UniTask.Delay(_hideDuration, true);
        }

        private void PauseGame()
            => _viewModel.PauseGame();

        private void FadeIn(TimeSpan duration)
        {
            _root.SetDisplay(true);
            _root.experimental.animation
                .Start(new StyleValues { opacity = 1f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic);
        }

        private void FadeOut(TimeSpan duration)
        {
            _root.experimental.animation
                .Start(new StyleValues { opacity = 0f }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InCubic)
                .OnCompleted(() => _root.SetDisplay(false));
        }

        #region AwaitsRefactoring

        private void SubscribeStats()
        {
            _viewModel.HeroHealth.Current.Subscribe(UpdateHealth);
            _viewModel.HeroHealth.Max.Subscribe(UpdateHealthMax);

            _viewModel.HeroMana.Current.Subscribe(UpdateMana);
            _viewModel.HeroMana.Max.Subscribe(UpdateManaMax);

            _viewModel.HeroStamina.Current.Subscribe(UpdateStamina);
            _viewModel.HeroStamina.Max.Subscribe(UpdateStaminaMax);
        }

        private void UnSubscribeStats()
        {
            _viewModel.HeroHealth.Current.UnSubscribe(UpdateHealth);
            _viewModel.HeroHealth.Max.UnSubscribe(UpdateHealthMax);

            _viewModel.HeroMana.Current.UnSubscribe(UpdateMana);
            _viewModel.HeroMana.Max.UnSubscribe(UpdateManaMax);

            _viewModel.HeroStamina.Current.UnSubscribe(UpdateStamina);
            _viewModel.HeroStamina.Max.UnSubscribe(UpdateStaminaMax);
        }

        private void UpdateHealth(float value)
            => _hp.text = value.ToString(CultureInfo.InvariantCulture);

        private void UpdateHealthMax(float value)
            => _hpMax.text = value.ToString(CultureInfo.InvariantCulture);

        private void UpdateMana(float value)
            => _mp.text = value.ToString(CultureInfo.InvariantCulture);

        private void UpdateManaMax(float value)
            => _mpMax.text = value.ToString(CultureInfo.InvariantCulture);

        private void UpdateStamina(float value)
            => _sp.text = value.ToString(CultureInfo.InvariantCulture);

        private void UpdateStaminaMax(float value)
            => _spMax.text = value.ToString(CultureInfo.InvariantCulture);

        #endregion
    }
}