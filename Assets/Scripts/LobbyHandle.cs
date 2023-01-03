using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHandle : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("a") == true && isServer) {
            GameManager.Instance.ClearPlayer();
            GameManager.Instance.StartGame();
            startGame();
        }
    }

    private void startGame()
    {
        NetworkManager.singleton.ServerChangeScene("Game");
    }
}
