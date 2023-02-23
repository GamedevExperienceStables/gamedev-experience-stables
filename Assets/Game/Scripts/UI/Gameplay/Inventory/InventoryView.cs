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

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class InventoryView : MonoBehaviour
    {
        private const int OFFSET_BOTTOM = -100;

        [SerializeField]
        private HudView hud;

        [SerializeField, Min(0f)]
        private float showDuration = 0.4f;

        [SerializeField, Min(0f)]
        private float hideDuration = 0.2f;

        private InventoryViewModel _viewModel;

        private VisualElement _container;
        private Button _buttonClose;

        private TimeSpan _showDuration;
        private TimeSpan _hideDuration;

        private RuneSlotDraggerElement _runeDragger;
        private RuneDragAndDropManipulator _runeManipulator;
        
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

            for (int i = 0; i < _viewModel.AllRunes.Count; i++)
            {
                if (i >= slots.Count)
                    break;

                RuneDefinition runeDefinition = _viewModel.AllRunes[i];
                
                var runeView = new RuneSlotInventoryView(slots[i], runeDefinition);
                runeView.SubscribeStartDrag(OnRuneStartDrag);

                if (runes.Contains(runeDefinition))
                    runeView.Activate();

                _runeSlots.Add(runeView);
            }
        }

        private void CleanupRunes()
        {
            foreach (RuneSlotInventoryView runeElement in _runeSlots)
                runeElement.UnSubscribeStartDrag(OnRuneStartDrag);
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

        public UniTask HideAsync()
        {
            OnHide();
            FadeOut(_hideDuration);
            return UniTask.Delay(_hideDuration);
        }

        private void FadeIn(TimeSpan duration)
        {
            _container.SetDisplay(true);
            _container.experimental.animation
                .Start(new StyleValues { opacity = 1f, bottom = 0 }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InOutSine);
        }

        private void FadeOut(TimeSpan duration)
        {
            _container.experimental.animation
                .Start(new StyleValues { opacity = 0f, bottom = OFFSET_BOTTOM }, (int)duration.TotalMilliseconds)
                .Ease(Easing.InOutSine)
                .OnCompleted(() => _container.SetDisplay(false));
        }
    }
}