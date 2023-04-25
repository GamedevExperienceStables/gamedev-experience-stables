using System;
using System.Collections.Generic;
using System.Linq;
using Game.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.Elements
{
    public class RuneDragAndDropManipulator : PointerManipulator
    {
        private Vector2 _targetStartPosition;
        private Vector3 _pointerStartPosition;

        private bool _enabled;

        private readonly VisualElement _root;
        private readonly IReadOnlyList<RuneSlotHudView> _hudSlots;

        private readonly Action _stopDragCallback;
        private readonly Action<RuneSlotDragCompleteEvent> _onDragComplete;

        private int _pointerId;
        
        private RuneSlotHudView _source;
        private RuneDefinition _rune;

        public RuneDragAndDropManipulator(VisualElement target, IEnumerable<RuneSlotHudView> hudSlots,
            Action onStopDrag, Action<RuneSlotDragCompleteEvent> onDragComplete)
        {
            _hudSlots = hudSlots.ToList();
            _stopDragCallback = onStopDrag;
            _onDragComplete = onDragComplete;

            this.target = target;
            _root = target.parent;
        }

        public void StartDrag(RuneSlotDragEvent evt)
        {
            _targetStartPosition = evt.elementPosition;
            _pointerStartPosition = evt.pointerPosition;
            _pointerId = evt.pointerId;
            _source = evt.source;
            _rune = evt.definition;

            target.transform.position = _targetStartPosition;

            target.CapturePointer(_pointerId);

            _enabled = true;
        }

        public void StopDrag()
        {
            if (!_enabled)
                return;

            ReleasePointer(_pointerId);

            target.transform.position = _targetStartPosition;

            _stopDragCallback.Invoke();

            _source = null;
            _rune = null;
            _pointerId = default;
            
            _enabled = false;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
            target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
            target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            if (!_enabled)
                return;

            if (!target.HasPointerCapture(evt.pointerId))
                return;

            Vector3 pointerDelta = evt.position - _pointerStartPosition;

            target.transform.position = new Vector2(
                _targetStartPosition.x + pointerDelta.x,
                _targetStartPosition.y + pointerDelta.y
            );
        }

        private void PointerUpHandler(PointerUpEvent evt)
            => ReleasePointer(evt.pointerId);

        private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
        {
            if (!_enabled)
                return;

            var overlappingSlots = _hudSlots.Where(OverlapsTarget);

            RuneSlotHudView closestOverlappingSlot = FindClosestSlot(overlappingSlots);
            if (closestOverlappingSlot is not null)
            {
                Vector3 closestPos = closestOverlappingSlot.Element.worldBound.position;
                target.transform.position = closestPos;
            }

            var result = new RuneSlotDragCompleteEvent
            {
                source = _source,
                target = closestOverlappingSlot,
                rune = _rune
            };
            
            _onDragComplete.Invoke(result);

            StopDrag();
        }

        private void ReleasePointer(int pointerId)
        {
            if (_enabled && target.HasPointerCapture(pointerId))
                target.ReleasePointer(pointerId);
        }

        private bool OverlapsTarget(RuneSlotHudView slot)
            => target.worldBound.Overlaps(slot.Element.worldBound);

        private static RuneSlotHudView FindClosestSlot(IEnumerable<RuneSlotHudView> slotsList)
        {
            float bestDistanceSq = float.MaxValue;

            RuneSlotHudView closest = null;
            foreach (RuneSlotHudView slotView in slotsList)
            {
                Vector3 displacement = slotView.Element.worldBound.position;
                float distanceSq = displacement.sqrMagnitude;
                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    closest = slotView;
                }
            }

            return closest;
        }
    }
}