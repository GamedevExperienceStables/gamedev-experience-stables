using System;
using Game.Input;

namespace Game.UI
{
    public class CutsceneViewModel
    {
        private readonly InputControlMenu _input;

        public CutsceneViewModel(InputControlMenu input) 
            => _input = input;

        public void SubscribeSkip(Action callback) 
            => _input.BackButton.Performed += callback;
    }
}