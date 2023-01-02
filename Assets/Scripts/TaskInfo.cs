using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class TaskInfo : NetworkBehaviour
{
    [SerializeField] public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlider(int taskComplete, int taskTotal)
    {
        slider.value = (float)taskComplete / (float)taskTotal;
    }
}
