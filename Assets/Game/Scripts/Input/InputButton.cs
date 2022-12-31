using System;
using UnityEngine;

namespace Game.Input
{
    public class InputButton
    {
        private float _lastButtonDownAt;
        private float _lastButtonUpAt;

        public event Action Performed;
        public event Action Canceled;

        public InputButtonStates State { get; private set; } = InputButtonStates.Off;
        public float TimeSinceLastButtonDown => Time.unscaledTime - _lastButtonDownAt;
        public float TimeSinceLastButtonUp => Time.unscaledTime - _lastButtonUpAt;
        public bool IsDown => State == InputButtonStates.Down;

        public void TriggerButtonDown()
        {
            Performed?.Invoke();
            State = InputButtonStates.Down;

            _lastButtonDownAt = Time.unscaledTime;
        }

        public void TriggerButtonUp()
        {
            Canceled?.Invoke();
            State = InputButtonStates.Up;

            _lastButtonUpAt = Time.unscaledTime;
        }

        public bool ButtonDownRecently(float time)
        {
            return Time.unscaledTime - TimeSinceLastButtonDown <= time;
        }

        public bool ButtonUpRecently(float time)
        {
            return Time.unscaledTime - TimeSinceLastButtonUp <= time;
        }
    }
}