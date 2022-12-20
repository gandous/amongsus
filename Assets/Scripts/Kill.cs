using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Kill : NetworkBehaviour
{
    [SerializeField] private GameObject deadBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    public void makeKill(string playerName)
    {
        int i = 0;
        for (; i < GameManager.Instance.Players.Count && GameManager.Instance.Players[i].playerName != playerName; i++);
        player_movement target_player = GameManager.Instance.Players[i];
        if (target_player.dead == true)
            return;
        Debug.Log("kill");
        Transform target_transform = target_player.GetComponent<Transform>();
        Vector3 pos = target_transform.position;
        pos.y -= 0.5f;
        GameObject body = Instantiate(deadBody, pos, new Quaternion());
        //NetworkManager.Destroy(target_player.GetComponent<MeshFilter>());
        //NetworkManager.Destroy(target_player.GetComponent<MeshRenderer>());
        NetworkServer.Spawn(body);
        target_player.Dead();
        target_player.dead = true;
    }
}
