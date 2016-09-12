using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityStandardAssets.Characters.FirstPerson;

public class CheckOVRAvailable : MonoBehaviour {

    public FirstPersonController fpc;

	void Start () {
        if (VRDevice.isPresent)
            fpc.enabled = false;
        else
            fpc.enabled = true;
	}
}
