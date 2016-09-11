using UnityEngine;
using System.Collections;

public class PlayerForwardPush : MonoBehaviour {

    private Transform heliDoorTransform;
    public Transform playerTransform;
    public bool finished = false;
    public StartLevelRayCaster slrc; 
    private Quaternion rotVector;
    private Vector3 dest;
    private Vector3 start;
    public Vector3 vecToAdd;

    private float speed = 0.05F;
    private float startTime;
    private float journeyLength;

	// Use this for initialization
	void Start () {
        finished = false;
        heliDoorTransform = GameObject.Find("exitDoor").transform;
        start = Vector3.zero;
        dest = new Vector3(0f, 0f, 0.125f);
        vecToAdd = Vector3.zero;
        startTime = Time.time;
        journeyLength = Vector3.Distance(start, dest);
	}
	
	// Update is called once per frame
	void Update () {
        if (slrc.startGame)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;

            vecToAdd = Vector3.Lerp(start, dest, fracJourney);

            if (vecToAdd.z >= 0.125f)
                finished = true;
        }
        else
            startTime = Time.time;
	}
}
