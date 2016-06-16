using UnityEngine;
using System.Collections;

public class BoosterCoin : Collectible {

    public float BoostSpeed = 5;
    public int BoostTime_Seconds = 3;

    public override void PerformAction(Collider collision)
    {
        Boost(collision.gameObject);
        this.gameObject.SetActive(false);
    }

    void Boost(GameObject go)
    {
        MyCharacterController cc = go.GetComponent<MyCharacterController>();
        cc.Boost(BoostSpeed, BoostTime_Seconds);
    }
}
