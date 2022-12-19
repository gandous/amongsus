using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    public UnityEvent OnTaskComplete;
    public UnityEvent OnTaskCancel;

    protected void Start()
    {
        if (OnTaskComplete == null)
            OnTaskComplete = new UnityEvent();
    }

}
