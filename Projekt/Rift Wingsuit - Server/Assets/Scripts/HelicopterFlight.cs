using UnityEngine;
using System.Collections;

public class HelicopterFlight : MonoBehaviour {

    public float flightSpeed = 10f;
    public float startSpeed = 20f;
    public int closeDoorAfter = 1;
    public int deleteSecondsAfterStart = 10;
    public float doorRotationThreshold = -0.7f;
    public float closeDoorSpeed = 5f;
    private bool goStart = false;
    private bool hasStarted = false;
    private bool shallCloseDoor = false;
    Transform BackCollider;
    Transform Door;

	// Use this for initialization
	void Start () {
        //BackCollider = transform.FindChild("Colliders/BackCollider");
        Door = transform.FindChild("door1");
        if(BackCollider == null)
        {
            Debug.LogWarning("Could not find Back Collider of Helicopter");
        }
        if (Door == null)
        {
            Debug.LogWarning("Could not find Door of Helicopter");
        }
    }

    public void GoHeliStart()
    {
        if (!hasStarted)
        {
            goStart = true;
            hasStarted = true;
            StartCoroutine(StartDelete(closeDoorAfter, deleteSecondsAfterStart));
        }
    }

    IEnumerator StartDelete(int closeDoorSeconds, int deleteSeconds)
    {
        yield return new WaitForSeconds(closeDoorSeconds);
        shallCloseDoor = true;
        yield return new WaitForSeconds(deleteSeconds);
        goStart = false;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate () {
        Vector3 flightVector = new Vector3(0, 0, flightSpeed * -1);
        transform.Translate(flightVector);
        if (goStart)
        {
            //Close Door
            if(shallCloseDoor && Door.rotation.y > doorRotationThreshold)
            {
                Debug.Log(Door.rotation.y);
                Door.rotation = Quaternion.Euler(Door.eulerAngles.x, Door.eulerAngles.y - closeDoorSpeed, Door.eulerAngles.z);
            }
        }
    }

}
