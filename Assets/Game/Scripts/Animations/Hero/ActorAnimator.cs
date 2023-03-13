using UnityEngine;

namespace Game.Animations.Hero
{
    public class ActorAnimator : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetAnimation(string animationName, bool isActive) 
            => animator.SetBool(animationName, isActive);


        public void SetAnimation(string animationName, float animationParameter)
            => animator.SetFloat(animationName, animationParameter);
        
        
        public void SetAnimation(string animationName, int animationParameter)
            => animator.SetInteger(animationName, animationParameter);
        
        public void SetAnimation(string animationParameter)
            => animator.SetTrigger(animationParameter);

        public void ResetAnimation(string animationParameter)
            => animator.ResetTrigger(animationParameter);
        
        public void PlayAnimation(string animationName)
            => animator.Play(animationName);
    }
}
