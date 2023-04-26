using Game.Utils.Factory;

namespace Game.Actors
{
    public abstract class ActorAbility : IRuntimeInstance<AbilityDefinition>
    {
        public IActorController Owner { get; set; }

        public bool IsEnabled { get; private set; }
        public bool IsActive { get; private set; }

        public AbilityDefinition Definition { get; private set; }

        public virtual void OnCreate(AbilityDefinition definition)
            => Definition = definition;

        public void InitAbility()
            => OnInitAbility();

        public void GiveAbility()
        {
            if (IsEnabled)
                return;
            
            IsEnabled = true;
            OnGiveAbility();
        }

        public void RemoveAbility()
        {
            if (!IsEnabled)
                return;
            
            IsEnabled = false;
            
            if (IsActive)
                CancelAbility();

            OnRemoveAbility();
        }

        public void DestroyAbility()
        {
            if (IsEnabled)
                RemoveAbility();

            OnDestroyAbility();
        }

        public void ResetAbility()
        {
            if (IsEnabled)
                OnResetAbility();
        }

        public bool TryActivateAbility()
        {
            if (!IsEnabled)
                return false;

            if (!CanActivateAbility())
                return false;

            ActivateAbility();
            return true;
        }

        public void ActivateAbility()
        {
            IsActive = true;
            OnActivateAbility();
        }

        public void CancelAbility()
        {
            if (!IsActive)
                return;

            IsActive = false;
            OnEndAbility(true);
        }

        public void EndAbility()
        {
            if (!IsActive) 
                return;
            
            IsActive = false;
            OnEndAbility(false);
        }

        public abstract bool CanActivateAbility();


        protected virtual void OnInitAbility()
        {
        }

        protected virtual void OnGiveAbility()
        {
        }

        protected virtual void OnRemoveAbility()
        {
        }

        protected abstract void OnActivateAbility();

        protected virtual void OnEndAbility(bool wasCancelled)
        {
        }

        protected virtual void OnResetAbility()
        {
        }

        protected virtual void OnDestroyAbility()
        {
        }
    }

    public abstract class ActorAbility<T> : ActorAbility where T : AbilityDefinition
    {
        protected new T Definition { get; private set; }

        public override void OnCreate(AbilityDefinition definition)
        {
            base.OnCreate(definition);

            Definition = (T)definition;
        }
    }
}