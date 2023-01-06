using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideable : MonoBehaviour
{
    [SerializeField] colorUI colorui;
    bool pickdefault = true;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickdefault && player_movement.Local != null) {
            pickdefault = false;
            StartCoroutine(SelectColor());
        }
        if (Input.GetKeyUp("t") == true) {
            UnlockShow();
        }
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (player_movement.Local != null) {
            player_movement.Local.GetComponent<FirstPersonMovement>().movementDisable = false;
        }
    }

    public void UnlockShow()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player_movement.Local.GetComponent<FirstPersonMovement>().movementDisable = true;
    }

    IEnumerator SelectColor()
    {
        yield return new WaitForSeconds(1);
        colorui.PickDefault();
    }
}
