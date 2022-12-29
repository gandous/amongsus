using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwipeCard : Task
{
    float AllowedTimeDiff = 0.1f;
    float AllowedTime = 0.35f;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject card;
    [SerializeField] private GameObject slider;
    Slider comp_slider;
    float start_time = 0;
    bool focus = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        slider.SetActive(false);
        comp_slider = slider.GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (focus && comp_slider.value < 0.05) {
            start_time = Time.time;
        }
    }

    public void OnCardClicked()
    {
        text.text = "Swipe card";
        Destroy(card);
        slider.SetActive(true);
    }

    public void onSliderValueChange(float value)
    {
        if (comp_slider.value > 0.95 && focus) {
            float diff = Time.time - start_time;
            if (Mathf.Abs(diff - AllowedTime) < AllowedTimeDiff) {
                OnTaskComplete.Invoke();
                focus = false;
                text.text = "Success!";
            } else if (diff > AllowedTime) {
                text.text = "Too slow!";
            } else if (diff < AllowedTime) {
                text.text = "Too fast!";
            }
        }
    }

    public void MouseDown()
    {
        focus = true;
        start_time = Time.time;
    }

    public void MouseUp()
    {
        comp_slider.value = 0;
    }
}
