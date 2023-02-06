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
            IsEnabled = true;
            OnGiveAbility();
        }

        public void RemoveAbility()
        {
            if (IsActive)
                CancelAbility();

            IsEnabled = false;
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

        public void TryActivateAbility()
        {
            if (!IsEnabled)
                return;

            if (CanActivateAbility())
                ActivateAbility();
        }

        public void ActivateAbility()
        {
            IsActive = true;
            OnActivateAbility();
        }

        public void CancelAbility()
        {
            if (IsActive)
                OnEndAbility(true);
            
            IsActive = false;
        }

        public void EndAbility()
        {
            if (IsActive)
                OnEndAbility(false);

            IsActive = false;
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