using System;
using UnityEngine;

namespace Game.UI
{
    [Serializable]
    public struct EmployeeData
    {
        public string name;
        public string position;
        public string url;
        
        public Sprite icon;
    }
}