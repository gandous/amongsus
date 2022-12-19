using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private CapsuleCollider cap_collider;
    [SerializeField] private GameObject ui;

    private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && obj != null) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(obj);
            obj = null;
        }
    }

    public void OnInteraction()
    {
        obj = Instantiate(ui);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Task task = obj.GetComponent<Task>();
        if (task) {
            task.OnTaskComplete.AddListener(InteractionComplete);
        }
    }

    void InteractionComplete()
    {
        Debug.Log("Complete");
    }
}
