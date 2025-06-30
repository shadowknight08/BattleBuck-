namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class ProjectileSystem : SystemMainThreadFilter<ProjectileSystem.Filter>,ISignalWeaponProjectileShoot,ISignalOnCollisionProjectileHitPlayer,ISignalOnCollisionProjectileHitProjectile
    {
        public void WeaponProjectileShoot(Frame frame ,EntityRef owner, FPVector2 spawnPosition, AssetRef<EntityPrototype> projectilePrototype)
        {
            EntityRef projectileEntity = frame.Create(projectilePrototype);
            Transform2D* projectileTransform = frame.Unsafe.GetPointer<Transform2D>(projectileEntity);
            Transform2D* ownerTransform = frame.Unsafe.GetPointer<Transform2D>(owner);


            projectileTransform->Rotation = ownerTransform->Rotation;
            projectileTransform->Position = spawnPosition;

            Projectile* projectile = frame.Unsafe.GetPointer<Projectile>(projectileEntity);
            var config = frame.FindAsset(projectile->ProjectileConfig);
            projectile->TTl = config.ProjectileTTL;
            projectile->Owner = owner;

            PhysicsBody2D* body = frame.Unsafe.GetPointer<PhysicsBody2D>(projectileEntity);
            body->Velocity = ownerTransform->Right * config.ProjectileInitialSpeed;
        }


        public struct Filter
        {
            public EntityRef Entity;
            public Projectile* Projectile;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            filter.Projectile->TTl -= frame.DeltaTime;
            if (filter.Projectile->TTl <= 0)
            {
                frame.Destroy(filter.Entity);
            }
        }

        public void OnCollisionProjectileHitPlayer(Frame f, CollisionInfo2D info, Projectile* projectile, Weapon* player)
        {
            Log.Debug("Bullet hit the player");
            if (projectile->Owner == info.Other)
            {
                info.IgnoreCollision = true;
                return;
            }

            f.Unsafe.TryGetPointer<Health>(info.Other, out var health);

            health--;

            Log.Debug("Bullet hit the player");
            f.Destroy(info.Entity);
        }

        public void OnCollisionProjectileHitProjectile(Frame f, CollisionInfo2D info, Projectile* projectile, Projectile* otherProjectile)
        {
            f.Destroy(info.Entity);
        }
    }
}
