using System;
using System.Collections.Generic;
using Game.TimeManagement;
using Game.Utils;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class CutsceneView
    {
        private const int HOLD_TIME = 2;
        private const int TYPE_WRITER_CHAR_PER_SEC = 30;

        private readonly TimerPool _timers;
        private readonly Typewriter _typewriter;
        private readonly TimerUpdatable _holdTimer;

        private VisualElement _container;
        private TypewriterLabel _subtitles;
        private Image _slide;

        private List<CutsceneSlide> _slides;
        private VisualElement _slider;
        
        private VisualElement _action;

        private int _currentSlideIndex;
        private int _currentTextIndex;
        
        private VisualElement _skipAction;
        private Label _skipActionText;

        public event Action Completed;

        public CutsceneView(TimerPool timers, Typewriter typewriter)
        {
            _timers = timers;
            _typewriter = typewriter;
            _typewriter.onComplete = CompleteText;

            _holdTimer = _timers.GetTimer(TimeSpan.FromSeconds(HOLD_TIME), OnHoldComplete);
        }


        public void Create(VisualElement root)
        {
            _container = root.Q<VisualElement>(LayoutNames.Cutscene.CONTAINER);
            _slider = root.Q<VisualElement>(LayoutNames.Cutscene.SLIDER);

            _slide = _container.Q<Image>(LayoutNames.Cutscene.SLIDE_CURRENT);
            _subtitles = _container.Q<TypewriterLabel>(LayoutNames.Cutscene.TEXT_SUBTITLES);

            _action = _container.Q<VisualElement>(LayoutNames.Cutscene.BLOCK_ACTION);

            _skipAction = _container.Q<VisualElement>(LayoutNames.Cutscene.BLOCK_SKIP);
            _skipActionText = _container.Q<Label>(LayoutNames.Cutscene.TEXT_SKIP);
            
            _action.SetVisibility(false);
            _skipAction.SetVisibility(false);
        }

        public void Destroy()
        {
            _timers.ReleaseTimer(_holdTimer);
            _typewriter.Dispose();
        }

        public void Start(List<CutsceneSlide> slides)
        {
            _slides = slides;

            ShowSlide(0);
        }

        private void ShowSlide(int slideIndex)
        {
            _currentSlideIndex = slideIndex;

            SetImage(slideIndex);
            StartText(0);
        }

        private void SetImage(int slideIndex)
        {
            CutsceneSlide slide = _slides[slideIndex];
            _slide.sprite = slide.Image;
        }

        private void StartText(int i)
        {
            CutsceneSlide slide = GetCurrentSlide();
            LocalizedString text = slide.Texts[i];
            _subtitles.Text = text.GetLocalizedString();

            _typewriter.Start(_subtitles, TYPE_WRITER_CHAR_PER_SEC);
            _action.SetVisibility(false);
        }

        private void CompleteText()
        {
            _typewriter.Complete();
            _action.SetVisibility(true);
        }

        public void Next()
        {
            if (!_typewriter.IsComplete)
            {
                CompleteText();
                return;
            }

            bool hasSlides = _currentSlideIndex < _slides.Count - 1;
            if (!hasSlides)
            {
                Complete();
                return;
            }

            CutsceneSlide slide = GetCurrentSlide();
            bool hasTexts = _currentTextIndex < slide.Texts.Count - 1;
            if (hasTexts)
                ShowNextText();
            else
                ShowNextSlide();
        }

        private void ShowNextSlide()
        {
            _currentSlideIndex++;
            ShowSlide(_currentSlideIndex);
        }

        private void ShowNextText()
        {
            _currentTextIndex++;
            StartText(_currentTextIndex);
        }

        private void Complete()
            => Completed?.Invoke();

        private CutsceneSlide GetCurrentSlide()
            => _slides[_currentSlideIndex];

        public void StartHoldExit()
        {
            _holdTimer.Start();
            _skipAction.SetVisibility(true);
        }

        public void StopHoldExit()
        {
            _holdTimer.Stop();
            _skipAction.SetVisibility(false);
        }

        private void OnHoldComplete()
            => Completed?.Invoke();
    }
}