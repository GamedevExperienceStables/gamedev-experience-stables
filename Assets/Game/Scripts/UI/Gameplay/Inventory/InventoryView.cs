using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Inventory;
using Game.UI.Elements;
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
        private const string UNKNOWN_TEXT = "??????";
        private readonly Color _unknownColor = Color.grey;

        [SerializeField]
        private HudView hud;


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
        
        private InventoryFx _fx;

        [Inject]
        public void Construct(InventoryViewModel viewModel, InventoryFx fx)
        {
            _viewModel = viewModel;
            _fx = fx;
        }

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
            _viewModel.SubscribeRuneRemoved(OnRuneRemoved);
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
            _viewModel.UnSubscribeRuneRemoved(OnRuneRemoved);
            _buttonClose.clicked -= OnCloseClicked;
            
            UnSubscribeHudRunes(hud.RuneSlots.Values);
            Cleanup();
            
            _fx.Destroy();
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

            for (int i = 0; i < slots.Count; i++)
            {
                var runeView = new RuneSlotInventoryView(slots[i]);
                runeView.SubscribeStartDrag(OnRuneStartDrag);
                runeView.SubscribeHover(OnRuneHover);

                if (i < activeRunes.Count)
                {
                    RuneDefinition rune = activeRunes[i];
                    runeView.SetRune(rune);
                    if (runes.Contains(rune))
                        runeView.Activate();
                }

                _runeSlots.Add(runeView);
            }

            for (int i = 0; i < passiveSlots.Count; i++)
            {
                var runeView = new RuneSlotInventoryView(passiveSlots[i]);
                runeView.SubscribeHover(OnRuneHover);

                if (i < passiveRunes.Count)
                {
                    RuneDefinition rune = passiveRunes[i];
                    runeView.SetRune(rune);
                    if (runes.Contains(rune))
                    {
                        runeView.Activate();
                    }
                }

                _runeSlots.Add(runeView);
            }
        }

        private void CreateRuneDragger(VisualElement root, IEnumerable<RuneSlotHudView> hudSlots)
        {
            _runeDragger = root.Q<RuneSlotDraggerElement>();
            _runeManipulator = new RuneDragAndDropManipulator(_runeDragger, hudSlots);

            _runeManipulator.DragStopped += OnRuneDragStopped;
            _runeManipulator.DragCompleted += OnRuneDragCompleted;
        }

        private void Cleanup()
        {
            foreach (RuneSlotInventoryView runeElement in _runeSlots)
            {
                runeElement.UnSubscribeStartDrag(OnRuneStartDrag);
                runeElement.UnsubscribeHover(OnRuneHover);
            }

            _runeManipulator.target.RemoveManipulator(_runeManipulator);
            _runeManipulator.DragStopped -= OnRuneDragStopped;
            _runeManipulator.DragCompleted -= OnRuneDragCompleted;
        }

        private void OnRuneStartDrag(RuneSlotDragEvent evt)
        {
            if (!evt.definition || !IsActive(evt.definition)) 
                return;
            
            _runeManipulator.StartDrag(evt);
            _runeDragger.StartDrag(evt.definition.Icon);
        }

        private void OnRuneHover(RuneSlotHoverEvent evt)
        {
            if (!evt.state)
            {
                HideDetails();
                return;
            }

            if (evt.definition && IsActive(evt.definition))
            {
                RuneDefinition definition = evt.definition;

                _runeTitleIcon.style.backgroundImage = new StyleBackground(definition.IconEmpty);
                _runeTitleText.text = definition.Name.GetLocalizedString();
                _runeDescription.text = definition.Description.GetLocalizedString();
                _runeLevelText.text = definition.Level.Text.GetLocalizedString();
                _runeLevelText.style.color = definition.Level.Color;

                _fx.RuneHoverFeedback();
            }
            else
            {
                _runeTitleIcon.style.backgroundImage = null;
                _runeTitleText.text = UNKNOWN_TEXT;
                _runeDescription.text = UNKNOWN_TEXT;
                _runeLevelText.text = UNKNOWN_TEXT;
                _runeLevelText.style.color = _unknownColor;
            }

            _runeDetails.RemoveFromClassList(LayoutNames.Inventory.BOOK_DETAILS_HIDDEN_CLASS_NAME);
        }

        private bool IsActive(RuneDefinition runeDefinition) 
            => _viewModel.ObtainedRunes.Contains(runeDefinition);

        private void HideDetails()
            => _runeDetails.AddToClassList(LayoutNames.Inventory.BOOK_DETAILS_HIDDEN_CLASS_NAME);

        private void OnRuneDragStopped()
            => _runeDragger.StopDrag();

        private void OnRuneDragCompleted(RuneSlotDragCompleteEvent evt)
        {
            bool sourceIsEmpty = evt.source is null;
            bool targetIsEmpty = evt.target is null;

            if (!sourceIsEmpty && !targetIsEmpty)
            {
                _viewModel.SwapSlots(evt.source.Id, evt.target.Id);
                _fx.PlaceRuneFeedback();
            }
            else
            {
                if (!sourceIsEmpty) 
                    RemoveRune(evt.source.Id);

                if (!targetIsEmpty)
                {
                    _viewModel.SetRuneToHudSlot(evt.target.Id, evt.rune);
                    _fx.PlaceRuneFeedback();
                }
            }
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

        private void OnRuneRemoved(RuneDefinition rune)
        {
            foreach (RuneSlotInventoryView runeView in _runeSlots)
            {
                if (!runeView.Contains(rune))
                    continue;

                runeView.Deactivate();
                return;
            }
        }

        private void OnRuneRemovingRequest(RuneSlotRemoveEvent evt) 
            => RemoveRune(evt.id);

        private void RemoveRune(RuneSlotId slotId)
        {
            _fx.RemoveRuneFeedback();
            _viewModel.RemoveRuneFromHudSlot(slotId);
        }

        private void OnCloseClicked()
            => _viewModel.CloseInventory();

        public void HideImmediate()
        {
            _container.AddToClassList(LayoutNames.Inventory.SCREEN_HIDDEN_CLASS_NAME);
            _book.AddToClassList(LayoutNames.Inventory.BOOK_HIDDEN_CLASS_NAME);

            HideDetails();

            OnHide();
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
            _container.AddToClassList(LayoutNames.Inventory.SCREEN_HIDDEN_CLASS_NAME);
        }

        private void Show()
        {
            _fx.ShowFeedback();

            _container.RemoveFromClassList(LayoutNames.Inventory.SCREEN_HIDDEN_CLASS_NAME);
            _book.RemoveFromClassList(LayoutNames.Inventory.BOOK_HIDDEN_CLASS_NAME);
        }

        private void Hide()
        {
            _fx.HideFeedback();
            
            _book.AddToClassList(LayoutNames.Inventory.BOOK_HIDDEN_CLASS_NAME);
        }
    }
}