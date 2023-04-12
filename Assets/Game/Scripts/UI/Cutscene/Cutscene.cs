using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.TimeManagement;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class Cutscene
    {
        private const int HOLD_TIME = 2;
        private const int TYPE_WRITER_CHAR_PER_SEC = 30;

        private readonly TimerPool _timers;
        private readonly Typewriter _typewriter;
        private readonly TimerUpdatable _holdTimer;

        private TypewriterLabel _textSubtitles;
        private Image _slide;

        private List<CutsceneSlide> _slides;

        private CutsceneActionArrow _action = new();

        private int _currentSlideIndex;
        private int _currentTextIndex;

        private VisualElement _skipAction;
        private Label _skipActionText;
        private VisualElement _blockSubtitles;

        private CancellationToken _cancellationToken;
        private bool _inTransition;

        public event Action Completed;

        [Inject]
        public Cutscene(TimerPool timers, Typewriter typewriter)
        {
            _timers = timers;
            _typewriter = typewriter;
            _typewriter.OnComplete = CompleteText;

            _holdTimer = _timers.GetTimer(TimeSpan.FromSeconds(HOLD_TIME), OnHoldComplete);
        }


        public void Create(VisualElement root, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            var container = root.Q<VisualElement>(LayoutNames.Cutscene.CONTAINER);

            _slide = container.Q<Image>(LayoutNames.Cutscene.SLIDE_CURRENT);

            _blockSubtitles = container.Q<VisualElement>(LayoutNames.Cutscene.BLOCK_SUBTITLES);
            _textSubtitles = container.Q<TypewriterLabel>(LayoutNames.Cutscene.TEXT_SUBTITLES);

            _action.Create(container);

            _skipAction = container.Q<VisualElement>(LayoutNames.Cutscene.BLOCK_SKIP);
            _skipActionText = container.Q<Label>(LayoutNames.Cutscene.TEXT_SKIP);

            _action.Hide();
            HideSkipAction();
            HideSlide();
            HideSubtitles();
        }

        public void Destroy()
        {
            _timers.ReleaseTimer(_holdTimer);
            _typewriter.Dispose();
        }

        public void Start(List<CutsceneSlide> slides)
        {
            _slides = slides;

            Show(0).Forget();
        }

        public void Next()
        {
            if (_inTransition)
                return;
            
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
        
        public void StartHoldExit()
        {
            _holdTimer.Start();

            ShowSkipAction();
        }

        public void StopHoldExit()
        {
            _holdTimer.Stop();

            HideSkipAction();
        }

        private async UniTask Show(int slideIndex)
        {
            _inTransition = true;
            
            _currentSlideIndex = slideIndex;

            _action.Hide();
            HideSlide();
            HideSubtitles();

            bool cancelled = await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cancellationToken)
                .SuppressCancellationThrow();
            if (cancelled)
                return;

            SetImage(slideIndex);
            ShowSlide();

            cancelled = await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cancellationToken)
                .SuppressCancellationThrow();
            if (cancelled)
                return;

            ShowSubtitles();

            StartText(0);

            _inTransition = false;
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
            _textSubtitles.Text = text.GetLocalizedString();

            _typewriter.Start(_textSubtitles, TYPE_WRITER_CHAR_PER_SEC);

            _action.Hide();
        }

        private void CompleteText()
        {
            _typewriter.Complete();

            _action.Show();
        }

        private void ShowNextSlide()
        {
            _currentTextIndex = 0;
            _currentSlideIndex++;

            Show(_currentSlideIndex).Forget();
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

        private void OnHoldComplete()
            => Completed?.Invoke();

        private void ShowSlide()
            => _slide.RemoveFromClassList(LayoutNames.Cutscene.SLIDE_HIDDEN_CLASS_NAME);

        private void HideSlide()
            => _slide.AddToClassList(LayoutNames.Cutscene.SLIDE_HIDDEN_CLASS_NAME);

        private void ShowSubtitles()
            => _blockSubtitles.RemoveFromClassList(LayoutNames.Cutscene.SUBTITLES_HIDDEN_CLASS_NAME);

        private void HideSubtitles()
            => _blockSubtitles.AddToClassList(LayoutNames.Cutscene.SUBTITLES_HIDDEN_CLASS_NAME);

        private void ShowSkipAction()
            => _skipAction.RemoveFromClassList(LayoutNames.Cutscene.ACTION_HIDDEN_CLASS_NAME);

        private void HideSkipAction()
            => _skipAction.AddToClassList(LayoutNames.Cutscene.ACTION_HIDDEN_CLASS_NAME);
    }
}