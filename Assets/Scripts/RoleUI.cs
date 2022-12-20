using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoleUI : MonoBehaviour
{
    public TMP_Text Role;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnStartGame += OnGameStart;
    }

    private void OnDestroy()
    {
        GameManager.OnStartGame -= OnGameStart;
    }

    private void OnGameStart()
    {
        player_movement player = player_movement.Local;
        string role = player_movement.roleString[(int)player.role];
        Role.text = $"You are {role}";
        StartCoroutine(RemoveUI());
    }

    private IEnumerator RemoveUI()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}