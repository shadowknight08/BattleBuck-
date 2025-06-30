namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerJoinSystem : SystemMainThread,ISignalOnPlayerConnected
    {
        public void OnPlayerConnected(Frame f, PlayerRef player)
        {
           var gameData = f.GetOrAddSingleton<GameData>();

            var list = f.ResolveList(gameData.playerList);
           
        }

        public override void Update(Frame frame)
        {
           
        }
    }
}
