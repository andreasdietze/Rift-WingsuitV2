using UnityEngine;
using System.Collections;

public class KinectController : Controller
{
    // Access to kincect input transformation script
    public HSDOutputText kinectOutput;
    public float kinectSensitivityY = 8.00f;
    public float kinectSensitivityZ = 8.00f;

    // Kinect output
    private float kinectYaw;
    public bool enableYaw = true;

    private float kinectPitch;
    public bool enablePitch = true;

    // Script access 
    public StartLevelRayCaster slrc;
    private NetworkManager nManager;
    private FlyCam flyCam;

    // Debug-controlls
    private Vector3 mouse = new Vector3();
    public bool useMouse = true;
    public bool useKB = true;
    public bool inverted = false;

    // Const flyspeed
    public bool hasAutoVelocity = false;
    public float flySpeed = 0.0f;			// between 0 and 1;

    // Network and gameStatus flag
    private bool serverInitiated = false;
    private bool gameStart = false;

    //Physic Global Variables
    public Rigidbody rb;

    // Debug font
    private GUIStyle font;

    void Start()
    {
        // Get NetworkManager-Script
        nManager = (NetworkManager)GameObject.FindGameObjectWithTag("Network").GetComponent("NetworkManager");

        // Get FlyCam-Script
        flyCam = (FlyCam)GameObject.FindGameObjectWithTag("MainCamera").GetComponent("FlyCam");

        // Debug font
        font = new GUIStyle();
        font.fontSize = 28;

        // Apply menue settings
        kinectSensitivityY *= GameController.instance.yAxisAdjust;
        kinectSensitivityZ *= GameController.instance.xAxisAdjust;
    }

    // Debug kb controls and auto velocity
    public override Vector3 GetDir()
    {
        // Speed
        Vector3 dir = new Vector3();

        // Debug kb controls
        if (useKB)
        {
            if (Input.GetKey(KeyCode.W))
                dir.z += 1.0f;
            if (Input.GetKey(KeyCode.S))
                dir.z -= 1.0f;
            if (Input.GetKey(KeyCode.A))
                dir.x -= 1.0f;
            if (Input.GetKey(KeyCode.D))
                dir.x += 1.0f;
            if (Input.GetKey(KeyCode.Q))
                dir.y -= 1.0f;
            if (Input.GetKey(KeyCode.E))
                dir.y += 1.0f;
        }

        gameStart = slrc.startGame;
        //Debug.Log (gameStart);

        // If all flags are true, add to z
        if (hasAutoVelocity)
        {				// by unity gui (debug)
            if (serverInitiated)
            {  			// by network	(works)
                if (gameStart)
                {				// by raycast	(works)
                    if (flyCam.startFly)
                    {	// by fly physics (works)
                        flySpeed = 1.0f;
                        dir.z += flySpeed;
                    }
                }
            }
        }

        dir.Normalize();
        return dir;
    }

    // Get kinect user input and use it as rotation
    public override Vector3 CalculateViewport(bool inverted)
    {
        // Kinect user input
        if (enableYaw)
            kinectYaw = kinectOutput.GetDeltaY() * kinectSensitivityY;
        else
            kinectYaw = 0.0f;
        if (enablePitch)
            kinectPitch = kinectOutput.GetDeltaZ() * kinectSensitivityZ;
        else
            kinectPitch = 0.0f;
        //print ("KinectOutput: " + kinectOutput.ToString ());

        // Mouse Look (Debug)
        if (useMouse)
        {
            mouse = Input.mousePosition;
            lastViewport = mouse - lastViewport;
            if (!inverted)
                lastViewport.y = -lastViewport.y;
        }

        // Actual angel + mouse + kinect
        lastViewport *= sensitivity;
        lastViewport = new Vector3(transform.eulerAngles.x + lastViewport.y + kinectPitch,
                                   transform.eulerAngles.y + lastViewport.x + kinectYaw, 0);// * sensitivity;

        // Testing kinect animation
        //lastViewport = new Vector3(transform.eulerAngles.x + lastViewport.y,
        //transform.eulerAngles.y + lastViewport.x, 0);// * sensitivity;


        // Lock cam rotation from 0-90 on x
        if (useMouse)
        {
            transform.eulerAngles = lastViewport;
            Vector3 rot = transform.eulerAngles;
            if (rot.x >= 90.0f)
                rot.x = 0.0f;

            transform.eulerAngles = rot;
            lastViewport = Input.mousePosition;
        }

        //Debug.Log ("Fall speed: " + rb.velocity.y);
        return lastViewport;
    }

    // Update is called once per frame
    void Update()
    {
        this.serverInitiated = nManager.serverInitiated;  	// works
        //if (Input.GetKey(KeyCode.Return))					// test
        //this.serverInitiated = true;
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(10, 280, 150, 150), "Viewport: " + lastViewport.ToString(), font);
    }
}
