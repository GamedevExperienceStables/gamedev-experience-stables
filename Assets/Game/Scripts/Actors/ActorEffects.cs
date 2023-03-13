using System.Collections.Generic;

namespace Game.Actors
{
    public class ActorEffects
    {
        private readonly List<Effect> _effects = new();

        public void Add(Effect effect, IActorController target)
        {
            _effects.Add(effect);

            effect.Execute(target);
        }

        public void Cancel(StatusDefinition status)
        {
            foreach (Effect effect in _effects)
            {
                if (effect.Definition.Status == status)
                    effect.Cancel();
            }
        }

        public void CancelAll(object instigator)
        {
            foreach (Effect effect in _effects)
                if (effect.Instigator == instigator)
                    effect.Cancel();
        }

        public void CancelAll()
        {
            foreach (Effect effect in _effects)
                effect.Cancel();

            _effects.Clear();
        }

        public void Tick()
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                if (_effects[i].IsDone)
                    _effects.RemoveAt(i);
            }
        }
    }
}