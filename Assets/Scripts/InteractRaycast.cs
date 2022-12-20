using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractRaycast : MonoBehaviour
{
    private Camera cam;

    private GameObject obj;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && obj != null) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Destroy(obj);
            obj = null;
        }
        if (Input.GetButtonDown("Interact")) {
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 5)) {
                FirstPersonMovement move = cam.GetComponentInParent<FirstPersonMovement>();
                if (move != null) {
                    //move.disableCameraRot = true;
                    Debug.Log("Dsiable movement");
                }
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null) {
                    interaction(interactable);
                }
            } else {
            }

        }
    }

    void interaction(Interactable interactable)
    {
        obj = Instantiate(interactable.OnInteraction());
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
