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

    private Color color;
    private GameObject colorPreview;

    // Start is called before the first frame update
    void Start()
    {
        color = Random.ColorHSV();
        colorPreview = GameObject.Find("color/Panel");
        Image img = colorPreview.GetComponent<Image>();
        img.color = color;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChooseColorButtonClick()
    {
        ColorPicker.Create(color, "Choose your color", SetColor, ColorFinished, true);
    }

    private void SetColor(Color currentColor)
    {
        color = currentColor;
    }

    private void ColorFinished(Color finishedColor)
    {
        Image img = colorPreview.GetComponent<Image>();
        img.color = finishedColor;
        Debug.Log("You chose the color " + ColorUtility.ToHtmlStringRGBA(finishedColor));
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
