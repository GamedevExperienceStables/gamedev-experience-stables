using UnityEngine;

namespace Game.Actors
{
    public abstract class ActorAbilityView : MonoBehaviour
    {
        public IActorController Owner { get; set; }

        public bool IsEnabled { get; private set; }
        public bool IsActive { get; private set; }

        public void InitAbility()
            => OnInitAbility();

        public void EnableAbility()
        {
            IsEnabled = true;
            OnEnableAbility();
        }

        public void DisableAbility()
        {
            IsEnabled = false;
            OnDisableAbility();
        }

        public void DestroyAbility()
        {
            if (IsEnabled)
                OnDisableAbility();

            OnDestroyAbility();
        }
        
        public void ResetAbility()
        {
            if (IsEnabled)
                OnResetAbility();
        }

        public void TryActivateAbility()
        {
            if (!IsEnabled)
                return;

            if (CanActivateAbility())
                ActivateAbility();
            else
                EndAbility();
        }

        public void ActivateAbility()
        {
            IsActive = true;
            OnActivateAbility();
        }

        public void EndAbility()
        {
            if (IsActive)
                OnEndAbility();

            IsActive = false;
        }

        public abstract bool CanActivateAbility();


        protected virtual void OnInitAbility()
        {
        }

        protected virtual void OnEnableAbility()
        {
        }

        protected virtual void OnDisableAbility()
        {
        }

        protected virtual void OnActivateAbility()
        {
        }

        protected virtual void OnEndAbility()
        {
        }
        
        protected virtual void OnResetAbility()
        {
        }

        protected virtual void OnDestroyAbility()
        {
        }
    }
}