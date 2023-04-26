using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yapp
{
    [CreateAssetMenu(fileName = Constants.TemplateCollection_FileName, menuName = Constants.TemplateCollection_MenuName)]
    [System.Serializable]
    public class PrefabTemplateCollection : ScriptableObject
    {
        /// <summary>
        /// Collection of various prefab settings templates
        /// </summary>
        [SerializeField]
        public List<PrefabSettingsTemplate> templates = new List<PrefabSettingsTemplate>();
    }
}