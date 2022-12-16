using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private List<player_movement> _players = new List<player_movement>();

    IEnumerator Start()
    {
        while (NetworkServer.active == false || _players.Count != NetworkServer.connections.Count || !_players.TrueForAll(x => x.connectionToClient.isReady))
            yield return null;
    }

  /*  public void AddPlayer(player_movement player)
    {
        _pl
    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
}
