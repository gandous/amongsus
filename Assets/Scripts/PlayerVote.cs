using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerVote : MonoBehaviour
{
    public Color PlayerColor;
    public TMP_Text PlayerName;
    public Button AcceptButton;
    public Vote _vote;
    public player_movement PlayerBind;

    // Start is called before the first frame update
    void Start()
    {
        AcceptButton.gameObject.SetActive(true);
        AcceptButton.onClick.AddListener(AcceptButtonClicked);
        PlayerBind.OnPlayerVote += OnPlayerVote;
    }

    private void AcceptButtonClicked()
    {
        PlayerBind.Local.CmdVote(player_movement);
    }

    private void OnPlayerVote(player_movement playerAimed)
    {
        
    }
}
