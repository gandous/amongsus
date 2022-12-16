using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotingRow : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _status;

    private int _actorNumber;
    public int ActorNumber {
        get { return _actorNumber; }
    }

    private Button _voteButton;
 //   private VoteStart _voteStart;

    private void Awake() {
        _voteButton = GetComponentInChildren<Button>();
        _voteButton.onClick.AddListener(OnVotePressed);
    }

    private void OnVotePressed() {
//        _voteStart.CastVote(_actorNumber);
    }
// Player player, qui manque
/*    public void Initialize(VoteStart voteStart) {
//        _actorNumber = player.ActorNumber;
//        _name.text = player.NickName;
        _status.text = "Waiting";
        _voteStart = voteStart;
    }*/

    public void UpdateStatus(string status) {
        _status.text = status;
    }

    public void StartVote(bool started) {
        _voteButton.interactable = started;
    }
}
