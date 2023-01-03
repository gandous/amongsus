using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerVote : MonoBehaviour
{
    public Color PlayerColor;
    public TMP_Text PlayerName;
    public Button RootButton;
    public Button CancelButton;
    public Button AcceptButton;
    public Vote _vote;
    public player_movement PlayerBind;

    // Start is called before the first frame update
    void Start()
    {
        RootButton.onClick.AddListener(RootButtonClicked);

        CancelButton.onClick.AddListener(CancelButtonClicked);
        AcceptButton.onClick.AddListener(AcceptButtonClicked);
    }

    private void AcceptButtonClicked()
    {

    }

    private void CancelButtonClicked()
    {
        AcceptButton.gameObject.SetActive(true);
        CancelButton.gameObject.SetActive(true);
    }

    private void RootButtonClicked()
    {
        AcceptButton.gameObject.SetActive(true);
        CancelButton.gameObject.SetActive(true);
    }
}
