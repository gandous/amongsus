using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UploadData : Task
{

    [SerializeField] Button button;
    [SerializeField] GameObject slider;
    [SerializeField] private TMP_Text text;

    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDownloadClick()
    {
        button.interactable = false;
        Timer timer = slider.GetComponent<Timer>();
        timer.StartTimer();
        text.text = "Downloading meredenathan.mp4";
    }

    public void OnTimerComplete()
    {
        OnTaskComplete.Invoke();
    }
}
