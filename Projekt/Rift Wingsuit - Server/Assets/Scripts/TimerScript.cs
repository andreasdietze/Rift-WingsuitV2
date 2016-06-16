using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimerScript : MonoBehaviour {

    public float time = 0;
    private TextMesh timerText;
    DateTime startTime;
    public bool isRunning;
    public string timeString;

	// Use this for initialization
	void Start () {
        timerText = this.gameObject.GetComponent<TextMesh>();
        if(timerText == null)
        {
            Debug.LogError("Timer Text is null!");
        }
        startTime = DateTime.Now;
        isRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            TimeSpan diffTime = startTime - DateTime.Now;
            diffTime = diffTime.Duration();
            this.timeString = string.Format("{0:mm\\:ss\\:ff}", new DateTime(diffTime.Ticks));
        }  
    }

    void FixedUpdate()
    {
        timerText.text = this.timeString;
    }
}
