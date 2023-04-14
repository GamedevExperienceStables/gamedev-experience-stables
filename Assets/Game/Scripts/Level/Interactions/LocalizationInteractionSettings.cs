using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Level
{
    [Serializable]
    public class LocalizationInteractionSettings
    {
        [SerializeField]
        private LocalizedString interactionDefault;

        [SerializeField]
        private RocketContainerInteractionLocalization rocketContainer;

        [SerializeField]
        private TransitionInteractionLocalization transition;

        [SerializeField]
        private LevelExitInteractionLocalization levelExit;

        [SerializeField]
        private SaveGameInteractionLocalization saveGame;

        public LocalizedString DefaultText => interactionDefault;
        public LevelExitInteractionLocalization LevelExit => levelExit;
        public RocketContainerInteractionLocalization RocketContainer => rocketContainer;
        public SaveGameInteractionLocalization SaveGame => saveGame;
        public TransitionInteractionLocalization Transition => transition;

        [Serializable]
        public struct TransitionInteractionLocalization
        {
            [SerializeField]
            private LocalizedString textDoor;

            [Header("To Planet")]
            [SerializeField]
            private LocationPointDefinition planetLocation;

            [SerializeField]
            private LocalizedString textToPlanet;

            [Header("To Rocket")]
            [SerializeField]
            private LocationPointDefinition rocketLocation;

            [SerializeField]
            private LocalizedString textToRocket;

            public LocalizedString TextDoor => textDoor;
            public LocationPointDefinition PlanetLocation => planetLocation;
            public LocalizedString TextToPlanet => textToPlanet;
            public LocationPointDefinition RocketLocation => rocketLocation;
            public LocalizedString TextToRocket => textToRocket;
        }

        [Serializable]
        public struct SaveGameInteractionLocalization
        {
            [SerializeField]
            private LocalizedString text;

            public LocalizedString Text => text;
        }


        [Serializable]
        public struct LevelExitInteractionLocalization
        {
            [SerializeField]
            private LocalizedString textTrue;

            [SerializeField]
            private LocalizedString textFalse;

            public LocalizedString TextTrue => textTrue;
            public LocalizedString TextFalse => textFalse;
        }

        [Serializable]
        public struct RocketContainerInteractionLocalization
        {
            [SerializeField]
            private LocalizedString textOk;

            [SerializeField]
            private LocalizedString textEmpty;

            [SerializeField]
            private LocalizedString textFull;

            public LocalizedString TextOk => textOk;
            public LocalizedString TextEmpty => textEmpty;
            public LocalizedString TextFull => textFull;
        }
    }
}