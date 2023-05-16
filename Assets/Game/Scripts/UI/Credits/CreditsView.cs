using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class CreditsView : MonoBehaviour
    {
        private VisualElement _container;
        private VisualElement _list;
        private VisualElement _wrapper;
        private VisualElement _spacer;

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

            _spacer = _container.Q<VisualElement>(LayoutNames.Credits.SPACER);

            _list = root.Q<VisualElement>(LayoutNames.Credits.LIST);

            _teamTemplate = _list.Q<TemplateContainer>(LayoutNames.Credits.TEAM_TEMPLATE).templateSource;
            _memberTemplate = _list.Q<TemplateContainer>(LayoutNames.Credits.MEMBER_TEMPLATE).templateSource;

            _list.Clear();

            CreateTeams(_settings.teams.Teams);
            
            _container.SetVisibility(false);
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
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            // set spacer height as screen height
            float screenHeight = _wrapper.resolvedStyle.height;
            _spacer.style.height = screenHeight;

            float height = _container.resolvedStyle.height + screenHeight;
            TimeSpan duration = TimeSpan.FromSeconds(height / _settings.scrollSpeed / 100);

            PlayAnimation(onComplete, height, duration);
            
            _container.SetVisibility(true);
        }

        private void PlayAnimation(Action onComplete, float height, TimeSpan duration)
        {
            // _container.experimental.animation
            //     .Start(
            //         new StyleValues { top = -height },
            //         (int)duration.TotalMilliseconds
            //     )
            //     .Ease(Easing.Linear)
            //     .OnCompleted(onComplete);

            // DOTween.To(
            //         value =>
            //         {
            //             Length currentValue = _container.style.translate.value.x;
            //             
            //             var styleTranslate = new Translate(currentValue, value);
            //             _container.style.translate = styleTranslate;
            //         },
            //         _container.style.translate.value.y.value,
            //         -height,
            //         duration
            //     )
            //     .SetEase(Ease.Linear)
            //     .OnComplete(() => onComplete?.Invoke());


            _container.experimental.animation
                .Start(
                    element => element.style.translate.value.y.value,
                    -height,
                    (int)duration.TotalMilliseconds,
                    (element, y) =>
                    {
                        var styleTranslate = new Translate(element.style.translate.value.x, y);
                        element.style.translate = styleTranslate;
                    }
                )
                .Ease(Easing.Linear)
                .OnCompleted(onComplete);
        }


        [Serializable]
        public class Settings
        {
            [SerializeField, Min(0.01f)]
            public float scrollSpeed = 1f;

            public TeamsSettings teams;
        }
    }
}