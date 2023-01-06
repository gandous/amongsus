using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Vote : MonoBehaviour
{
    public CanvasGroup Group;
    public VerticalLayoutGroup LayoutGroup;
    public PlayerVote PlayerPrefab;
    private List<PlayerVote> _players = new List<PlayerVote>();
    public Button SkipButton;

    // Start is called before the first frame update
    void Start()
    {
        SkipButton.onClick.AddListener(SkipButtonClicked);
        SetGroupActive(false);
    }

    private void OnEnable()
    {
        GameManager.OnPlayerReport += OnPlayerReport;
        GameManager.OnVoteEnd += OnVoteEnd;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerReport -= OnPlayerReport;
        GameManager.OnVoteEnd -= OnVoteEnd;
    }
    // Update is called once per frame
    private void OnPlayerReport(player_movement playerReported)
    {
        SetGroupActive(true);
        foreach (Transform t in LayoutGroup.transform)
        {
            Destroy(t.gameObject);
        }
        _players.Clear();
        List<player_movement> playersList = GameManager.Instance.Players;
        playersList.Sort((a,b) => a.dead == false ? -1 : 1);
        foreach(player_movement player in GameManager.Instance.Players)
        {
            PlayerVote playerUI = Instantiate(PlayerPrefab, LayoutGroup.transform);
            playerUI._vote = this;
            playerUI.PlayerBind = player;
            playerUI.PlayerName.text = player.name;
            playerUI.AcceptButton.interactable = player.dead == false;

            _players.Add(playerUI);
        }
    }

    private void OnVoteEnd()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetGroupActive(false);
    }

    internal void SetGroupActive(bool state)
    {
        Group.alpha = state ? 1 : 0;
        Group.interactable = state;
        Group.blocksRaycasts = state;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void SkipButtonClicked()
    {
        player_movement.Local.CmdVote(null);
    }
}
