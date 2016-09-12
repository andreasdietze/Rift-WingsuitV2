using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//https://www.youtube.com/watch?v=f6dEj7-G-Fw

public class RadialProgressBar : MonoBehaviour {

	public Transform LoadingBar;
	public float currentAmount;
	public float speed;
    public bool trigger;
    public static RadialProgressBar instance = null;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

	void Update (){
        if(trigger)
        {
            if (currentAmount < 100)
            {
                currentAmount += speed * Time.deltaTime;
            }
            LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
        }
	}
    public void Clear()
    {
        trigger = false;
        speed = 0.0f;
        currentAmount = 0.0f;
        LoadingBar.GetComponent<Image>().fillAmount = 0.0f;
    }
}
