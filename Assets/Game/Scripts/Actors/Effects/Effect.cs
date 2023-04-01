using Game.Utils.Factory;
using UnityEngine;

namespace Game.Actors
{
    public abstract class Effect : IRuntimeInstance<EffectDefinition>
    {
        private GameObject _view;

        private bool _hasStatusView;

        public bool IsCanceled { get; private set; }

        public EffectDefinition Definition { get; private set; }
        public object Instigator { get; set; }

        public virtual void OnCreate(EffectDefinition definition)
        {
            Definition = definition;

            _hasStatusView = HasStatusView(definition.Status);

            Validate();
        }

        public void Execute(IActorController target)
        {
#if UNITY_EDITOR
            Debug.Log($"[Effect] Execute: {Definition.name} | {target.Transform.name}");
#endif
            if (_hasStatusView)
                AttachStatusView(target);

            OnExecute(target);
        }

        public void Replace(EffectDefinition definition, IActorController target)
        {
#if UNITY_EDITOR
            Debug.Log($"[Effect] Replace: {Definition.name} => {definition.name} | {target.Transform.name}");
#endif
            OnCancel();
            
            Definition = definition;
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
            DestroyView();
        }

        private void AttachStatusView(IActorController target)
        {
            Vector3 position = target.Transform.position + Definition.Status.Offset;
            _view = Object.Instantiate(Definition.Status.StatusPrefab, position, Quaternion.identity,
                target.Transform);
        }

        private void DestroyView()
        {
            if (_hasStatusView)
                Object.Destroy(_view);
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

        private static bool HasStatusView(StatusDefinition status)
            => status && status.StatusPrefab;
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