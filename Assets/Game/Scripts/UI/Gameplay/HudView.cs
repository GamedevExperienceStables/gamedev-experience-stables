﻿using System;
using System.Collections.Generic;
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

        private Label _crystal;
        private Label _crystalMax;
        private VisualElement _crystalIcon;

        private float _currentMaxHp;
        private VisualElement _hpBarWidgetMask;
        private float _currentMaxMp;
        private VisualElement _mpBarWidgetMask;
        private float _currentMaxSp;
        private VisualElement _spBarWidgetMask;
        
        private HudRuneSlotsView _runeSlotsView;

        public IReadOnlyList<RuneSlotHudView> RuneSlots => _runeSlotsView.Slots;

        [Inject]
        public void Construct(GameplayViewModel viewModel, HudRuneSlotsView runeSlotsView)
        {
            _viewModel = viewModel;
            _runeSlotsView = runeSlotsView;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _buttonMenu = _root.Q<Button>(LayoutNames.Hud.BUTTON_MENU);
            _buttonMenu.clicked += PauseGame;

            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);

            var hpWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_HP);
            _hpBarWidgetMask = hpWidget.Q<VisualElement>(LayoutNames.Hud.WIDGET_BAR_MASK);

            var mpWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_MP);
            _mpBarWidgetMask = mpWidget.Q<VisualElement>(LayoutNames.Hud.WIDGET_BAR_MASK);

            var spWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_SP);
            _spBarWidgetMask = spWidget.Q<VisualElement>(LayoutNames.Hud.WIDGET_BAR_MASK);

            var crystalWidget = _root.Q<VisualElement>(LayoutNames.Hud.WIDGET_CRYSTAL);
            _crystalIcon = crystalWidget.Q<VisualElement>(LayoutNames.Hud.CRYSTAL_ICON);
            _crystal = crystalWidget.Q<Label>(LayoutNames.Hud.TEXT_CURRENT);
            _crystalMax = crystalWidget.Q<Label>(LayoutNames.Hud.TEXT_MAX);
            
            _runeSlotsView.Create(_root);

            InitCrystalView(_viewModel.GetCurrentMaterial());
            
            SubscribeStats();
        }

        private void OnDestroy()
        {
            _buttonMenu.clicked -= PauseGame;

            _runeSlotsView.Destroy();
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

            _viewModel.LevelBagMaterialSubscribe(UpdateCrystal);
        }

        private void UnSubscribeStats()
        {
            _viewModel.HeroStatUnSubscribe(CharacterStats.Health, UpdateHealth);
            _viewModel.HeroStatUnSubscribe(CharacterStats.HealthMax, UpdateHealthMax);

            _viewModel.HeroStatUnSubscribe(CharacterStats.Mana, UpdateMana);
            _viewModel.HeroStatUnSubscribe(CharacterStats.ManaMax, UpdateManaMax);

            _viewModel.HeroStatUnSubscribe(CharacterStats.Stamina, UpdateStamina);
            _viewModel.HeroStatUnSubscribe(CharacterStats.StaminaMax, UpdateStaminaMax);

            _viewModel.LevelBagMaterialUnSubscribe(UpdateCrystal);
        }

        private void UpdateHealth(StatValueChange change)
        {
            Length stylePercent = GetStylePercent(change.newValue, _currentMaxHp);
            _hpBarWidgetMask.style.height = stylePercent;
        }

        private void UpdateHealthMax(StatValueChange change)
            => _currentMaxHp = change.newValue;

        private void UpdateMana(StatValueChange change)
        {
            Length stylePercent = GetStylePercent(change.newValue, _currentMaxMp);
            _mpBarWidgetMask.style.height = stylePercent;
        }

        private void UpdateManaMax(StatValueChange change)
            => _currentMaxMp = change.newValue;

        private void UpdateStamina(StatValueChange change)
        {
            Length stylePercent = GetStylePercent(change.newValue, _currentMaxSp);
            _spBarWidgetMask.style.width = stylePercent;
        }

        private void UpdateStaminaMax(StatValueChange change)
            => _currentMaxSp = change.newValue;

        private void InitCrystalView(IReadOnlyMaterialData materialData)
        {
            _crystalIcon.style.unityBackgroundImageTintColor = materialData.Definition.Color;

            _crystal.text = materialData.Current.ToString(CultureInfo.InvariantCulture);
            _crystalMax.text = materialData.Total.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateCrystal(MaterialChangedData change)
            => _crystal.text = change.newValue.ToString(CultureInfo.InvariantCulture);

        private static Length GetStylePercent(float current, float max)
        {
            float percent = current / max * 100;
            var stylePercent = new Length(percent, LengthUnit.Percent);
            return stylePercent;
        }
    }
}