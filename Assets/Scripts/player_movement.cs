using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Role {
    Victime = 0,
    SUS = 1,
}

public class PlayerInfo
{
    public Color PlayerColor;
}

public class player_movement : NetworkBehaviour
{
    public static string[] roleString = {"Victime", "AmongSUSImposter"};
    public static player_movement Local;
    public TextMesh playerNameText;
    public GameObject floatingInfo;
    private Material playerMaterialClone;
    [SyncVar]
    public List<string> taskList;

    string Pname;
    Color Pcolor;
    [SyncVar]
    public Role role = Role.Victime;

    [SyncVar]
    public bool dead = false;
    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    [SerializeField] public GameObject taskInfoObject;
    public TaskInfo taskInfo;

    public event Action<player_movement> OnPlayerVote;

    internal void SetColor(Color color)
    {
        PlayerInfo infos = (PlayerInfo)connectionToClient.authenticationData;
        if(infos == null)
        {
            infos = new PlayerInfo();
            connectionToClient.authenticationData = infos;
            ((PlayerInfo)connectionToClient.authenticationData).PlayerColor = color;
        }
        playerColor = color;
    }

    void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = playerName;
    }

    void OnColorChanged(Color _Old, Color _New)
    {
        playerNameText.color = _New;
        playerMaterialClone = new Material(GetComponent<Renderer>().material);
        playerMaterialClone.color = _New;
        GetComponent<Renderer>().material = playerMaterialClone;
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            Local = this;
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
            GameObject obj = Instantiate(taskInfoObject);
            taskInfo = obj.GetComponent<TaskInfo>();
        }

        if (GameManager.Instance)
            GameManager.Instance.AddPlayer(this);

        if (isClient && connectionToClient != null && connectionToClient.authenticationData != null)
        {
            PlayerInfo infos = (PlayerInfo)connectionToClient.authenticationData;
            if(infos != null)
            {
                playerColor = infos.PlayerColor;
            }
        }
        Pname = "bgDu" + UnityEngine.Random.Range(11, 99);
        floatingInfo.transform.localPosition = new Vector3(0, 1.2f, 0.2f);
        playerName = Pname;
    }

    [ClientRpc]
    public void Dead()
    {
        Debug.Log("Dead");
        gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
        playerNameText.gameObject.SetActive(false);
        NetworkManager.Destroy(GetComponent<MeshFilter>());
        NetworkManager.Destroy(GetComponent<MeshRenderer>());
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            floatingInfo.transform.LookAt(Camera.main.transform);
            return;
        }
    }

    [ClientRpc]
    public void UpdateCompleteTask(int complete, int total)
    {
        if (isLocalPlayer) {
            taskInfo.UpdateSlider(complete, total);
            if (complete == total) {
                SceneManager.LoadScene("CrewmateWin");
                Destroy(GameObject.FindWithTag("DontDestroyOnLoad"));
                Destroy(GameObject.FindWithTag("GameManager"));
            }
        }
    }

    public void TaskComplete(string id)
    {
        taskList.Remove(id);
        taskInfo.UpdateText(taskList);
        SendTaskComplete();
    }

    [Command]
    private void SendTaskComplete()
    {
        GameManager.Instance.TaskComplete();
    }

    [ClientRpc]
    public void TaskListChanged(List<string> _new)
    {
        if (isLocalPlayer) {
            taskInfo.UpdateText(_new);
        }
    }

    [Command]
    public void Report()
    {
        GameManager.Instance.ReportPlayerServer(this);
        this.RpcReport(this);
    }

    [ClientRpc]
    internal void RpcReport(player_movement reportedBy)
    {
        GameManager.Instance.ReportPlayerClient(reportedBy);
    }

    [Command]
    public void CmdVote(player_movement playerVoted)
    {
        if(GameManager.Instance.Vote(this, playerVoted))
        {
            RpcVote(playerVoted);
            GameManager.Instance.CheckEndVotes();
        }
    }

    [ClientRpc]
    public void RpcVote(player_movement playerVoted)
    {
        GameManager.Instance.Vote(this, playerVoted);
        OnPlayerVote?.Invoke(playerVoted);
    }
}