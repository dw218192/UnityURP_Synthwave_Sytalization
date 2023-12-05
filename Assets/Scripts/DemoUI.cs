using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUI : MonoBehaviour
{
    public Slider speedSlider;

    private void Start()
    {
        ConfigSlider(speedSlider, "Speed", 0.5f, 20.0f, DemoManager.Instance.Speed, (float value) =>
        {
            DemoManager.Instance.Speed = value;
        });
    }

    void ConfigSlider(Slider slider, string label,
        float min, float max, float value,
        UnityEngine.Events.UnityAction<float> callback)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = value;
        slider.transform.parent.GetComponentInChildren<Text>().text = label;
        slider.onValueChanged.AddListener(callback);
    }
}
