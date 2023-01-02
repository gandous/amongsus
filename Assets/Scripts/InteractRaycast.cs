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
    private Interactable interact;
    private player_movement pplayer;

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
            interact = null;
        }
        if (Input.GetButtonDown("Interact")) {
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, InteractDistance)) {
                FirstPersonMovement move = cam.GetComponentInParent<FirstPersonMovement>();
                player_movement player = cam.GetComponentInParent<player_movement>();
                DeadPlayer dead = hit.collider.GetComponent<DeadPlayer>();
                pplayer = player;

                if (dead != null && player.dead != true) {
                    player.Report(dead.playerName);
                } else if (player.role == Role.SUS) {
                    Kill kill_comp = cam.GetComponentInParent<Kill>();
                    player_movement target_player = hit.collider.GetComponent<player_movement>();

                    if (kill_comp != null && target_player != null) {
                        kill_comp.makeKill(target_player.playerName);
                    }
                } else if (player.role == Role.Victime && obj == null) {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                        print("interact");
                        print(interactable);
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
        interact = interactable;

        Task task = obj.GetComponent<Task>();
        if (task) {
            task.OnTaskComplete.AddListener(InteractionComplete);
        }
    }

    void InteractionComplete()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Task complete");
        pplayer.TaskComplete(interact.id);
        Destroy(obj);
        interact = null;
        obj = null;
    }
}
