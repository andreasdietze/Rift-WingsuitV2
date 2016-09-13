using UnityEngine;
using System.Collections;
using System;

public class RWS_UIController : MonoBehaviour {

    public static bool timerIsRunning = false;
    public static bool timerFinish = false;

    StartLevelRayCaster slrc;
    public static int updraftsUsed = 0;
    public static int boostersUsed = 0;
    public static int ringScore = 0;
    public static DateTime startTime;
    public static String time;

    public TextMesh timerText;
    public TextMesh ringsText;

    public TextMesh gameOverText;

    public TextMesh scoreBoardText;
    public TextMesh totalScore;

    public GameObject HUD;
    public GameObject ScoreBoard;

    private Player player;
    public NetworkManager nm;

    public static int pointsPerUpdraft = 2;
    public static int pointsPerBooster = 3;
    public static int pointsPerRing = 1;
    public static int basePoints = 100;

    // Use this for initialization
    void Start () {
        timerText = timerText.gameObject.GetComponent<TextMesh>();
        if (timerText == null)
        {
            Debug.LogError("Timer Text is null!");
        }

        ringsText = ringsText.gameObject.GetComponent<TextMesh>();
        if (ringsText == null)
        {
            Debug.LogError("rings Text is null!");
        }

        gameOverText = gameOverText.gameObject.GetComponent<TextMesh>();
        if (gameOverText == null)
        {
            Debug.LogError("gameOverText Text is null!");
        }

        scoreBoardText = scoreBoardText.gameObject.GetComponent<TextMesh>();
        if (scoreBoardText == null)
        {
            Debug.LogError("scoreBoardText Text is null!");
        }

        totalScore = totalScore.gameObject.GetComponent<TextMesh>();
        if (totalScore == null)
        {
            Debug.LogError("totalScore Text is null!");
        }

        gameOverText.text = "";
        HUD.SetActive(true);
        ScoreBoard.SetActive(false);

        RWS_UIController.Reset();
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (slrc.startGame)
        {
            RWS_UIController.timerIsRunning = true;
        }
        */
        if (nm.serverInitiated && player == null)
        {
            player = (Player)GameObject.FindGameObjectWithTag("Player").GetComponent("Player");
        }
        if (RWS_UIController.timerIsRunning && !RWS_UIController.timerFinish)
        {
            if (player != null)
            {
                player.score = RWS_UIController.ringScore;
            }
            
            TimeSpan diffTime = RWS_UIController.startTime - DateTime.Now;
            diffTime = diffTime.Duration();
            RWS_UIController.time = string.Format("{0:mm\\:ss\\:ff}", new DateTime(diffTime.Ticks));
        }
	}

    public void StartTimer()
    {
        RWS_UIController.startTime = DateTime.Now;
        RWS_UIController.timerIsRunning = true;
    }

    public void GameOver()
    {
        HUD.SetActive(false);
        gameOverText.text = "Game Over!!!";
    }

    public void Finish()
    {
        RWS_UIController.StopTimer();
        HUD.SetActive(false);
        scoreBoardText.text = RWS_UIController.GetScoreBoardText();
        totalScore.text = RWS_UIController.GetTotalScore().ToString();
        ScoreBoard.SetActive(true);
    }

    void FixedUpdate()
    {
        timerText.text = RWS_UIController.time;
        ringsText.text = RWS_UIController.ringScore.ToString();
    }

    public static void StopTimer()
    {
        RWS_UIController.timerFinish = true;
    }

    public static void Reset()
    {
        RWS_UIController.updraftsUsed = 0;
        RWS_UIController.boostersUsed = 0;
        RWS_UIController.ringScore = 0;
        RWS_UIController.time = "00:00:00";
        RWS_UIController.timerIsRunning = false;
        RWS_UIController.timerFinish = false;
    }

    public static int GetTotalScore()
    {

        return RWS_UIController.updraftsUsed * RWS_UIController.pointsPerUpdraft +
               RWS_UIController.boostersUsed * RWS_UIController.pointsPerUpdraft +
               RWS_UIController.ringScore * RWS_UIController.pointsPerRing;
    }

    public static String GetScoreBoardText()
    {
        return " Time: " + RWS_UIController.time +
                "\n \n Rings: " + RWS_UIController.ringScore +
                "\n \n Updrafts: " + RWS_UIController.updraftsUsed +
                "\n \n Boosters: " + RWS_UIController.boostersUsed;
    }

}
