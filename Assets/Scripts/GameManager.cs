using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct StartMessage : NetworkMessage
{

}

public struct PlayerVoteStruct
{
    public player_movement Voter;
    public player_movement Voted;
}

public class GameManager : MonoBehaviour
{
    const int TaskPerUser = 2;
    public static GameManager Instance;


    [SerializeField]
    public GameObject taskLists;
    private void Awake()
    {
        Instance = this;
    }

    private List<player_movement> _players = new List<player_movement>();
    public List<player_movement> Players => _players;
    private List<PlayerVoteStruct> _votes = new List<PlayerVoteStruct>();

    public static Action OnStartGame;
    public static Action<player_movement> OnPlayerReport;
    public int totalTask = 0;
    public int totalTaskComplete = 0;

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
        AsignTask();
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

    public void TaskComplete()
    {
        totalTaskComplete++;
        SendTaskUpdate();
    }

    private void SendTaskUpdate()
    {
        for (int i = 0; i < _players.Count; i++) {
            _players[i].UpdateCompleteTask(totalTaskComplete, totalTask);
        }
    }

    private void AsignTask()
    {
        Transform tasks = taskLists.transform;
        int differentTaskCount = tasks.childCount;
        for (int i = 0; i < _players.Count; i++) {
            if (_players[i].role == Role.SUS)
                continue;
            List<string> playerTasks = new List<string>();
            while (_players[i].taskList.Count < TaskPerUser) {
                string id = tasks.GetChild(UnityEngine.Random.Range(0, differentTaskCount)).GetComponent<Interactable>().id;
                if (!playerTasks.Contains(id))
                    playerTasks.Add(id);
                _players[i].taskList = playerTasks;
                _players[i].TaskListChanged(playerTasks);
            }
            totalTask += playerTasks.Count;
        }
        SendTaskUpdate();
    }

    internal bool Vote(player_movement voter, player_movement voted)
    {
        if(voter.dead == false && voted.dead == false && _votes.FindIndex(x => x.Voter == voter) == -1)
        {
            _votes.Add(new PlayerVoteStruct {Voter = voter, Voted = voted});
            return true;
        }
        return false;
    }

    internal void CheckEndVotes()
    {
        List<player_movement> alivePlayers = new List<player_movement>();;
        _players.CopyTo(alivePlayers);
        alivePlayers.Sort((a,b) => a.dead == false ? -1 : 1);
        if(alivePlayers.Count == _votes.Count)
        {
            //faireregfrsf fin
        }
    }
}
