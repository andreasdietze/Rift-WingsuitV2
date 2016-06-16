using UnityEngine;
using System.Collections;

public class MyCharacterController : MonoBehaviour {

    public float inputDelay = 0.1f;
    public float forwardVel = 12;
    public float rotateVel = 100;

    Quaternion targetRotation;
    Rigidbody rBody;
    float forwardInput, turnInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

	// Use this for initialization
	void Start () {
        targetRotation = transform.rotation;
        if(GetComponent<Rigidbody>())
        {
            rBody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Character Controller does not have a rigidbody");
        }
        forwardInput = turnInput = 0;
	}
	
    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

	// Update is called once per frame
	void Update () {
        GetInput();
        Turn();
    }

    void FixedUpdate()
    {
        Run();
    }

    void Run()
    {
        if(Mathf.Abs(forwardInput) > inputDelay)
        {
            //move
            Vector3 v3 = transform.forward * forwardInput * forwardVel;
            v3.y = rBody.velocity.y;
            //v3.y = Physics.gravity.y;
            rBody.velocity = v3;
        }
        else
        {   //stop velocity
            Vector3 v3 = Vector3.zero;
            v3.y = rBody.velocity.y;
            //v3.y = Physics.gravity.y;
            rBody.velocity = v3;
        }
        rBody.AddForce(Physics.gravity * 20);
    }

    void Turn()
    {
        targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;
    }

    public void Boost(float boostSpeed, int seconds)
    {
        StartCoroutine(Booster(boostSpeed, seconds));
    }

    IEnumerator Booster(float boostSpeed, int seconds)
    {
        this.forwardVel += boostSpeed;
        yield return new WaitForSeconds(seconds);
        this.forwardVel -= boostSpeed;
    }


}
