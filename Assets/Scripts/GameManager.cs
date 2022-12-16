using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    IEnumerator Start()
    {
        while (NetworkServer.active != true)
            yield return null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
