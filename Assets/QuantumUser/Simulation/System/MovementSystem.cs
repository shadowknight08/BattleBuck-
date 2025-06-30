namespace Quantum
{
    using Photon.Deterministic;
    using System.Diagnostics;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter> , ISignalOnPlayerAdded,ISignalOnComponentAdded<PlayerLink>
    {
       

        public override void Update(Frame frame, ref Filter filter)
        {

            var input = frame.GetPlayerInput(filter.link->Player);

            var direction = input->Direction;

          
            if(direction.Magnitude > 1)
            {
                direction = direction.Normalized;
            }

            filter.Transform->Position += new FPVector2(direction.X, direction.Y);


            // Rotate if moving
            if (direction.SqrMagnitude > FP._0)
            {
                // Get angle in radians
                FP angleRad = FPMath.Atan2(direction.Y, direction.X);

                // Assign to Transform2D's Rotation (expects radians)
                filter.Transform->Rotation = angleRad;
            }

            // Weapon Control

            UpdateWeaponFire(frame, ref filter, input);
        }


        private void UpdateWeaponFire(Frame frame , ref Filter filter, Input* input)
        {
            var config = frame.FindAsset(filter.Weapon->weaponConfig);
            if(input->Fire && filter.Weapon->FireInterval <=0)
            {
                filter.Weapon->FireInterval = config.FireInterval;

                //ToDo create projectile

                var relativeOffset = FPVector2.Right * config.ShotOffset;
                var spawnPosition = filter.Transform->TransformPoint(relativeOffset);
                frame.Signals.WeaponProjectileShoot(filter.Entity, spawnPosition, config.bulletPrototype);
            }
            else
            {
                filter.Weapon->FireInterval -= frame.DeltaTime;
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public CharacterController2D* Kcc;
            public PlayerLink* link;
            public Weapon* Weapon;
        }


        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var runtimePlayer = f.GetPlayerData(player);
            var entity = f.Create(runtimePlayer.PlayerAvatar);

            var gameData = f.GetOrAddSingleton<GameData>();

            var list = f.ResolveList(gameData.playerList);

            list.Add(entity);

           

           
           var link = new PlayerLink()
           {
                Player = player
        };

           f.Add(entity, link);

           
        }

        public void OnAdded(Frame f, EntityRef entity, PlayerLink* component)
        {
            f.Events.PlayerAdded(entity);
        }
    }
}
