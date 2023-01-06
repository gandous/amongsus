using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private NetworkManager manager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnHostGameClicked()
    {
        Debug.Log("Host game");
        manager.StartHost(); // Client + Server
        // manager.StartServer(); Server only
    }

    public void OnJoinGameClicked()
    {
        if (string.IsNullOrEmpty(ipInput.text)) {
            Debug.Log("Empty ip address");
            return;
        }
        Debug.Log("Join game");
        Debug.Log(ipInput.text);
        manager.networkAddress = ipInput.text;
        manager.StartClient();
    }
}
