using Cysharp.Threading.Tasks;
using Game.Inventory;
using Game.Level;
using Game.Utils;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Auto Pickup")]
    public class AutoPickupAbilityDefinition : AbilityDefinition<AutoPickupAbility>
    {
        [SerializeField, Range(0.01f, 5f)]
        private float pickupDistance = 3f;

        [SerializeField]
        private Vector3 offset;

        public float PickupDistance => pickupDistance;

        public Vector3 Offset => offset;
    }

    public class AutoPickupAbility : ActorAbility<AutoPickupAbilityDefinition>
    {
        private readonly IMagnetSystem _magnet;
        private readonly IInventoryItems _inventory;
        private ZoneTrigger _trigger;

        [Inject]
        public AutoPickupAbility(IMagnetSystem magnet, IInventoryItems inventory)
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
            => _trigger.TriggerEntered += OnTriggerEntered;

        protected override void OnEndAbility(bool wasCancelled)
            => _trigger.TriggerEntered -= OnTriggerEntered;

        private void OnTriggerEntered(GameObject other)
        {
            if (!other.TryGetComponent(out LootItem loot))
                return;

            if (!_inventory.CanAddToBag(loot.Definition.ItemDefinition, Owner))
                return;

            PickItemAsync(loot, Owner).Forget();
        }

        private async UniTask PickItemAsync(LootItem loot, IActorController target)
        {
            Transform targetTransform = target.Transform;
            await _magnet.StartPullAsync(loot.transform, targetTransform, Definition.Offset);

            if (_inventory.TryAddToBag(loot.Definition.ItemDefinition, target))
            {
                if (loot.Definition.PickupFeedback)
                    Object.Instantiate(loot.Definition.PickupFeedback,  targetTransform.TransformWithOffset(Definition.Offset), targetTransform.rotation);
                
                Object.Destroy(loot.gameObject);
            }
        }
    }
}