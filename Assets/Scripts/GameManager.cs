using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public struct StartMessage : NetworkMessage
{

}

public struct ClearMessage : NetworkMessage
{

}

public struct VoteDeadMessage : NetworkMessage
{
    public player_movement PlayerDead;
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
    public static Action OnClearPlayers;
    public static event Action OnVoteEnd;
    public static event Action<player_movement> OnPlayerReport;
    public int totalTask = 0;
    public int totalTaskComplete = 0;

    void Start()
    {
        DontDestroyOnLoad(this);
        NetworkClient.RegisterHandler<StartMessage>(OnStartMessage);
        NetworkClient.RegisterHandler<ClearMessage>(OnClearMessage);
        NetworkClient.RegisterHandler<VoteDeadMessage>(OnVoteDeadReceived);
    }

    public void StartGame()
    {
        NetworkServer.SendToReady(new ClearMessage());
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

    private void OnClearMessage(ClearMessage message)
    {
        ClearPlayer();
        OnClearPlayers?.Invoke();
    }

    private void OnVoteDeadReceived(VoteDeadMessage message)
    {
        _votes.Clear();
        OnVoteEnd?.Invoke();
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
        print("testVotes");
        if (voted == null && voter.dead == false && _votes.FindIndex(x => x.Voter == voter) == -1)
        {
            _votes.Add(new PlayerVoteStruct {Voter = voter, Voted = null});
            return true;
        }
        else if(voter.dead == false && voted.dead == false && _votes.FindIndex(x => x.Voter == voter) == -1)
        {
            _votes.Add(new PlayerVoteStruct {Voter = voter, Voted = voted});
            return true;
        }
        return false;
    }

    internal void ReportPlayerServer(player_movement reportedBy)
    {
        foreach(player_movement player in _players)
        {
            if(player.dead == false)
            {
                Transform startPos = NetworkManager.singleton.GetStartPosition();
            }
        }
    }

    internal void ReportPlayerClient(player_movement deadPlayer)
    {
        OnPlayerReport?.Invoke(deadPlayer);
    }

    internal void CheckEndVotes()
    {
        int alivePlayers = _players.Where(s => s.dead == false).Count();
        print(alivePlayers);
        print(_votes.Count());
        if(alivePlayers == _votes.Count)
        {
            print("testccsdcez");

            Dictionary<player_movement, int> votes = new Dictionary<player_movement, int>();
            int skips = 0;
            foreach(PlayerVoteStruct vote in _votes)
            {
                if (vote.Voted == null)
                    skips += 1;
                else if (votes.ContainsKey(vote.Voted))
                {
                    ++votes[vote.Voted];
                }
                else
                {
                    votes.Add(vote.Voted, 1);
                }
            }
            if (votes.Count < 1) {
                _votes.Clear();
                NetworkServer.SendToAll(new VoteDeadMessage { PlayerDead = null });
            }
            else
            {
                List<KeyValuePair<player_movement, int>> order = votes.OrderByDescending(x => x.Value).ToList();
                if (order[0].Value <= skips) {
                    _votes.Clear();
                    NetworkServer.SendToAll(new VoteDeadMessage { PlayerDead = null });
                }
                else if(order.Count() <= 1 || order[0].Value != order[1].Value)
                {
                    player_movement player = order[0].Key;
                    player_movement useless = player_movement.Local;
                    useless.GetComponent<Kill>().makeKill(player.playerName, true);
                    _votes.Clear();
                    NetworkServer.SendToAll(new VoteDeadMessage { PlayerDead = player });
                }
                else
                {
                    _votes.Clear();
                    NetworkServer.SendToAll(new VoteDeadMessage { PlayerDead = null });
                }
            }
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("DeadBody")) {
            Destroy(obj);
        }
    }

    public void CheckWin()
    {
        int aliveImposter = _players.Where(s => s.role == Role.SUS && s.dead == false ).Count();
        int alivePlayer = _players.Where(s => s.dead == false ).Count();
        alivePlayer -= aliveImposter;

        if (alivePlayer <= 0) {
            ImposterWin();
        } else if (aliveImposter == 0) {
            CrewmateWin();
        }
    }

    public void CrewmateWin()
    {
        NetworkManager.singleton.ServerChangeScene("CrewmateWin");
    }

    public void ImposterWin()
    {
        NetworkManager.singleton.ServerChangeScene("ImposterWin");
    }
}
