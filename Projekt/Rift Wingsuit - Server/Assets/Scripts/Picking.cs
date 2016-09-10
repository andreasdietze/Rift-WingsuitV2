using UnityEngine;
using System.Collections;

public class Picking : MonoBehaviour {

	//public GameObject [] menuObjects;
	//das brauch man net wirklich, kann man einfach über die namen handlen aber es so zu machen is halt dynamischer
	//anscheinend sollte ich aber nicht das bell objekt nehmen sondern dieses fusell oder wie das heißt, was das außenskelett des helis darstellt sonst kommt ne exception
	//alle objecte brauchen nen meshcollider...

	//was getriggert wird, sollte dann wohl anhand der tags festgemacht werden

	//NORMAL UND HEIGHT MAPS ÜBERALL REINHAUEN (MIT PHOTOSHOP ERSTELLEN)
	//https://www.youtube.com/watch?v=_3CG-lzy0RA
	public float raySize;
	public GameObject[] menuObjects;
	string lastHoverTag;
	bool collided;


	// Use this for initialization
	void Start () {
		lastHoverTag = "";
		collided = false;
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		Vector3 forward = transform.TransformDirection (Vector3.forward) * raySize;

		Ray pickingRay = new Ray (transform.position, forward);

		Debug.DrawRay (transform.position, forward, Color.green);

		if (Physics.Raycast (pickingRay, out hit)) {
			if (menuObjects != null && menuObjects.Length > 0) {
				foreach (GameObject menuObject in menuObjects) {
					if (hit.collider.gameObject.name == menuObject.name) {
						collided = true;
						lastHoverTag = menuObject.tag;
						ShowHoverEffect (hit.collider.gameObject);
					}
				}

			}
			//float dist = hit.distance;
			//if(hit.collider.gameObject.name != "garage")
			   // print (dist + " " + hit.collider.gameObject.name);
		}
		if (!collided) {
			if(lastHoverTag != "")
			{
				GameObject [] objs = GameObject.FindGameObjectsWithTag (lastHoverTag);
				if (objs.Length != 0) {
					foreach (GameObject obj in objs) {
						if (obj.GetComponent<Renderer> () != null) {
							Material [] mats = obj.GetComponent<Renderer> ().materials;
							for (int i = 0; i < mats.Length; i++) {
								if (mats [i].name.Contains ("hover")) {
									mats [i].shader = Shader.Find ("Standard");
								}
							}
						}
					}
				}
				lastHoverTag = "";
			}
		}
		else{
			collided = false;
		}
	}

	void ShowHoverEffect(GameObject go){
		GameObject []objs = GameObject.FindGameObjectsWithTag (go.tag);
		if (objs.Length != 0) {
			foreach (GameObject obj in objs) {
				if(obj.GetComponent<Renderer>() != null)
				{
					Material []mats = obj.GetComponent<Renderer>().materials;
					for (int i = 0; i < mats.Length; i++) {
						if(mats[i].name.Contains("hover")) {
							mats[i].shader = Shader.Find("Custom/ItemGlow");
						}
					}
				}
			}
		}
	}
	
	void OnCollisionEnter(){

	}
}
