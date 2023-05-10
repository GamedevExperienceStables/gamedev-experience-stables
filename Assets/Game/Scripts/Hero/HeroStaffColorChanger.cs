using System;
using Game.Inventory;
using Game.Player;
using VContainer;
using VContainer.Unity;

namespace Game.Hero
{
    public sealed class HeroStaffColorChanger : IStartable, IDisposable
    {
        private readonly IInventorySlots _slots;
        private readonly PlayerController _player;

        private HeroStaffView _staff;

        [Inject]
        public HeroStaffColorChanger(PlayerController player, IInventorySlots slots)
        {
            _slots = slots;
            _player = player;
        }
        
        public void Start()
        {
            _player.HeroBound += OnHeroBound;
            _player.HeroUnBound += OnHeroUnbound;
            
            _slots.ActiveSlotChanged += OnActiveSlotChanged;
        }

        public void Dispose()
        {
            _player.HeroBound -= OnHeroBound;
            _player.HeroUnBound -= OnHeroUnbound;
            
            _slots.ActiveSlotChanged -= OnActiveSlotChanged;
        }

        private void OnHeroBound(HeroController hero) 
            => _staff = hero.GetComponent<HeroStaffView>();

        private void OnHeroUnbound() 
            => _staff = null;

        private void OnActiveSlotChanged(RuneActiveSlotChangedEvent obj)
        {
            if (_slots.HasActive)
                SetColor(_slots.ActiveSlot.Rune);
            else
                ResetColor();
        }

        private void SetColor(RuneDefinition rune)
            => _staff.SetColor(rune.Color);

        private void ResetColor()
            => _staff.ResetColor();
    }
}