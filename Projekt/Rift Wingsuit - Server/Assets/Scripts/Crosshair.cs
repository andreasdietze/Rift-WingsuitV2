using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	Rect crosshairRect;
	public Texture crosshairTexture;
	public float crosshairSize;

	// Use this for initialization
	void Start () {
		//crosshairSize = Screen.width * 0.1f;
		//crosshairTexture = Resources.Load ("Images/crosshair") as Texture;
		crosshairRect = new Rect (Screen.width / 2 - crosshairSize / 2, Screen.height / 2 - crosshairSize / 2, crosshairSize, crosshairSize);
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/

	void OnGUI()
	{
		GUI.DrawTexture (crosshairRect, crosshairTexture);
	}
}
