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
        Transform target_transform = target_player.GetComponent<Transform>();
        Transform source_transform = GetComponent<Transform>();
        if (Vector3.Distance(source_transform.position, target_transform.position) > InteractRaycast.InteractDistance)
            return;
        Debug.Log("kill");
        target_player.dead = true;
        Vector3 pos = target_transform.position;
        pos.y -= 0.5f;
        GameObject body = Instantiate(deadBody, pos, new Quaternion());
        Debug.Log(target_player.GetComponent<MeshFilter>());
        NetworkServer.Spawn(body);
        target_player.Dead();
        gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
        NetworkManager.Destroy(target_player.GetComponent<MeshFilter>());
        NetworkManager.Destroy(target_player.GetComponent<MeshRenderer>());
        target_player.dead = true;
    }
}
