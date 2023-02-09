using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Game.Inventory;
using Game.Stats;
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
        private Label _crystal;
        private Label _crystalMax;

        private float _currentMaxHp;
        private VisualElement _hpBarWidgetMask;

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

            VisualElement hpBarWidget = _root.Q(LayoutNames.Hud.WIDGET_HP_BAR);
            _hpBarWidgetMask = hpBarWidget.Q<VisualElement>(LayoutNames.Hud.WIDGET_BAR_MASK);

            var mpWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_MP);
            mpWidget.Q<Label>(LayoutNames.Hud.TEXT_LABEL).text = "MP";
            _mp = mpWidget.Q<Label>(LayoutNames.Hud.TEXT_CURRENT);
            _mpMax = mpWidget.Q<Label>(LayoutNames.Hud.TEXT_MAX);

            var spWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_SP);
            spWidget.Q<Label>(LayoutNames.Hud.TEXT_LABEL).text = "SP";
            _sp = spWidget.Q<Label>(LayoutNames.Hud.TEXT_CURRENT);
            _spMax = spWidget.Q<Label>(LayoutNames.Hud.TEXT_MAX);

            var crystalWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_CRYSTAL);
            _crystal = crystalWidget.Q<Label>(LayoutNames.Hud.TEXT_CURRENT);
            _crystalMax = crystalWidget.Q<Label>(LayoutNames.Hud.TEXT_MAX);

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

        private void SubscribeStats()
        {
            _viewModel.HeroStatSubscribe(CharacterStats.Health, UpdateHealth);
            _viewModel.HeroStatSubscribe(CharacterStats.HealthMax, UpdateHealthMax);

            _viewModel.HeroStatSubscribe(CharacterStats.Mana, UpdateMana);
            _viewModel.HeroStatSubscribe(CharacterStats.ManaMax, UpdateManaMax);

            _viewModel.HeroStatSubscribe(CharacterStats.Stamina, UpdateStamina);
            _viewModel.HeroStatSubscribe(CharacterStats.StaminaMax, UpdateStaminaMax);

            _viewModel.BagMaterialsSubscribe(UpdateCrystal);
            _viewModel.BagMaterialsSubscribe(UpdateCrystalMax);
        }

        private void UnSubscribeStats()
        {
            _viewModel.HeroStatUnSubscribe(CharacterStats.Health, UpdateHealth);
            _viewModel.HeroStatUnSubscribe(CharacterStats.HealthMax, UpdateHealthMax);

            _viewModel.HeroStatUnSubscribe(CharacterStats.Mana, UpdateMana);
            _viewModel.HeroStatUnSubscribe(CharacterStats.ManaMax, UpdateManaMax);

            _viewModel.HeroStatUnSubscribe(CharacterStats.Stamina, UpdateStamina);
            _viewModel.HeroStatUnSubscribe(CharacterStats.StaminaMax, UpdateStaminaMax);

            _viewModel.BagMaterialsUnSubscribe(UpdateCrystal);
            _viewModel.BagMaterialsUnSubscribe(UpdateCrystalMax);
        }

        private void UpdateHealth(StatValueChange change)
        {
            Length stylePercent = GetStylePercent(change.newValue, _currentMaxHp);
            _hpBarWidgetMask.style.height = stylePercent;

            _hp.text = change.newValue.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateHealthMax(StatValueChange change)
        {
            _currentMaxHp = change.newValue;
            _hpMax.text = change.newValue.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateMana(StatValueChange change)
            => _mp.text = change.newValue.ToString(CultureInfo.InvariantCulture);

        private void UpdateCrystal(MaterialChangedData change)
            => _crystal.text = change.newValue.ToString(CultureInfo.InvariantCulture);

        private void UpdateManaMax(StatValueChange change)
            => _mpMax.text = change.newValue.ToString(CultureInfo.InvariantCulture);

        private void UpdateStamina(StatValueChange change)
            => _sp.text = change.newValue.ToString(CultureInfo.InvariantCulture);

        private void UpdateStaminaMax(StatValueChange change)
            => _spMax.text = change.newValue.ToString(CultureInfo.InvariantCulture);

        private void UpdateCrystalMax(MaterialChangedData change)
            => _crystalMax.text = change.newValue.ToString(CultureInfo.InvariantCulture);

        private static Length GetStylePercent(float current, float max)
        {
            float percent = current / max * 100;
            var stylePercent = new Length(percent, LengthUnit.Percent);
            return stylePercent;
        }
    }
}