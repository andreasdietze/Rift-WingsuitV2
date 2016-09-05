using UnityEngine;
using System.Collections;

public class ArmSync : MonoBehaviour
{
    // Script access
    private NetworkManager nManager;
    private Player player;
    private HSDOutputText kinectOutput;

    // Arm transformation
    private float rot = 0.0f;
    private Transform leftShoulder;
    private Transform rightShoulder;

    void Start()
    {
        // Find networkManager
        nManager = (NetworkManager)GameObject.FindGameObjectWithTag("Network").GetComponent("NetworkManager");
        // Funzt leider nicht -> Arme seperat aber nicht suit
        leftShoulder = GameObject.Find("ShoulderL").transform;
        Debug.Log(leftShoulder.position);
        rightShoulder = GameObject.Find("ShoulderR").transform;

        kinectOutput = (HSDOutputText)GameObject.FindGameObjectWithTag("MainCamera").GetComponent("HSDOutputText");

    }

    // Update is called once per frame
    void Update()
    {
        rot = kinectOutput.GetDeltaZ();
        // Check if client has joined a server.
        // This is necessary because the playerprefab is automatically generated.
        if (nManager.serverInitiated) //serverJoined)
        {
            try
            {
                //player = (Player)GameObject.FindGameObjectWithTag ("Player").GetComponent("Player");
                //armPosition = player.syncEndOVRRotation;//lerpedOVRRotation;
                leftShoulder.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rot));
                rightShoulder.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rot));
            }
            catch (UnityException e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    bool showText = true;
    Rect textArea = new Rect(300, 60, Screen.width, Screen.height);

    private void OnGUI()
    {
        //if(nManager.serverJoined && showText)
        //GUI.Label(textArea, headOrientation.ToString());
    }

}
