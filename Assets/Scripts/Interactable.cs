using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private CapsuleCollider cap_collider;
    [SerializeField] private GameObject ui;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject OnInteraction()
    {
        return (ui);
    }
}
