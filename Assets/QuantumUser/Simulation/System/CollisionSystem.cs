namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CollisionSystem : SystemSignalsOnly,ISignalOnCollision2D
    {
        public void OnCollision2D(Frame f, CollisionInfo2D info)
        {
            // Projectile is colliding with something
            if (f.Unsafe.TryGetPointer<Projectile>(info.Entity, out var projectile))
            {
                if (f.Unsafe.TryGetPointer<Weapon>(info.Other, out var weapon))
                {
                    // Projectile Hit the player
                    f.Signals.OnCollisionProjectileHitPlayer(info, projectile, weapon);
                   
                }
                else if (f.Unsafe.TryGetPointer<Projectile>(info.Other, out var otherProjectile))
                {
                    // projectile Hit with projectile
                    f.Signals.OnCollisionProjectileHitProjectile(info, projectile, otherProjectile);
                }
            }

            // Ship is colliding with something
            else if (f.Unsafe.TryGetPointer<Weapon>(info.Entity, out var player))
            {
                if (f.Unsafe.TryGetPointer<Weapon>(info.Other, out var otherPlayer))
                {
                    // player collides with played
                    f.Signals.OnCollisionPlayerHitOtherPlayer(info, player, otherPlayer);
                }
            }
        }

       
        
    }
}
