using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

   
    public  Dictionary<EntityRef, string> playerNames = new Dictionary<EntityRef, string>();

   
    public QuantumGame Game =>  QuantumRunner.DefaultGame;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

//         Game.Frames.Predicted.TryGet<Health>(player, out Health value);

        // var health =  value.CurrentHealth;

        QuantumEvent.Subscribe(listener: this, handler: (EventPlayerAdded e) => PlayerAdded(e.player));
    }

    void PlayerAdded(EntityRef player) {

        var frame = Game.Frames.Predicted;

        var gamedata = frame.GetSingleton<GameData>();

        
        var playerList = frame.ResolveList(gamedata.playerList);


        
        //playerNames.Clear();
       // UIManager.Instance.ClearName();
        foreach(var p in playerList)
        {

           frame.TryGet<PlayerLink>(p, out PlayerLink playerLink);

           var playerName = frame.GetPlayerData(playerLink.Player).PlayerNickname;

           if (playerNames.ContainsKey(p)) continue;

           playerNames.Add(p, playerName);
           UIManager.Instance.AddPlayerUi(playerName);
        }


          

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
