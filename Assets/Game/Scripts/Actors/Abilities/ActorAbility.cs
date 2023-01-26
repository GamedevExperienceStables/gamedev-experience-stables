namespace Game.Actors
{
    public abstract class ActorAbility
    {
        public IActorController Owner { get; set; }

        public bool IsEnabled { get; private set; }
        public bool IsActive { get; private set; }

        private AbilityDefinition Definition { get; set; }

        public virtual void CreateAbility(AbilityDefinition definition) 
            => Definition = definition;

        public bool InstanceOf(AbilityDefinition definition)
            => ReferenceEquals(Definition, definition);

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
            OnCancelAbility();
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

        protected virtual void OnGiveAbility()
        {
        }

        protected virtual void OnRemoveAbility()
        {
        }

        protected abstract void OnActivateAbility();

        protected virtual void OnCancelAbility()
            => OnEndAbility();

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

    public abstract class ActorAbility<T> : ActorAbility where T : AbilityDefinition
    {
        protected T Definition { get; private set; }

        public override void CreateAbility(AbilityDefinition definition)
        {
            base.CreateAbility(definition);

            Definition = (T)definition;
        }
    }
}