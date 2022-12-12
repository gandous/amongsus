using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{

    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private TMP_InputField nameInput;

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
    }

    public void OnJoinGameClicked()
    {
        if (string.IsNullOrEmpty(ipInput.text))
            return;
        Debug.Log("Join game");
        Debug.Log(ipInput.text);
    }
}
