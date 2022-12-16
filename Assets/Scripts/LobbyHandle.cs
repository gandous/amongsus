using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHandle : MonoBehaviour
{
    // Start is called before the first frame update
    public Button StartButton;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("a") == true)
            startGame();
    }

    private void startGame()
    {
        NetworkManager.singleton.ServerChangeScene("Game");
    }
}
