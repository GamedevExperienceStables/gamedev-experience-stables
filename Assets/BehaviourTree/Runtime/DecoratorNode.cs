using UnityEngine;

namespace BehaviourTree.Runtime
{
    public abstract class DecoratorNode : Node
    {

        [SerializeReference]
        [HideInInspector]
        public Node child;
    }
}
