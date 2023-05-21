using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class CreditsView : MonoBehaviour
    {
        private VisualElement _container;
        private VisualElement _list;
        private VisualElement _wrapper;
        private VisualElement _intro;

        private VisualTreeAsset _teamTemplate;
        private VisualTreeAsset _memberTemplate;

        private Settings _settings;

        private StyleTranslate _position;

        [Inject]
        public void Construct(Settings settings)
            => _settings = settings;


        private void Awake()
        {
            var document = GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            _wrapper = root.Q<VisualElement>(LayoutNames.Credits.WRAPPER);
            _container = root.Q<VisualElement>(LayoutNames.Credits.CONTAINER);

            var introText = _container.Q<Label>(LayoutNames.Credits.INTRO_TEXT);
            introText.text = _settings.introText.GetLocalizedString();

            _intro = _container.Q<VisualElement>(LayoutNames.Credits.INTRO);

            _list = root.Q<VisualElement>(LayoutNames.Credits.LIST);

            _teamTemplate = _list.Q<TemplateContainer>(LayoutNames.Credits.TEAM_TEMPLATE).templateSource;
            _memberTemplate = _list.Q<TemplateContainer>(LayoutNames.Credits.MEMBER_TEMPLATE).templateSource;

            _list.Clear();

            CreateTeams(_settings.teams.Teams);

            _container.AddToClassList(LayoutNames.Credits.CONTAINER_HIDDEN_CLASS_NAME);
        }


        private void CreateTeams(List<TeamData> teams)
        {
            foreach (TeamData team in teams)
            {
                VisualElement element = CreateTeam(team);
                _list.Add(element);
            }
        }

        private VisualElement CreateTeam(TeamData team)
        {
            TemplateContainer instance = _teamTemplate.Instantiate();

            var teamName = instance.Q<Label>(LayoutNames.Credits.TEAM_NAME);
            teamName.text = team.name;

            var list = instance.Q<VisualElement>(LayoutNames.Credits.TEAM_LIST);
            foreach (EmployeeData member in team.list)
            {
                VisualElement item = CreateMember(member);
                list.Add(item);
            }

            return instance;
        }

        private VisualElement CreateMember(EmployeeData member)
        {
            TemplateContainer instance = _memberTemplate.Instantiate();

            var memberPosition = instance.Q<Label>(LayoutNames.Credits.MEMBER_POSITION);
            memberPosition.text = member.position;

            var memberName = instance.Q<Label>(LayoutNames.Credits.MEMBER_NAME);
            memberName.text = member.name;

            return instance;
        }

        public void Play(Action onComplete)
        {
            PlayDelay(onComplete).Forget();
        }

        private async UniTask PlayDelay(Action onComplete)
        {
            bool canceled = await UniTask
                .DelayFrame(1, cancellationToken: destroyCancellationToken)
                .SuppressCancellationThrow();

            if (canceled)
                return;

            float height = UpdateIntroHeight();

            _container.RemoveFromClassList(LayoutNames.Credits.CONTAINER_HIDDEN_CLASS_NAME);

            canceled = await UniTask
                .Delay(TimeSpan.FromSeconds(_settings.startDelay), cancellationToken: destroyCancellationToken)
                .SuppressCancellationThrow();

            if (canceled)
                return;

            TimeSpan duration = TimeSpan.FromSeconds(height / _settings.scrollSpeed / 100);
            PlayAnimation(onComplete, height, duration);
        }

        private float UpdateIntroHeight()
        {
            // set height as screen height
            float screenHeight = _wrapper.resolvedStyle.height;
            _intro.style.height = screenHeight;

            return _container.resolvedStyle.height + screenHeight;
        }

        private void PlayAnimation(Action onComplete, float height, TimeSpan duration)
        {
            DOTween
                .To(
                    getter: () => _container.style.translate.value.y.value,
                    setter: y =>
                    {
                        var styleTranslate = new Translate(_container.style.translate.value.x, y);
                        _container.style.translate = styleTranslate;
                    },
                    endValue: -height,
                    duration: (float)duration.TotalSeconds
                )
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete?.Invoke())
                .ToUniTask(cancellationToken: destroyCancellationToken)
                .SuppressCancellationThrow();
        }


        [Serializable]
        public class Settings
        {
            [Range(0f, 10f)]
            public float startDelay = 2f;

            [Min(0.01f)]
            public float scrollSpeed = 1f;

            public LocalizedString introText;

            public TeamsSettings teams;
        }
    }
}