using System;
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
    public List<player_movement> Players => _players;

    public static Action OnStartGame;

    void Start()
    {
        DontDestroyOnLoad(this);
        NetworkClient.RegisterHandler<StartMessage>(OnStartMessage);
    }

    public void StartGame()
    {
        StartCoroutine("StartGameCoro");
    }

    IEnumerator StartGameCoro()
    {
        while (NetworkServer.active == false)
            yield return null;

        while (_players.Count != NetworkServer.connections.Count || !_players.TrueForAll(x => x.connectionToClient.isReady))
            yield return null;

        int role = UnityEngine.Random.Range(0, _players.Count);
        _players[role].role = Role.SUS;

        yield return new WaitForSeconds(1);

        NetworkServer.SendToReady(new StartMessage());
    }

    public void AddPlayer(player_movement player)
    {
        print("player added");
        _players.Add(player);
    }

    public void ClearPlayer()
    {
        for (int i = 0; i < _players.Count; i++) {
            NetworkServer.Destroy(_players[i].gameObject);
        }
        _players.Clear();
    }

    private void OnStartMessage(StartMessage message)
    {
        OnStartGame?.Invoke();
    }
}
