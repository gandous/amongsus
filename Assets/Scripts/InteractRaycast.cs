using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractRaycast : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
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
                    interactable.OnInteraction();
                }
            } else {
            }

        }
    }
}
