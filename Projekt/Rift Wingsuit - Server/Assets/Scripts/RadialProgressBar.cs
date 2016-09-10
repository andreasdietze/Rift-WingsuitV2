using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//https://www.youtube.com/watch?v=f6dEj7-G-Fw

public class RadialProgressBar /*: MonoBehaviour*/ {

	public Transform LoadingBar;
	[SerializeField] public float currentAmount;
	[SerializeField] public float speed;

	// Use this for initialization
	/*void Start () {
	
	}*/
	
	// Update is called once per frame
	/*void Update () {

	}*/

	void Progress (){
		if (currentAmount < 100) {
			currentAmount += speed * Time.deltaTime;
			
		} else {
			
		}
		LoadingBar.GetComponent<Image> ().fillAmount = currentAmount / 100;
	}
}
