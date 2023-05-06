using Game.Utils.Factory;
using UnityEngine;

namespace Game.Actors
{
    public abstract class Effect : IRuntimeInstance<EffectDefinition>
    {
        public bool IsCanceled { get; private set; }

        public EffectDefinition Definition { get; private set; }
        public object Instigator { get; set; }

        protected bool IsSuppressed { get; private set; }

        public virtual void OnCreate(EffectDefinition definition)
        {
            Definition = definition;

            Validate();
        }

        public void Execute(IActorController target)
        {
#if UNITY_EDITOR
            Debug.Log($"[Effect] Execute: {Definition.name} | {target.Transform.name}");
#endif
            OnExecute(target);
        }

        public void Cancel()
        {
            if (IsCanceled)
                return;

#if UNITY_EDITOR
            Debug.Log($"[Effect] Cancel: {Definition.name}");
#endif

            IsCanceled = true;
            OnCancel();
        }

        private void Validate()
        {
            if (Definition.EffectType == EffectDuration.Limited)
            {
                float duration = Definition.Duration;
                Debug.Assert(duration > 0,
                    $"{Definition.name}: type is {nameof(EffectDuration.Limited)} and {nameof(duration)} must be greater than 0",
                    context: Definition);
            }
        }

        protected abstract void OnExecute(IActorController target);

        protected abstract void OnCancel();

        public void Suppress()
        {
            if (IsSuppressed)
                return;
            
#if UNITY_EDITOR
            Debug.Log($"[Effect] Suppress: {Definition.name}");
#endif
            IsSuppressed = true;
            
            OnSuppress();
        }

        public void UnSuppress()
        {
            if (!IsSuppressed)
                return;
            
#if UNITY_EDITOR
            Debug.Log($"[Effect] Unsuppress: {Definition.name}");
#endif
            IsSuppressed = false;
            
            OnUnSuppress();
        }

        protected virtual void OnSuppress()
        {
        }
        
        protected virtual void OnUnSuppress()
        {
        }
    }

    public abstract class Effect<T> : Effect where T : EffectDefinition
    {
        protected new T Definition { get; private set; }

        public override void OnCreate(EffectDefinition definition)
        {
            base.OnCreate(definition);

            Definition = (T)definition;
        }
    }
}