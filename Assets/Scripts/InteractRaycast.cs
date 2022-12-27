using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class InteractRaycast : MonoBehaviour
{
    public const float InteractDistance = 5.0f;
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
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, InteractDistance)) {
                FirstPersonMovement move = cam.GetComponentInParent<FirstPersonMovement>();
                player_movement player = cam.GetComponentInParent<player_movement>();

                /*if (player.role == Role.SUS) {
                    player_movement target_player = hit.collider.GetComponent<player_movement>();
                    Kill kill_comp = cam.GetComponentInParent<Kill>();

                    if (kill_comp != null && target_player != null) {
                        kill_comp.makeKill(target_player.playerName);
                    }
                } else */if (/*player.role == Role.Victime && */obj == null) {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null) {
                        interaction(interactable);
                        if (move != null) {
                            //move.disableCameraRot = true;
                            Debug.Log("Dsiable movement");
                        }
                    }
                }
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
