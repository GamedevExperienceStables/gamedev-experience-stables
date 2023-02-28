using DG.Tweening.Core;
using Game.Actors;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Animations.Hero
{
    public class ActorAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        
        public void SetAnimation(string animationName, bool isActive)
            => animator.SetBool(animationName, isActive);
        
        
        public void SetAnimation(string animationName, float animationParameter)
            => animator.SetFloat(animationName, animationParameter);
        
        
        public void SetAnimation(string animationName, int animationParameter)
            => animator.SetInteger(animationName, animationParameter);
        
    }
}
