using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Vote : MonoBehaviour
{
    public CanvasGroup Group;
    public GridLayoutGroup LayoutGroup;
    public PlayerVote PlayerPrefab;
    private List<PlayerVote> _players = new List<PlayerVote>();

    // Start is called before the first frame update
    void Start()
    {
        SetGroupActive(false);
    }

    private void OnEnable()
    {
//        GameManager.OnPlayerReport += OnPlayerReport;
    }

    private void OnDisable()
    {
//        GameManager.OnPlayerReport -= OnPlayerReport;
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
        playersList.Sort((a,b) => a.dead == false ? 1 : -1);
        foreach(player_movement player in GameManager.Instance.Players)
        {
/*            PlayerVote playerUI = Instantiate(PlayerVote, LayoutGroup.transform);
            playerUI._vote = this;
            playerUI.PlayerBind = player;
            playerUI.RootButton.interactable = player.dead == false;*/

//            _players.Add(playerUI);
        }
    }

    internal void SetGroupActive(bool state)
    {
        Group.alpha = state ? 1 : 0;
        Group.interactable = state;
        Group.blocksRaycasts = state;
    }
}
