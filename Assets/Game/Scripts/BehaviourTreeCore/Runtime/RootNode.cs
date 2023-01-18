using UnityEngine;

namespace BehaviourTreeCore
{
    [System.Serializable]
    public class RootNode : Node
    {
        [SerializeReference]
        [HideInInspector]
        public Node child;

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate() => child.Update();
    }
}