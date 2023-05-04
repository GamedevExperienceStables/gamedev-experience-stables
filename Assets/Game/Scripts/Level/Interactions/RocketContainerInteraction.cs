using Game.Inventory;
using Game.Settings;
using VContainer;

namespace Game.Level
{
    public class RocketContainerInteraction : Interaction
    {
        private readonly IInventoryItems _inventory;
        private readonly MaterialDefinition _levelMaterial;

        private RocketContainer _rocketContainer;

        [Inject]
        public RocketContainerInteraction(LevelController level, IInventoryItems inventory)
        {
            LevelDefinition currentLevel = level.GetCurrentLevel();
            _levelMaterial = currentLevel.Goal.Material;

            _inventory = inventory;
        }

        public MaterialDefinition Material => _rocketContainer.TargetMaterial;

        public override void OnCreate()
            => _rocketContainer = Source.GetComponent<RocketContainer>();

        public override bool CanExecute()
            => CanExecuteWithResult(out _);

        public bool CanExecuteWithResult(out InteractionRocketResult result)
        {
            if (_inventory.IsContainerFull(_rocketContainer.TargetMaterial))
            {
                result = InteractionRocketResult.Full;
                return false;
            }

            if (_inventory.IsBagEmpty(_rocketContainer.TargetMaterial))
            {
                result = InteractionRocketResult.Empty;
                return false;
            }

            result = InteractionRocketResult.Ok;
            return true;
        }


        public override void Execute()
        {
            _inventory.TransferToContainer(_levelMaterial);

            _rocketContainer.TransferComplete();
        }
    }
}