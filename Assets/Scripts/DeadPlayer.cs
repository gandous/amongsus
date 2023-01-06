using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeadPlayer : NetworkBehaviour
{
    [SyncVar] public string playerName;
    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnColorChanged(Color _Old, Color _New)
    {
        Material mat = new Material(GetComponent<Renderer>().material);
        mat.color = _New;
        GetComponent<Renderer>().material = mat;
    }
}
