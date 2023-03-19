using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Inventory;
using Game.UI.Elements;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using VContainer;
using Button = UnityEngine.UIElements.Button;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class InventoryView : MonoBehaviour
    {
        private const int OFFSET_BOTTOM = -100;

        [SerializeField]
        private HudView hud;

        [Header("Show")]
        [SerializeField, Min(0f)]
        private float showDuration = 0.4f;
        
        [SerializeField]
        private GameObject showFeedbacks;

        [Header("Hide")]
        [SerializeField, Min(0f)]
        private float hideDuration = 0.2f;

        [SerializeField]
        private GameObject hideFeedbacks;

        private InventoryViewModel _viewModel;

        private VisualElement _container;
        private Button _buttonClose;

        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;

        private RuneSlotDraggerElement _runeDragger;
        private RuneDragAndDropManipulator _runeManipulator;

        private VisualElement _runeTitleIcon;
        private VisualElement _runeDetail;
        private Label _runeTitleText;
        private Label _runeDescription;
        
        private readonly List<RuneSlotInventoryView> _runeSlots = new();
        private VisualElement _root;

        [Inject]
        public void Construct(InventoryViewModel viewModel)
            => _viewModel = viewModel;

        private void Awake()
        {
            _showDuration = TimeSpan.FromSeconds(showDuration);
            _hideDuration = TimeSpan.FromSeconds(hideDuration);

            _root = GetComponent<UIDocument>().rootVisualElement;

            _container = _root.Q<VisualElement>(LayoutNames.Inventory.CONTAINER);
            _buttonClose = _root.Q<Button>(LayoutNames.Inventory.BUTTON_CLOSE);

            _runeDetail = _root.Q<VisualElement>(LayoutNames.Inventory.PAGE_DETAILS);
            _runeTitleIcon = _root.Q<VisualElement>(LayoutNames.Inventory.RUNE_ICON);
            _runeTitleText = _root.Q<Label>(LayoutNames.Inventory.RUNE_NAME);
            _runeDescription = _root.Q<Label>(LayoutNames.Inventory.RUNE_DESCRIPTION);

            CreateRuneSlots(_root, _viewModel.ObtainedRunes);

            _viewModel.SubscribeRuneAdded(OnRuneAdded);
            _buttonClose.clicked += OnCloseClicked;
        }

        private void Start()
        {
            CreateRuneDragger(_root, hud.RuneSlots);
            SubscribeHudRunes(hud.RuneSlots);
        }

        private void OnDestroy()
        {
            _viewModel.UnSubscribeRuneAdded(OnRuneAdded);
            _buttonClose.clicked -= OnCloseClicked;

            _runeManipulator.target.RemoveManipulator(_runeManipulator);

            UnSubscribeHudRunes(hud.RuneSlots);
            CleanupRunes();
        }

        private void SubscribeHudRunes(IEnumerable<RuneSlotHudView> hudRuneSlots)
        {
            foreach (RuneSlotHudView runeSlotHudView in hudRuneSlots)
                runeSlotHudView.SubscribeRemovingRequest(OnRuneRemovingRequest);
        }
        
        private void UnSubscribeHudRunes(IEnumerable<RuneSlotHudView> hudRuneSlots)
        {
            foreach (RuneSlotHudView runeSlotHudView in hudRuneSlots)
                runeSlotHudView.UnSubscribeRemovingRequest(OnRuneRemovingRequest);
        }

        private void CreateRuneSlots(VisualElement root, IReadOnlyList<RuneDefinition> runes)
        {
            var slots = root.Query<VisualElement>(className: LayoutNames.Inventory.RUNE_SLOT_CLASS_NAME).ToList();
            var passiveSlots = root.Query<VisualElement>(className: LayoutNames.Inventory.RUNE_PASSIVE_SLOT_CLASS_NAME).ToList();

            List<RuneDefinition> activeRunes = new(); 
            List<RuneDefinition> passiveRunes = new();

            foreach (RuneDefinition rune in _viewModel.AllRunes)
            {
                switch (rune.Type)
                {
                    case RuneType.Active:
                        activeRunes.Add(rune);
                        break;
                    case RuneType.Passive:
                        passiveRunes.Add(rune);
                        break;
                }
            }

            for (int i = 0; i < activeRunes.Count; i++)
            {
                if (i >= slots.Count)
                    break;
                
                var runeView = new RuneSlotInventoryView(slots[i], activeRunes[i]);
                runeView.SubscribeStartDrag(OnRuneStartDrag);
                runeView.SubscribeHover(OnRuneHover);

                if (runes.Contains(activeRunes[i]))
                    runeView.Activate();

                _runeSlots.Add(runeView);
            }

            for (int i = 0; i < passiveRunes.Count; i++)
            {
                if (i >= passiveSlots.Count)
                    break;
                
                var runeView = new RuneSlotInventoryView(passiveSlots[i], passiveRunes[i]);
                runeView.SubscribeHover(OnRuneHover);

                if (runes.Contains(passiveRunes[i]))
                    runeView.Activate();

                _runeSlots.Add(runeView);
            }
        }

        private void CleanupRunes()
        {
            foreach (RuneSlotInventoryView runeElement in _runeSlots)
            {
                runeElement.UnSubscribeStartDrag(OnRuneStartDrag);
                runeElement.UnsubscribeHover(OnRuneHover);
            }
        }

        private void CreateRuneDragger(VisualElement root, IReadOnlyList<RuneSlotHudView> hudSlots)
        {
            _runeDragger = root.Q<RuneSlotDraggerElement>();
            _runeManipulator = new RuneDragAndDropManipulator(
                _runeDragger,
                hudSlots,
                OnRuneStopDrag,
                OnRuneCompleteDrag
            );
        }

        private void OnRuneStartDrag(RuneSlotDragEvent evt)
        {
            _runeManipulator.StartDrag(evt);
            _runeDragger.StartDrag(evt.definition);
        }

        private void OnRuneHover(RuneSlotHoverEvent evt)
        {
            _runeDetail.visible = evt.state;
            _runeTitleIcon.style.backgroundImage = new StyleBackground(evt.definition.IconEmpty);
            _runeTitleText.text = evt.definition.name;
            _runeDescription.text = evt.definition.name;
        }

        private void OnRuneStopDrag()
            => _runeDragger.StopDrag();

        private void OnRuneCompleteDrag(RuneSlotHudView targetSlot)
        {
            RuneDefinition targetRune = _runeDragger.Rune;
            _viewModel.SetRuneToHudSlot(targetSlot.Id, targetRune);
        }

        private void OnRuneAdded(RuneDefinition rune)
        {
            foreach (RuneSlotInventoryView runeView in _runeSlots)
            {
                if (!runeView.Contains(rune))
                    continue;

                runeView.Activate();
                return;
            }
        }

        private void OnRuneRemovingRequest(RuneSlotRemoveEvent evt) 
            => _viewModel.RemoveRuneFromHudSlot(evt.id);

        private void OnCloseClicked()
            => _viewModel.CloseInventory();

        public void HideImmediate()
        {
            _container.SetDisplay(false);
            _container.SetOpacity(0f);
            _container.style.bottom = OFFSET_BOTTOM;

            OnHide();
            HideFeedbacks();
        }

        private void OnShow()
        {
            foreach (RuneSlotHudView hudRuneSlotView in hud.RuneSlots)
                hudRuneSlotView.EnableInteraction();
        }

        private void OnHide()
        {
            foreach (RuneSlotHudView hudRuneSlotView in hud.RuneSlots) 
                hudRuneSlotView.DisableInteraction();
        }

        public async UniTask ShowAsync()
        {
            FadeIn(_showDuration);
            await UniTask.Delay(_showDuration);
            OnShow();
        }

        public async UniTask HideAsync()
        {
            OnHide();
            FadeOut(_hideDuration);
            await UniTask.Delay(_hideDuration);
            
            HideFeedbacks();
        }

        private void FadeIn(TimeSpan duration)
        {
            if (showFeedbacks)
                showFeedbacks.SetActive(true);
            
            _container.SetDisplay(true);
            _container.experimental.animation
                .Start(new StyleValues { opacity = 1f, bottom = 0 }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InOutSine);
        }

        private void FadeOut(TimeSpan duration)
        {
            if (hideFeedbacks)
                hideFeedbacks.SetActive(true);
            
            _container.experimental.animation
                .Start(new StyleValues { opacity = 0f, bottom = OFFSET_BOTTOM }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InOutSine)
                .OnCompleted(() => _container.SetDisplay(false));
        }

        private void HideFeedbacks()
        {
            if (showFeedbacks)
                showFeedbacks.SetActive(false);

            if (hideFeedbacks)
                hideFeedbacks.SetActive(false);
        }
    }
}