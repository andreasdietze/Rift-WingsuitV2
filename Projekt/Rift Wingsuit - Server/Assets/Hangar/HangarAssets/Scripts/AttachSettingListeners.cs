using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttachSettingListeners : MonoBehaviour {

	void Awake()
    {
        Slider sliderX, sliderY;
        Button confirm;

        sliderX = GameObject.Find("SliderX").GetComponent<Slider>();
        sliderY = GameObject.Find("SliderY").GetComponent<Slider>();
        confirm = GameObject.Find("ConfirmButton").GetComponent<Button>();

        sliderX.onValueChanged.AddListener(delegate { OnChangeX(sliderX); });
        sliderY.onValueChanged.AddListener(delegate { OnChangeY(sliderY); });
        confirm.onClick.AddListener(() => { OnConfirm(); });
    }

    void OnChangeX(Slider slider)
    {
        GameController.instance.xAxisAdjust = slider.value;
        print("X-Axis-Value: " + slider.value);
    }
    void OnChangeY(Slider slider)
    {
        GameController.instance.yAxisAdjust = slider.value;
        print("Y-Axis-Value: " + slider.value);
    }

    void OnConfirm()
    {
        RadialProgressBar.instance.gameObject.SetActive(true);

        Text[] texts = AdjustingPanel.instance.GetComponentsInChildren<Text>();
        Slider[] sliders = AdjustingPanel.instance.GetComponentsInChildren<Slider>();
        Image[] images = AdjustingPanel.instance.GetComponentsInChildren<Image>();
        Button confirmBtn = AdjustingPanel.instance.GetComponentInChildren<Button>();

        foreach (Text text in texts)
        {
            text.enabled = false;
        }
        foreach (Slider slider in sliders)
        {
            slider.interactable = false;
        }
        foreach (Image image in images)
        {
            image.enabled = false;
        }
        confirmBtn.interactable = false;

        GameObject.Find("FirstPersonCharacter").GetComponent<Picking>().lockBar = false;
    }
}
