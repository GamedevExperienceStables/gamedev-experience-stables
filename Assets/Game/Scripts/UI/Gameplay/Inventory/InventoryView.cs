using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Inventory;
using Game.UI.Elements;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Button = UnityEngine.UIElements.Button;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class InventoryView : MonoBehaviour
    {
        private const float SHOW_DURATION = 0.1f;
        private const float HIDE_DURATION = 0.2f;
        
        [SerializeField]
        private HudView hud;

        [Header("Show")]
        [SerializeField]
        private GameObject showFeedbacks;

        [Header("Hide")]
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
        private VisualElement _runeDetails;
        private Label _runeTitleText;
        private Label _runeDescription;
        private Label _runeLevelText;

        private readonly List<RuneSlotInventoryView> _runeSlots = new();
        private VisualElement _root;
        private VisualElement _book;

        [Inject]
        public void Construct(InventoryViewModel viewModel)
            => _viewModel = viewModel;

        private void Awake()
        {
            _showDuration = TimeSpan.FromSeconds(SHOW_DURATION);
            _hideDuration = TimeSpan.FromSeconds(HIDE_DURATION);

            _root = GetComponent<UIDocument>().rootVisualElement;

            _container = _root.Q<VisualElement>(LayoutNames.Inventory.SCREEN);
            _book = _container.Q<VisualElement>(LayoutNames.Inventory.BOOK);
            _buttonClose = _root.Q<Button>(LayoutNames.Inventory.BUTTON_CLOSE);

            _runeDetails = _root.Q<VisualElement>(LayoutNames.Inventory.PAGE_DETAILS);
            _runeTitleIcon = _root.Q<VisualElement>(LayoutNames.Inventory.RUNE_ICON);
            _runeTitleText = _root.Q<Label>(LayoutNames.Inventory.RUNE_NAME);
            _runeLevelText = _root.Q<Label>(LayoutNames.Inventory.RUNE_LEVEL);
            _runeDescription = _root.Q<Label>(LayoutNames.Inventory.RUNE_DESCRIPTION);

            CreateRuneSlots(_root, _viewModel.ObtainedRunes);

            _viewModel.SubscribeRuneAdded(OnRuneAdded);
            _buttonClose.clicked += OnCloseClicked;
        }

        private void Start()
        {
            CreateRuneDragger(_root, hud.RuneSlots.Values);
            SubscribeHudRunes(hud.RuneSlots.Values);
        }

        private void OnDestroy()
        {
            _viewModel.UnSubscribeRuneAdded(OnRuneAdded);
            _buttonClose.clicked -= OnCloseClicked;

            _runeManipulator.target.RemoveManipulator(_runeManipulator);

            UnSubscribeHudRunes(hud.RuneSlots.Values);
            CleanupRunes();
        }

        private void SubscribeHudRunes(IEnumerable<RuneSlotHudView> hudRuneSlots)
        {
            foreach (RuneSlotHudView runeSlotHudView in hudRuneSlots)
            {
                runeSlotHudView.SubscribeRemovingRequest(OnRuneRemovingRequest);
                runeSlotHudView.SubscribeStartDrag(OnRuneStartDrag);
            }
        }

        private void UnSubscribeHudRunes(IEnumerable<RuneSlotHudView> hudRuneSlots)
        {
            foreach (RuneSlotHudView runeSlotHudView in hudRuneSlots)
            {
                runeSlotHudView.UnSubscribeRemovingRequest(OnRuneRemovingRequest);
                runeSlotHudView.UnSubscribeStartDrag(OnRuneStartDrag);
            }
        }

        private void CreateRuneSlots(VisualElement root, IReadOnlyList<RuneDefinition> runes)
        {
            var slots = root.Query<VisualElement>(className: LayoutNames.Inventory.RUNE_SLOT_CLASS_NAME).ToList();
            var passiveSlots = root.Query<VisualElement>(className: LayoutNames.Inventory.RUNE_PASSIVE_SLOT_CLASS_NAME)
                .ToList();

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

        private void CreateRuneDragger(VisualElement root, IEnumerable<RuneSlotHudView> hudSlots)
        {
            _runeDragger = root.Q<RuneSlotDraggerElement>();
            _runeManipulator = new RuneDragAndDropManipulator(
                _runeDragger,
                hudSlots,
                OnRuneDragStop,
                OnRuneDragComplete
            );
        }

        private void OnRuneStartDrag(RuneSlotDragEvent evt)
        {
            _runeManipulator.StartDrag(evt);
            _runeDragger.StartDrag(evt.definition);
        }

        private void OnRuneHover(RuneSlotHoverEvent evt)
        {
            if (!evt.state)
            {
                HideDetails();
                return;
            }

            RuneDefinition definition = evt.definition;

            _runeTitleIcon.style.backgroundImage = new StyleBackground(definition.IconEmpty);
            _runeTitleText.text = definition.Name.GetLocalizedString();
            _runeDescription.text = definition.Description.GetLocalizedString();
            _runeLevelText.text = definition.Level.Text.GetLocalizedString();
            _runeLevelText.style.color = definition.Level.Color;

            _runeDetails.RemoveFromClassList(LayoutNames.Inventory.BOOK_DETAILS_HIDDEN_CLASS_NAME);
        }

        private void HideDetails()
            => _runeDetails.AddToClassList(LayoutNames.Inventory.BOOK_DETAILS_HIDDEN_CLASS_NAME);

        private void OnRuneDragStop()
            => _runeDragger.StopDrag();

        private void OnRuneDragComplete(RuneSlotDragCompleteEvent evt)
        {
            bool sourceIsEmpty = evt.source is null;
            bool targetIsEmpty = evt.target is null;

            if (!sourceIsEmpty && !targetIsEmpty)
            {
                SwapSlots(evt.source.Id, evt.target.Id);
            }
            else
            {
                if (!sourceIsEmpty)
                    _viewModel.RemoveRuneFromHudSlot(evt.source.Id);

                if (!targetIsEmpty)
                    _viewModel.SetRuneToHudSlot(evt.target.Id, evt.rune);
            }
        }

        private void SwapSlots(RuneSlotId sourceId, RuneSlotId targetId)
        {
            RuneDefinition sourceRune = _viewModel.GetRuneFromHudSlot(sourceId);
            RuneDefinition targetRune = _viewModel.GetRuneFromHudSlot(targetId);

            _viewModel.SetRuneToHudSlot(targetId, sourceRune);
            _viewModel.SetRuneToHudSlot(sourceId, targetRune);
        }

        private void SwapSlots(RuneSlotId sourceId, RuneDefinition sourceRune, RuneSlotId targetId,
            RuneDefinition targetRune)
        {
            _viewModel.SetRuneToHudSlot(targetId, sourceRune);
            _viewModel.SetRuneToHudSlot(sourceId, targetRune);
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
            _container.SetVisibility(false);
            _book.AddToClassList(LayoutNames.Inventory.BOOK_HIDDEN_CLASS_NAME);

            HideDetails();

            OnHide();
            HideFeedbacks();
        }

        private void OnShow()
        {
            foreach (RuneSlotHudView hudRuneSlotView in hud.RuneSlots.Values)
                hudRuneSlotView.EnableInteraction();
        }

        private void OnHide()
        {
            _runeManipulator.StopDrag();

            foreach (RuneSlotHudView hudRuneSlotView in hud.RuneSlots.Values)
                hudRuneSlotView.DisableInteraction();
        }

        public async UniTask ShowAsync()
        {
            Show();
            await UniTask.Delay(_showDuration);
            OnShow();
        }

        public async UniTask HideAsync()
        {
            OnHide();
            Hide();
            await UniTask.Delay(_hideDuration);
            _container.SetVisibility(false);

            HideFeedbacks();
        }

        private void Show()
        {
            ShowFeedbacks();

            _container.SetVisibility(true);
            _book.RemoveFromClassList(LayoutNames.Inventory.BOOK_HIDDEN_CLASS_NAME);
        }

        private void Hide()
        {
            if (hideFeedbacks)
                hideFeedbacks.SetActive(true);

            _book.AddToClassList(LayoutNames.Inventory.BOOK_HIDDEN_CLASS_NAME);
        }

        private void ShowFeedbacks()
        {
            if (showFeedbacks)
                showFeedbacks.SetActive(true);
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