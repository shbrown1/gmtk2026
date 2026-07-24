using TMPro;
using UnityEngine;

public class BombCountdown : MonoBehaviour
{
    private TMP_Text countdownText;
    private float timeRemaining;
    private bool isTimerRunning = false;

    void Start()
    {
        countdownText = GetComponent<TMP_Text>();
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
        countdownText.text = seconds.ToString();

    }

    private void OnTimerFinish()
    {
        Debug.Log("TODO: Implement game over state");
    }
}
