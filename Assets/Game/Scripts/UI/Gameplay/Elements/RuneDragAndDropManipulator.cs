using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Action<RuneSlotHudView> _onDragComplete;

        public RuneDragAndDropManipulator(VisualElement target, IReadOnlyList<RuneSlotHudView> hudSlots,
            Action onStopDrag, Action<RuneSlotHudView> onDragComplete)
        {
            _hudSlots = hudSlots;
            _stopDragCallback = onStopDrag;
            _onDragComplete = onDragComplete;

            this.target = target;
            _root = target.parent;
        }

        public void StartDrag(RuneSlotDragEvent evt)
        {
            _targetStartPosition = evt.elementPosition;
            _pointerStartPosition = evt.pointerPosition;

            target.transform.position = _targetStartPosition;

            target.CapturePointer(evt.pointerId);

            _enabled = true;
        }

        private void StopDrag()
        {
            target.transform.position = _targetStartPosition;

            _stopDragCallback.Invoke();

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
        {
            if (_enabled && target.HasPointerCapture(evt.pointerId))
                target.ReleasePointer(evt.pointerId);
        }

        private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
        {
            if (!_enabled)
                return;

            var overlappingSlots = _hudSlots.Where(OverlapsTarget);

            RuneSlotHudView closestOverlappingSlot = FindClosestSlot(overlappingSlots);
            if (closestOverlappingSlot != null)
            {
                Vector3 closestPos = RootSpaceOfSlot(closestOverlappingSlot.Element);
                closestPos = new Vector2(closestPos.x - 5, closestPos.y - 5);
                target.transform.position = closestPos;

                _onDragComplete.Invoke(closestOverlappingSlot);
            }
            else
            {
                target.transform.position = _targetStartPosition;
            }
            
            StopDrag();
        }

        private bool OverlapsTarget(RuneSlotHudView slot) 
            => target.worldBound.Overlaps(slot.Element.worldBound);

        private RuneSlotHudView FindClosestSlot(IEnumerable<RuneSlotHudView> slotsList)
        {
            float bestDistanceSq = float.MaxValue;

            RuneSlotHudView closest = null;
            foreach (RuneSlotHudView slotView in slotsList)
            {
                VisualElement slot = slotView.Element;
                Vector3 displacement = RootSpaceOfSlot(slot) - target.transform.position;
                float distanceSq = displacement.sqrMagnitude;
                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    closest = slotView;
                }
            }

            return closest;
        }

        private Vector3 RootSpaceOfSlot(VisualElement slot)
        {
            Vector2 slotWorldSpace = slot.parent.LocalToWorld(slot.layout.position);
            return _root.WorldToLocal(slotWorldSpace);
        }
    }
}