using Game.Actors;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    [RequireComponent(typeof(TrapZoneView))]
    public class TrapZoneStatic : MonoBehaviour
    {
        [SerializeField, Expandable]
        private TrapZoneEffectBehaviours behaviours;

        private EffectHandler _effectHandler;
        private TrapZoneView _view;

        [Inject]
        public void Construct(EffectHandler effectFactory)
            => _effectHandler = effectFactory;

        private void Awake()
            => _view = GetComponent<TrapZoneView>();

        private void Start()
        {
            foreach (TrapZoneEffectBehaviour effectDefinition in behaviours.Behaviours)
            {
                var trap = new TrapZone(effectDefinition, _effectHandler);
                _view.Add(trap);    
            }
        }
    }
}