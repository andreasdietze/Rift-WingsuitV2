using UnityEngine;
using System.Collections;

public class AdjustingPanel : MonoBehaviour {

    public static AdjustingPanel instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        // für Zugriff zum Aktivieren und Deaktivieren; GameObject.Find() findet keine inaktiven Objekte und ist langsam!
    }
}
