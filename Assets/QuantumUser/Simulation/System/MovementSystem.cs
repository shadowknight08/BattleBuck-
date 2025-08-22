namespace Quantum
{
    using Photon.Deterministic;
    using System.Diagnostics;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>, ISignalOnPlayerAdded, ISignalOnComponentAdded<PlayerLink>
    {
        public override void Update(Frame frame, ref Filter filter)
        {
            // Get player input
            var input = frame.GetPlayerInput(filter.link->Player);
            if (input == null) return; // Defensive: skip if input is missing

            var direction = input->Direction;
            if (direction.Magnitude > 1)
            {
                direction = direction.Normalized;
            }

            // Move entity
            filter.Transform->Position += new FPVector2(direction.X, direction.Y);

            // Rotate if moving
            if (direction.SqrMagnitude > FP._0)
            {
                FP angleRad = FPMath.Atan2(direction.Y, direction.X);
                filter.Transform->Rotation = angleRad;
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
            // Create player entity and add to player list
            var runtimePlayer = f.GetPlayerData(player);
            var entity = f.Create(runtimePlayer.PlayerAvatar);
            var gameData = f.GetOrAddSingleton<GameData>();
            var list = f.ResolveList(gameData.playerList);
            list.Add(entity);

            // Link player to entity
            var link = new PlayerLink()
            {
                Player = player
            };
            f.Add(entity, link);
        }

        public void OnAdded(Frame f, EntityRef entity, PlayerLink* component)
        {
            // Raise PlayerAdded event for view/UI
            f.Events.PlayerAdded(entity);
        }
    }
}
