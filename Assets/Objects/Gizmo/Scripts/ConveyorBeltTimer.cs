using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConveyorBeltTimer : MonoBehaviour
{
    [SerializeField] private float seconds = 60f;
    private TMP_Text timerText;
    private float timeRemaining;
    private bool isRunning = false;
    private ConveyorBelt _conveyorBelt;
    private MusicMixerController _musicMixerController;
    float timeSinceLastSpeedIncrease;

    void Awake()
    {
        timerText = GetComponentInChildren<TMP_Text>();
        timeRemaining = seconds;
    }

    void Start()
    {
        isRunning = true;
        DisplayTime(timeRemaining);

        _musicMixerController = FindAnyObjectByType<MusicMixerController>();
        timeSinceLastSpeedIncrease = Time.time;
        _conveyorBelt = FindAnyObjectByType<ConveyorBelt>();
    }

    void Update()
    {
        if (!isRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
            if (timeRemaining != seconds && (Time.time - timeSinceLastSpeedIncrease) >= 30f)
            {
                _musicMixerController.SetTargetSpeed(_musicMixerController.targetSpeed + 0.2f);
                timeSinceLastSpeedIncrease = Time.time;
            }
        }
        else if(_conveyorBelt.GetRemainingCount() > 0)
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
        timerText.color = Color.green;
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
        SceneManager.LoadScene("Game Over Screen");
    }
}
