using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DivertPower : Task
{

    new void Start()
    {
        base.Start();
        foreach (Transform child in transform.Find("SliderList").GetComponentInChildren<Transform>()) {
            child.GetComponent<Slider>().interactable = false;
        }
        transform.Find("SliderList").GetChild(Random.Range(0, transform.Find("SliderList").childCount)).GetComponent<Slider>().interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSliderValueChange(float value)
    {
        if (value >= 0.99) {
            OnTaskComplete.Invoke();
        }
    }
}
