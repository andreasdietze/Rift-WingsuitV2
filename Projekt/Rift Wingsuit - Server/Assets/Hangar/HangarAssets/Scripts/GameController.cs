using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance = null;
    public string ip;
    public int currentScene;
    public float xAxisAdjust, yAxisAdjust;
    GameObject x, y;
    Slider sX, sY;
    public bool paused;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        currentScene = 0;
        xAxisAdjust = 0.5f;//auf sinnvollen Startwert zu ändern (später auf File auslagern)
        yAxisAdjust = 0.5f;//auf sinnvollen Startwert zu ändern (später auf File auslagern)
        paused = false;
    }

    void Update()
    {
        //controls
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            switch(currentScene)
            {
                case 0:
                    Application.Quit();
                    break;
                case 1:
                    SceneManager.LoadScene(0);
                    currentScene = 0;
                    Cursor.visible = true;
                    break;
            }
        }

        if(Input.GetKeyDown(KeyCode.Pause) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            switch(currentScene)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    paused = !paused;
                    break;
            }
        }



        //scenes
        switch(currentScene)
        {
            case 0:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case 1:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                
                if(x == null || y == null)
                {
                    x = GameObject.Find("SliderX");
                    y = GameObject.Find("SliderY");
                    
                }
                else if (x != null && y != null)
                {
                    sX = x.GetComponent<Slider>();
                    sY = y.GetComponent<Slider>();
                    sX.value = xAxisAdjust;
                    sY.value = yAxisAdjust;
                }
                break;
            case 2:
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}