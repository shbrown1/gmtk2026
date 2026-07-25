using TMPro;
using UnityEngine;

public class ConveyorBeltTimer : MonoBehaviour
{
    [SerializeField] private float seconds = 60f;
    private TMP_Text timerText;
    private float timeRemaining;
    private bool isRunning = false;

    void Awake()
    {
        timerText = GetComponentInChildren<TMP_Text>();
        timeRemaining = seconds;
    }

    void Start()
    {
        isRunning = true;
        DisplayTime(timeRemaining);
    }

    void Update()
    {
        if (!isRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            isRunning = false;
            DisplayTime(0);
            OnTimerFinished();
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void DisplayTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int secs = Mathf.CeilToInt(time % 60f);
        if (secs == 60) { minutes++; secs = 0; }
        timerText.text = string.Format("{0}:{1:00}", minutes, secs);
    }

    private void OnTimerFinished()
    {
        Debug.Log("Conveyor belt timer finished!");
    }
}
