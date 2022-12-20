using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role {
    Victime = 0,
    SUS = 1,
}

public class player_movement : NetworkBehaviour
{
    public static string[] roleString = {"Victime", "AmongSUSImposter"};
    public static player_movement Local;
    public TextMesh playerNameText;
    public GameObject floatingInfo;
    private Material playerMaterialClone;

    string Pname;
    Color Pcolor;
    [SyncVar]
    public Role role = Role.Victime;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    internal void SetColor(Color color)
    {
        playerColor = color;
        CmdSetupPlayer(Pname, color);
    }

    void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = playerName;
    }

    void OnColorChanged(Color _Old, Color _New)
    {
        playerNameText.color = _New;
        playerMaterialClone = new Material(GetComponent<Renderer>().material);
        playerMaterialClone.color = _New;
        GetComponent<Renderer>().material = playerMaterialClone;
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            Local = this;
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        if (GameManager.Instance)
            GameManager.Instance.AddPlayer(this);

        Pname = "bgDu" + UnityEngine.Random.Range(11, 99);
        floatingInfo.transform.localPosition = new Vector3(0, 1.2f, 0.2f);
        CmdSetupPlayer(Pname, Pcolor);
    }

    [Command]
    public void CmdSetupPlayer(string _name, Color _col)
    {
        playerName = _name;
        playerColor = _col;
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            floatingInfo.transform.LookAt(Camera.main.transform);
            return;
        }
        if (Input.GetKeyUp("t") == true)
        {
            print("bit");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
//                characterController = GetComponent<CharacterController>();
        }
    }
}