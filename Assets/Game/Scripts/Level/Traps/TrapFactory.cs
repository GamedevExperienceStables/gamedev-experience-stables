using Game.Actors;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Level
{
    public class TrapFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly EffectHandler _effectFactory;

        [Inject]
        public TrapFactory(IObjectResolver resolver, EffectHandler effectFactory)
        {
            _resolver = resolver;
            _effectFactory = effectFactory;
        }

        public TrapView Create(TrapDefinition definition)
        {
            TrapView instance = Object.Instantiate(definition.Prefab);
            _resolver.InjectGameObject(instance.gameObject);

            SetSize(instance, definition.Size);
            instance.Init(definition.Lifetime);

            HandleDefinition(definition, instance);
            
            return instance;
        }

        private static void SetSize(Component instance, float size) 
            => instance.transform.localScale = Vector3.one * size;

        private void HandleDefinition(TrapDefinition definition, Component view)
        {
            switch (definition)
            {
                case TrapZoneDefinition zone:
                    InitZone(view, zone);
                    break;
            }
        }

        private void InitZone(Component view, TrapZoneDefinition zone)
        {
            var zoneView = view.GetComponent<TrapZoneView>();
            var trap = new TrapZone(zone, _effectFactory);
            zoneView.Init(trap);
        }
    }
}