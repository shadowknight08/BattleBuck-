namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    // Handles weapon switching, firing, and cooldowns for multiple weapon types
    public unsafe class WeaponSystem : SystemMainThreadFilter<WeaponSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Weapon* Weapon;
            public WeaponInventory* Inventory;
            public PlayerLink* Link;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var input = frame.GetPlayerInput(filter.Link->Player);
            if (input == null) return;

            // Weapon switching
            if (input->SwitchWeapon)
            {
                SwitchWeapon(frame, ref filter);
            }

            // Weapon firing
            var config = frame.FindAsset(filter.Weapon->weaponConfig);
            if (input->Fire && filter.Weapon->FireInterval <= 0)
            {
                filter.Weapon->FireInterval = config.FireInterval;
                // Spawn projectile via signal
                var relativeOffset = FPVector2.Right * config.ShotOffset;
                var spawnPosition = frame.Unsafe.GetPointer<Transform2D>(filter.Entity)->TransformPoint(relativeOffset);
                frame.Signals.WeaponProjectileShoot(filter.Entity, spawnPosition, config.bulletPrototype);
            }
            else
            {
                filter.Weapon->FireInterval -= frame.DeltaTime;
            }
        }

        private void SwitchWeapon(Frame frame, ref Filter filter)
        {
            // Example: cycle through inventory
            var inventory = filter.Inventory;
            if (inventory == null || inventory->Count == 0) return;
            inventory->CurrentIndex = (inventory->CurrentIndex + 1) % inventory->Count;
            var newWeaponConfig = inventory->WeaponConfigs[inventory->CurrentIndex];
            filter.Weapon->weaponConfig = newWeaponConfig;
            frame.Events.WeaponSwitch(filter.Entity, newWeaponConfig);
        }
    }
} 