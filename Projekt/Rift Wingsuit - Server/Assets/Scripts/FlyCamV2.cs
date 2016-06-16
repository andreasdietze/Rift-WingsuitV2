using UnityEngine;
using System.Collections;
using System.Linq;

public class FlyCamV2 : MonoBehaviour
{
    public bool hasStarted = true;

    // smoothing
    public bool smooth = true;
    public float acceleration = 0.2f;
    protected float actSpeed = 0.0f;    // keep it from 0 to 1
    private float currentSpeed = 0.0f;
    private float lastAngle = -1.0f;
    public float cameraAcceleration = 1.5f;
    public bool inverted = false;
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    public float sensitivity = 0.25f;   // keep it from 0..1

    // Direction
    protected Vector3 lastDir = new Vector3();
    protected Vector3 lastViewport = new Vector3();
    Quaternion startingRot;
    Vector3 startingPos;

    // Global controller settings
    public bool useKinect = false;
    public bool useMouseKB = false;
    public bool usePad = false;
    private Controller controller;
    public Rigidbody playerRidgid;

	private Quaternion initialRotation;
	private Vector3 position;

    // Fly physics
    float startTime = -1.0f;
    Vector3 fallVelocity = new Vector3();
    public bool startFly = false;
    Vector3 addToZ = Vector3.zero;

    // Script access
    NetworkManager nManager;
    StartLevelRayCaster slrc;

    // Debug font
    GUIStyle font;

    //Updraft and Speed Boost
    float updraftStrength = 0;
    float speedBoost = 1.0f;
    public Transform playerPivot;

    void Start()
    {
        // Set preferred controller
        if (useKinect) controller = (Controller)GameObject.Find("RiftCam").GetComponent("KinectController"); //gameObject.AddComponent<KinectController> ();
        else if (useMouseKB) controller = (Controller)GameObject.Find("RiftCam").GetComponent("KeyboardAndMouseController");
        else if (usePad) controller = (Controller)GameObject.Find("RiftCam").GetComponent("XBoxController");

        // Get NetworkManager-Script
        nManager = (NetworkManager)GameObject.FindGameObjectWithTag("Network").GetComponent("NetworkManager");

        // Debug font
        font = new GUIStyle();
        font.fontSize = 28;

        // Tmp start transformation for reset
        startingPos = transform.position;
        startingRot = transform.rotation;
    }

    public void StartFlight()
    {
        this.hasStarted = true;
		playerRidgid.rotation = startingRot;
    }

    float getTime()
    {
        return (startTime == -1.0f ? 0.0f : Time.time - startTime);
    }

    void Update()
    {
        if (hasStarted)
        {

            // Setup direction vector
            Vector3 dir = new Vector3();// create (0,0,0)

            // Compute direction by active controller
            lastViewport = controller.CalculateViewport(inverted);
            //Directory of the "force to apply"
            dir = controller.GetDir();

            //Updraft
            Updraft(2, 20);

            //Boost

            float currentAngle = FlyPhysics.getAngleToFloor(playerRidgid.rotation);
            if (lastAngle < 0.0f)
            {
                lastAngle = currentAngle;
            }
            float diff = Mathf.Abs(currentAngle - lastAngle);
            if (diff >= 90.0f)
            {
                currentAngle = lastAngle;
            }
            float currentRotation = FlyPhysics.getRotation(playerRidgid.rotation);
            lastAngle = currentAngle;

            slrc = (StartLevelRayCaster)GameObject.FindGameObjectWithTag("MainCamera").GetComponent("StartLevelRayCaster");
            fallVelocity = playerRidgid.velocity;
			if (slrc.startGame)
            {
                if (!playerRidgid.useGravity)
                {
                    startTime = Time.time;
                }
                playerRidgid.useGravity = true;
            }

            if (playerRidgid.useGravity)
            {
                FlyPhysics.adjustCrossSectionArea(lastAngle);
                fallVelocity = FlyPhysics.calculateFallVector(lastAngle, currentRotation);
            }


            Vector3 fallingOnly = new Vector3(0.0f, fallVelocity.y, 0.0f);
            Vector3 updraftVector = new Vector3(0.0f, updraftStrength, 0.0f);

            fallVelocity.y = 0.0f;

            //Apply Speed Boost
            fallVelocity.x = fallVelocity.x * speedBoost;
            fallVelocity.z = fallVelocity.z * speedBoost;

            playerRidgid.velocity = fallVelocity;
            transform.Translate(playerRidgid.velocity);
            transform.Translate(transform.InverseTransformVector(fallingOnly));
            transform.Translate(updraftVector);
            playerPivot.Translate(updraftVector);
        }
    }

    public void Updraft(float seconds, float strenght)
    {
        if (updraftStrength > 0)
            updraftStrength = updraftStrength - seconds;

        if (Input.GetKeyDown(KeyCode.U))
        {
            //speedBoost = 2;
            //StartCoroutine(UpdraftRoutine(seconds, strenght));
            updraftStrength = strenght;
        }
    }

    IEnumerator UpdraftRoutine(float seconds, float strenght)
    {
        updraftStrength = strenght;
        yield return new WaitForSeconds(seconds);
        updraftStrength = 0;
    }

    // 1 m/s = 3,6 km/h   -> fall speed m/s * 3.6f
    private float MStoKMH(float ms)
    {
        return ms * 3.6f;
    }

    // Nach Formel
    Vector3 UpdateSpeed()
    {
        // Winkel zwischen z- und y-Achse: ---------
        // 0° x -> vec3.back			   | \
        // 90°  x -> vec3.down			   |    \
        // alpha -> x rotation   		   | alpha \
        float alpha = transform.rotation.eulerAngles.x;
        if (alpha >= 90.0f)
            alpha = 90.0f; // -alpha; 

        // Geschindigkeit toAdd (z):
        // toAdd = gravityVec * sin(alpha) - (attenuation) * V²
        // Dämpfung abhängig von x rotation, 90° -> A = 1.0, 0° -> A = 0.0
        // att   =  xrot / 90
        Vector3 gravity = Vector3.zero;
        if (nManager.serverInitiated)
            gravity = new Vector3(0.0f, playerRidgid.velocity.y, 0.0f);   // Vector3.down;  playerRidgid.velocity
        float att = alpha / 90.0f;
        Vector3 toAdd = (gravity * Mathf.Sin(alpha)); //  - ((1.0f * att) / 2); 
        return toAdd - (Vector3.forward * ((1.0f * att) / 2));
    }

    // Einfache, derzeit verwendete Flugphysik
    Vector3 UpdateSpeedSimple()
    {
        float alpha = transform.rotation.eulerAngles.x;
        if (alpha >= 90.0f)
            alpha = 0.0f;

        float att = alpha / 90.0f;

        return Vector3.forward * att;
    }

    private GameObject GetParent(Transform trans)
    {
        GameObject result = null;
        if (trans != null)
        {
            result = trans.parent.gameObject;
        }
        return result;
    }

    // Reload level if player collides with terrain
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boden"))
        {
            Application.LoadLevel(0);
            playerRidgid.rotation = startingRot;
            playerRidgid.position = startingPos;
        }
        else if (other.name.Equals("StartWP"))
        {
            //"StartWP" is the name of every ring, which means a parent object is
            //existent. Check for the Finisher tag, if existent, start fading.
            GameObject actualWaypointRing = GetParent(other.transform);
            if (actualWaypointRing.CompareTag("FinisherRing"))
            {
                //Fade out in around 5 seconds and restarts the level
                Fader.StartFade(Color.white);
            }
        }
    }

    void OnGUI()
    {
        //GUILayout.Box("Vector: " + lastViewport.ToString());
        //GUI.Label(new Rect(10, 130, 150, 150), "Delta: " + kinectY.ToString(), kinectOutput.labelFont);
        float angleToFloor = FlyPhysics.getAngleToFloor(playerRidgid.rotation);
        float rotation = FlyPhysics.getRotation(playerRidgid.rotation);
        GUI.Label(new Rect(10, 250, 150, 150), "Angle: " + angleToFloor + ", Rot: " + rotation + ", Spd: " + FlyPhysics.calculateFallVector(angleToFloor, rotation), font);
    }
}
