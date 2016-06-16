using UnityEngine;
using System.Collections;

public class RotorTurner : MonoBehaviour {

    public bool isBackRotor = false;
    public float rotationSpeedTop = 10;
    public float rorationSpeedBack = 10;
	
	// Update is called once per frame
	void Update () {
        Rotate();
	}

    void Rotate()
    {
        //Rotate
        if (isBackRotor)
        {
           transform.rotation = Quaternion.Euler(transform.eulerAngles.x + rorationSpeedBack, transform.eulerAngles.y , 0);
        } else
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotationSpeedTop, 0);
        }
        
    }
}
