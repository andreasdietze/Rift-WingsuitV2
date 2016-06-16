using UnityEngine;
using System.Collections;

public class SecretCollectible : Collectible {

    public SecretsManager sm;

    public void Start()
    {
        sm = (SecretsManager) GameObject.Find("/Main Camera/UIPivot/SecretsManager").GetComponent("SecretsManager");
        if (sm == null)
        {
            Debug.LogError("Could not find Secrets Manager");
        }
    }

    public override void PerformAction(Collider collision)
    {
        sm.Collect();
        this.gameObject.SetActive(false);
    }
}
