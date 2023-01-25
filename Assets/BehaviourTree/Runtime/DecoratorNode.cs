using UnityEngine;

namespace BehaviourTree
{
    public abstract class DecoratorNode : Node
    {

        [SerializeReference]
        [HideInInspector]
        public Node child;
    }
}
