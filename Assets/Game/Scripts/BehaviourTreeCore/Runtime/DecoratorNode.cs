using UnityEngine;

namespace BehaviourTreeCore
{
    public abstract class DecoratorNode : Node
    {
        [SerializeReference]
        [HideInInspector]
        public Node child;
    }
}
