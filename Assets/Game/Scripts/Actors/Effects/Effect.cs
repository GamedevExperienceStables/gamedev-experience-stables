using Game.Utils.Factory;
using UnityEngine;

namespace Game.Actors
{
    public abstract class Effect : IRuntimeInstance<EffectDefinition>
    {
        private GameObject _view;

        public bool IsDone { get; private set; }

        public EffectDefinition Definition { get; private set; }
        public object Instigator { get; set; }

        public virtual void OnCreate(EffectDefinition definition)
            => Definition = definition;

        public void Execute(IActorController target)
        {
#if UNITY_EDITOR
            Debug.Log($"[Effect] Execute: {Definition.name} => {target.Transform.name}");
#endif
            AttachView(target);
            OnExecute(target);
        }

        public void Cancel()
        {
            if (IsDone)
                return;
            
#if UNITY_EDITOR
            Debug.Log($"[Effect] Cancel: {Definition.name}");
#endif

            IsDone = true;

            DestroyView();
            OnCancel();
        }

        private void AttachView(IActorController target)
        {
            if (!Definition.Status.StatusPrefab) 
                return;
            
            Vector3 position = target.Transform.position + Definition.Status.Offset;
            _view = Object.Instantiate(Definition.Status.StatusPrefab, position, Quaternion.identity, target.Transform);
        }

        private void DestroyView()
        {
            if (_view)
                Object.Destroy(_view);
        }

        protected abstract void OnExecute(IActorController target);

        protected abstract void OnCancel();
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