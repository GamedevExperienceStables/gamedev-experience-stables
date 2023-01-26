using Cysharp.Threading.Tasks;
using Game.Inventory;
using Game.Level;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "AutoPickupAbility")]
    public class AutoPickupAbilityDefinition : AbilityDefinition<AutoPickupAbility>
    {
        [field: SerializeField, Range(0.01f, 5f)]
        public float PickupDistance { get; set; } = 3f;
    }

    public class AutoPickupAbility : ActorAbility<AutoPickupAbilityDefinition>
    {
        private readonly IMagnetSystem _magnet;
        private readonly InventorySystem _inventory;
        private ZoneTrigger _trigger;

        [Inject]
        public AutoPickupAbility(IMagnetSystem magnet, InventorySystem inventory)
        {
            _magnet = magnet;
            _inventory = inventory;
        }

        protected override void OnInitAbility()
        {
            var view = Owner.GetComponent<AutoPickupAbilityView>();
            _trigger = view.Trigger;

            var sphereCollider = _trigger.GetComponent<SphereCollider>();
            sphereCollider.radius = Definition.PickupDistance;
        }

        protected override void OnGiveAbility()
        {
            base.OnGiveAbility();

            ActivateAbility();
        }

        public override bool CanActivateAbility()
            => !IsActive;

        protected override void OnActivateAbility()
        {
            _trigger.TriggerEntered += OnTriggerEntered;
        }

        protected override void OnEndAbility()
        {
            _trigger.TriggerEntered -= OnTriggerEntered;
        }

        private void OnTriggerEntered(GameObject other)
        {
            if (!other.TryGetComponent(out LootItem item))
                return;

            if (!_inventory.CanAddItem(item.Definition))
                return;

            PickItem(item).Forget();
        }

        private async UniTask PickItem(LootItem item)
        {
            Debug.Log($"Start picking! {item.name}");
            
            await _magnet.StartPullAsync(item.transform, Owner.Transform);

            // on magnet ends -> try execute effect
            if (_inventory.TryAddItem(item.Definition)) 
                Object.Destroy(item.gameObject);
        }
    }
}