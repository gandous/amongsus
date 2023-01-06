using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class Chat : NetworkBehaviour
{
    [SerializeField] private TMP_InputField msg;
    [SerializeField] private TMP_Text history;
    [SerializeField] private Scrollbar scroll;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SendPressed()
    {
        ClientSendMsg(player_movement.Local.playerName + ": " + msg.text);
        msg.text = "";
    }

    [Command(requiresAuthority=false)]
    private void ClientSendMsg(string msg)
    {
        ReceiveMsg(msg);
    }

    [ClientRpc]
    private void ReceiveMsg(string msg)
    {
        history.text += msg + "\n";
    }
}
