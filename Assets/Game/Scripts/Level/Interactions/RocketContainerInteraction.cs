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

        public override void OnCreate()
            => _rocketContainer = Source.GetComponent<RocketContainer>();

        public override bool CanExecute()
            => _inventory.CanTransferToContainer(_levelMaterial);

        public override void Execute()
        {
            _inventory.TransferToContainer(_levelMaterial);

            _rocketContainer.TransferCompleted();
        }
    }
}