namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    // This system handles player connection and adds them to the GameData player list.
    // If you need to spawn player entities or initialize player data, do it here.
    public unsafe class PlayerJoinSystem : SystemMainThread, ISignalOnPlayerConnected
    {
        public void OnPlayerConnected(Frame f, PlayerRef player)
        {
            var gameData = f.GetOrAddSingleton<GameData>();
            var list = f.ResolveList(gameData.playerList);
            // Add logic here if you want to spawn a player entity or initialize player data.
            // Example: spawn player entity, assign components, etc.
            // list.Add(playerEntity);
        }

        public override void Update(Frame frame)
        {
            // No per-frame logic needed for player join. Consider merging this with a broader player management system.
        }
    }
}
