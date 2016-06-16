using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    public float RotationSpeed = 1;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Bunny got the collectible!");
        PerformAction(other);

    }
        // Update is called once per frame
    void Update ()
    {
        Rotate();
    }

    void Rotate()
    {
        //Rotate
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + RotationSpeed, 0);
    }

    public virtual void PerformAction(Collider collision)
    {
        this.gameObject.SetActive(false);
        //this.enabled = false;
    }
}
