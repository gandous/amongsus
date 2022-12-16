using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct StartMessage : NetworkMessage
{

}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private List<player_movement> _players = new List<player_movement>();

    public static Action OnStartGame;
    IEnumerator Start()
    {
        NetworkClient.RegisterHandler<StartMessage>(OnStartMessage);
        while (NetworkServer.active == false || _players.Count != NetworkServer.connections.Count || !_players.TrueForAll(x => x.connectionToClient.isReady))
            yield return null;

        int role = UnityEngine.Random.Range(0, _players.Count);
        _players[role].role = "SUSSUSIMPOSTER";

        NetworkServer.SendToReady(new StartMessage());
    }
    
    public void AddPlayer(player_movement player)
    {
        _players.Add(player);
    }

    private void OnStartMessage(StartMessage message)
    {
        OnStartGame?.invoke();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
