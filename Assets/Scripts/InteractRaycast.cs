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
    private bool canKill = true;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && obj != null) {
            StartCoroutine(completeInter());
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
                    player.Report();
                } else if (player.role == Role.SUS) {
                    Kill kill_comp = cam.GetComponentInParent<Kill>();
                    player_movement target_player = hit.collider.GetComponent<player_movement>();

                    if (kill_comp != null && target_player != null && canKill) {
                        canKill = false;
                        StartCoroutine(killCoro());
                        kill_comp.makeKill(target_player.playerName, false);
                    }
                } else if (player.role == Role.Victime && obj == null) {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null) {
                        if (player.taskList.Contains(interactable.id)) {
                            interaction(interactable);
                            if (move != null) {
                                move.movementDisable = true;
                                Debug.Log("Dsiable movement");
                            }
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
        Debug.Log("Task complete");
        pplayer.TaskComplete(interact.id);
        StartCoroutine(completeInter());
    }

    IEnumerator completeInter()
    {
        yield return new WaitForSeconds(0.5f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(obj);
        interact = null;
        obj = null;
        pplayer.GetComponent<FirstPersonMovement>().movementDisable = false;
    }

    IEnumerator killCoro()
    {
        yield return new WaitForSeconds(20f);
        canKill = true;
    }
}
