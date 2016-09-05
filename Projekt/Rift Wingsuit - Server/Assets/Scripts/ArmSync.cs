using UnityEngine;
using System.Collections;

public class ArmSync : MonoBehaviour
{
    // Script access
    private NetworkManager nManager;
    private Player player;
    private HSDOutputText kinectOutput;

    // Arm transformation
    private float rotY = 0.0f;
    private float rotZ = 0.0f;
    private Transform leftShoulder;
    private Transform rightShoulder;
    private Transform playerMesh;

    void Start()
    {
        // Find networkManager
        nManager = (NetworkManager)GameObject.FindGameObjectWithTag("Network").GetComponent("NetworkManager");
        leftShoulder = GameObject.Find("ShoulderL").transform;
        //Debug.Log(leftShoulder.position);
        rightShoulder = GameObject.Find("ShoulderR").transform;
        kinectOutput = (HSDOutputText)GameObject.FindGameObjectWithTag("MainCamera").GetComponent("HSDOutputText");
        player = (Player)GameObject.FindGameObjectWithTag("Player").GetComponent("Player");
    }

    // Update is called once per frame
    void Update()
    {
        rotY = kinectOutput.GetDeltaY();
        rotZ = kinectOutput.GetDeltaZ();
        //Debug.Log(rot * 100);

        // Check if client has joined a server.
        // This is necessary because the playerprefab is automatically generated.
        if (nManager.serverInitiated) //serverJoined)
        {
            try
            {
                player.transform.rotation = player.transform.rotation * 
                    Quaternion.Euler(new Vector3(0.0f, -rotY * 100, 0.0f));
                leftShoulder.rotation = player.transform.rotation * 
                    Quaternion.Euler(new Vector3(0.0f, 0.0f, 90f + (rotZ * 50)));
                rightShoulder.rotation = player.transform.rotation * 
                    Quaternion.Euler(new Vector3(0.0f, 0.0f, -90.0f - (rotZ * 50)));
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
