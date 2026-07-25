using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    //mostly copied from BombCountdown, id delete but keeping in just in case for the time being.
    private TMP_Text countdownText;
    private float timeRemaining;
    private bool isTimerRunning = false;

    void Start()
    {
        countdownText = GetComponentInChildren<TMP_Text>();
    }

    public void Init(int seconds)
    {
        if (GameController.instance.timerEnabled)
        {
            timeRemaining = seconds;
            isTimerRunning = true;
        }
    }

    void Update()
    {
        if(isTimerRunning)
        {
            if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime();
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                OnTimerFinish();
            }
        } 
        else
        {
            countdownText.color = Color.green;
        }
    }

    public void DefuseBomb()
    {
        isTimerRunning = false;
    }

    private void DisplayTime()
    {
        float seconds = Mathf.CeilToInt(timeRemaining);
        if (Mathf.Approximately(seconds, 0))
        {
            countdownText.text = "BOOM!";
        }
        else
        {
            countdownText.text = seconds.ToString();
        }

    }

    private void OnTimerFinish()
    {
        Debug.Log("TODO: Implement game over state");
    }
}
