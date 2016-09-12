using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Picking : MonoBehaviour {

	//https://www.youtube.com/watch?v=_3CG-lzy0RA
	public float raySize;
	public GameObject[] menuObjects;
	string lastHoverTag, currentHoverTag, currentEvent;
    bool collided, progressStarted;
    public bool lockBar;
    Shader itemGlow, standard;

	// Use this for initialization
	void Start () {
		lastHoverTag = "";
        currentHoverTag = "";
        currentEvent = "";
		collided = progressStarted = lockBar = false;
        itemGlow = Shader.Find("Custom/ItemGlow");
        standard = Shader.Find("Standard");
    }
	
	// Update is called once per frame
	void Update () {

        if(!lockBar)
        {
            if (RadialProgressBar.instance.currentAmount >= 100.0f)
            {
                switch (currentEvent)
                {
                    case "Exit":
                        RadialProgressBar.instance.Clear();
                        Application.Quit();
                        break;
                    case "Start":
                        RadialProgressBar.instance.Clear();
                        SceneManager.LoadScene(2);
                        GameController.instance.currentScene = 2;
                        break;
                    case "Settings":
                        lockBar = true;
                        RadialProgressBar.instance.Clear();
                        RadialProgressBar.instance.gameObject.SetActive(false);
                        Text[] texts = AdjustingPanel.instance.GetComponentsInChildren<Text>();
                        Slider[] sliders = AdjustingPanel.instance.GetComponentsInChildren<Slider>();
                        Image[] images = AdjustingPanel.instance.GetComponentsInChildren<Image>();
                        Button confirmBtn = AdjustingPanel.instance.GetComponentInChildren<Button>();

                        foreach(Text text in texts)
                        {
                            text.enabled = true;
                        }
                        foreach(Slider slider in sliders)
                        {
                            slider.interactable = true;
                        }
                        foreach(Image image in images)
                        {
                            image.enabled = true;
                        }
                        confirmBtn.interactable = true;
                        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("SliderX"));
                        break;
                }
            }

            RaycastHit hit;

            Vector3 forward = transform.TransformDirection(Vector3.forward) * raySize;

            Ray pickingRay = new Ray(transform.position, forward);

            Debug.DrawRay(transform.position, forward, Color.green);

            if (Physics.Raycast(pickingRay, out hit))
            {
                if (menuObjects != null && menuObjects.Length > 0)
                {
                    foreach (GameObject menuObject in menuObjects)
                    {

                        if (hit.collider.gameObject.name == menuObject.name)
                        {
                            collided = true;
                            if (currentHoverTag == "")
                            {
                                currentHoverTag = menuObject.tag;
                            }
                            else
                            {
                                lastHoverTag = currentHoverTag;
                                currentHoverTag = menuObject.tag;
                            }
                            if (!progressStarted)
                            {
                                progressStarted = true;
                                RadialProgressBar.instance.trigger = true;
                                RadialProgressBar.instance.speed = 24;
                                if (currentHoverTag == "Exit(Hover)")
                                {
                                    currentEvent = "Exit";
                                }
                                else if (currentHoverTag == "Heli(Hover)")
                                {
                                    currentEvent = "Start";
                                }
                                else if (currentHoverTag == "Schrank(Hover)")
                                {
                                    currentEvent = "Settings";
                                }
                            }
                            ShowHoverEffect(hit.collider.gameObject);
                        }
                    }
                }



                //float dist = hit.distance;
                //if(hit.collider.gameObject.name != "garage")
                //print (dist + " " + hit.collider.gameObject.name);
            }

            if (collided && lastHoverTag != currentHoverTag && lastHoverTag != "")
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag(lastHoverTag);
                if (objs.Length != 0)
                {
                    foreach (GameObject obj in objs)
                    {
                        if (obj.GetComponent<Renderer>() != null)
                        {
                            Material[] mats = obj.GetComponent<Renderer>().materials;
                            for (int i = 0; i < mats.Length; i++)
                            {
                                if (mats[i].name.Contains("hover"))
                                {
                                    mats[i].shader = standard;
                                }
                            }
                        }
                    }
                }
                lastHoverTag = "";
                RadialProgressBar.instance.Clear();
                if (!lockBar)
                {
                    progressStarted = false;
                }
            }
            else if (!collided)
            {
                if (currentHoverTag != "" || lastHoverTag != "")
                {
                    foreach (GameObject obj in menuObjects)
                    {
                        if (obj.GetComponent<Renderer>() != null)
                        {
                            Material[] mats = obj.GetComponent<Renderer>().materials;
                            for (int i = 0; i < mats.Length; i++)
                            {
                                if (mats[i].name.Contains("hover"))
                                {
                                    if (mats[i].shader.name == "Custom/ItemGlow")
                                        mats[i].shader = standard;
                                }
                            }
                        }
                    }
                    lastHoverTag = "";
                    currentHoverTag = "";
                    RadialProgressBar.instance.Clear();
                    if (!lockBar)
                    {
                        progressStarted = false;
                    }
                }
            }
            else
            {
                collided = false;
            }
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
                            mats[i].shader = itemGlow;
						}
					}
				}
			}
		}
	}
	
	/*void OnCollisionEnter(){

	}*/
}
