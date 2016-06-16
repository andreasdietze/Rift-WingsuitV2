using UnityEngine;
using System.Collections;

public class CameraFollowerController : MonoBehaviour {

    public Transform target;
    public float lookSmoth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0, 3, -8);
    public float xTilt = 10;

    Vector3 destination = Vector3.zero;
    MyCharacterController charController;
    float rotateVel = 0;

	// Use this for initialization
	void Start ()
    {
        SetCameraTarget(target);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetCameraTarget(Transform t)
    {
        target = t;
        if(target != null)
        {
            if (target.GetComponent<MyCharacterController>())
            {
                charController = target.GetComponent<MyCharacterController>();
            }
            else
            {
                Debug.LogError("Your Camera target needs a Camera Controller!");
            }
        }
        else
        {
            Debug.LogError("Your Camera needs a target!");
        }
    }

    void LateUpdate()
    {
        MoveToTarget();
        LookAtTarget();
    }

    void MoveToTarget()
    {
        destination = charController.TargetRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmoth);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0);
    }
}
