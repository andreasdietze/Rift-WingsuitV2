using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SecretsManager : MonoBehaviour {

    public int collectedSecrets;
    public int maxSecrets = 7;
    TextMesh secretsText;

	// Use this for initialization
	void Start () {
        collectedSecrets = 0;
        secretsText = this.gameObject.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        secretsText.text = "Secrets: " + collectedSecrets + "/" + maxSecrets;
	}

    public void Collect()
    {
        if(collectedSecrets < maxSecrets)
        {
            collectedSecrets++;
        }
    }
}
