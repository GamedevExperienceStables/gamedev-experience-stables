using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "Cutscene")]
    public class CutsceneDefinition : ScriptableObject
    {
        [SerializeField]
        private List<CutsceneSlide> slides;

        public List<CutsceneSlide> Slides => slides;
    }
}