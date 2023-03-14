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

        public void SetAnimation(int animationName, bool isActive)
        {
            animator.SetBool(animationName, isActive);
        }
        


        public void SetAnimation(int animationName, float animationParameter)
            => animator.SetFloat(animationName, animationParameter);
        
        
        public void SetAnimation(int animationName, int animationParameter)
            => animator.SetInteger(animationName, animationParameter);
        
        public void SetAnimation(int animationParameter)
            => animator.SetTrigger(animationParameter);

        public void ResetAnimation(int animationParameter)
            => animator.ResetTrigger(animationParameter);
        
        public void PlayAnimation(int animationName)
            => animator.Play(animationName);
    }
}
