using UnityEngine;
using System.Collections;

/* Kinect input transformation:
 * During tracking the right shoulder and right hand joint
 * an offset between this two vertices can be computed
 * and used as user input.
 * There are two different offsets. 
 * One for height (arm y-movement) and 
 * another for the depth (arm z-movement).
 * Info: Rigth-handed system !
 */ 
public class HSDOutputText : MonoBehaviour {
	
	[Tooltip("Joint position form kinect")]
	public Transform rightShoulder;
	[Tooltip("Joint position form kinect")]
	public Transform rightHand;
	
	// Delta height between shoulder and hand
	private float deltaY;
	[Tooltip("Delta y multiplier for up/down arm movement")]
	private float dm = 1.25f;
	
	// Delta depth between shoulder and hand
	private float deltaZ;
	[Tooltip("Delta z multiplier for positive rear arme movement")]
	public float pdm = 1.95f; 
	[Tooltip("Delta z multiplier for negative front arme movement")]
	public float ndm = 1.25f;

	// Max controller value (also -0.5)
	private float delta = 0.5f;

	[Tooltip("Controler tolerance")]
	public float tol = 0.02f;
	
	[Tooltip("GUI Style")]
	public GUIStyle labelFont;

	// Getter
	public float GetDeltaY(){return deltaY;}
	public float GetDeltaZ(){return deltaZ;}
	
	// Setup font styles
	void Start () {
		// Debug font
		labelFont = new GUIStyle ();
		labelFont.fontSize = 28;
	}
	
	// Update kinect controller input
	void Update () {
		// Get delta y
		deltaY = rightShoulder.position.y - 
		         rightHand.position.y;
		// Get delta z
		deltaZ = rightShoulder.position.z - 
		         rightHand.position.z;
		
		// Extend rear arme range
		if (deltaZ >   1E-02f)  deltaZ *= pdm;
		if (deltaZ < (-1E-02f)) deltaZ *= ndm;
		
		// Sync maxY
		if (deltaY > delta) deltaY 	= delta;
		// Sync minY
		if (deltaY < -delta) deltaY = -delta;
		// Sync maxZ
		if (deltaZ > delta) deltaZ  = delta;
		// Sync minZ
		if (deltaZ < -delta) deltaZ = -delta;

		// Check for positive tolerance
		if (tol < 0.00f) tol *= -1; 
		// Set controller tolerance
		deltaY = (deltaY <= (-tol) || deltaY >= tol)
			? deltaY : 0.0f;
	}

	// Update debug info
	void OnGUI(){
		GUI.Label(new Rect(10, 10, 150, 150), "ShoulderY: " 
		          + rightShoulder.position.y.ToString("F2"), labelFont);
		GUI.Label(new Rect(10, 40, 150, 150), "HandY: " 
		          + rightHand.position.y.ToString("F2"), labelFont);
		GUI.Label(new Rect(10, 70, 150, 150), "OffsetY: " 
		          + deltaY.ToString("F2"), labelFont);
		GUI.Label(new Rect(10, 100, 150, 150), "HandZ: " 
		          + rightHand.position.z.ToString(), labelFont);
		GUI.Label(new Rect(10, 130, 150, 150), "OffsetZ: " 
		          + deltaZ.ToString("F2"), labelFont);
	}
}
