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

        public TrapView Create(TrapView prefab, TrapDefinition definition)
        {
            TrapView instance = Object.Instantiate(prefab);
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
            
            if (zone.Durability > 0) 
                zoneView.SetDurability(zone.Durability, zone.OnDestroyVFX);

            foreach (TrapZoneEffectBehaviour zoneBehaviour in zone.Behaviours)
            {
                var trap = new TrapZone(zoneBehaviour, _effectFactory);
                zoneView.Add(trap);    
            }
        }
    }
}