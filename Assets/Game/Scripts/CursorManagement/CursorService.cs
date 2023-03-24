using System;
using UnityEngine;

// ReSharper disable MemberCanBeMadeStatic.Global
namespace Game.CursorManagement
{
    public class CursorService
    {
        private readonly Settings _settings;

        public CursorService(Settings settings) 
            => _settings = settings;

        public void SetAlternative()
            => Cursor.SetCursor(_settings.Alternative, Vector2.zero, CursorMode.Auto);

        public void SetVisible(bool isVisible) 
            => Cursor.visible = isVisible;

        public void Reset()
        {
            SetVisible(true);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        [Serializable]
        public class Settings
        {
            [SerializeField]
            private Texture2D alternative;

            public Texture2D Alternative => alternative;
        }
    }
}