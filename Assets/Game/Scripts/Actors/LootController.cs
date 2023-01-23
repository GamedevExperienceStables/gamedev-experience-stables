using Game.Actors.Health;
using Game.Level;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    [RequireComponent(typeof(DeathController))]
    public class LootController : MonoBehaviour
    {
        private LootBagDefinition _lootBag;
        private DeathController _deathController;
        private LootSpawner _lootSpawner;

        [Inject]
        private void Construct(LootSpawner lootSpawner) 
            => _lootSpawner = lootSpawner;

        private void Awake() 
            => _deathController = GetComponent<DeathController>();

        private void OnEnable() 
            => _deathController.Died += OnDied;

        private void OnDisable() 
            => _deathController.Died -= OnDied;

        public void SetLoot(LootBagDefinition lootBag) 
            => _lootBag = lootBag;

        private void OnDied() 
            => SpawnLoot();

        private void SpawnLoot() 
            => _lootSpawner.SpawnScattered(_lootBag, transform.position);
    }
}