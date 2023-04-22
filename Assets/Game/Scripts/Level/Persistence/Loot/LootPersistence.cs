using CleverCrow.Fluid.UniqueIds;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(LootItem))]
    [RequireComponent(typeof(UniqueId))]
    public class LootPersistence : MonoBehaviour, ILocationPersistenceLoot
    {
        private LootItem _item;
        private UniqueId _uid;

        public string Id => _uid.Id;

        public LocationLootData Value => new(_item.Definition, _item.transform.position);

        private void Awake()
        {
            _uid = GetComponent<UniqueId>();
            _uid.PopulateIdIfEmpty();
            
            _item = GetComponent<LootItem>();
        }
    }
}