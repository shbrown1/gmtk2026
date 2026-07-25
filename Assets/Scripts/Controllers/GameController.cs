using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    //for debugging/development
    //this setup lets you set the _value in the editor while the public value remains readonly
    [SerializeField] private bool _timerEnabled;
    public bool timerEnabled => _timerEnabled;

    public static GameController instance;

    [SerializeField] private float startingTime = 60f;
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private bool startOnAwake = true;

    public event Action OnTimerFinished;

    private float timeRemaining;
    private bool isRunning;

    public bool IsRunning => isRunning;
    public float TimeRemaining => timeRemaining;

    private void Awake()
    {
        instance = this;

        timeRemaining = startingTime;
        UpdateDisplay();

        if (startOnAwake) StartTimer();
    }

    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            UpdateDisplay();
            OnTimerFinished?.Invoke();
            return;
        }

        UpdateDisplay();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer(bool autoStart = false)
    {
        timeRemaining = startingTime;
        isRunning = autoStart;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (displayText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        displayText.text = $"{minutes:00}:{seconds:00}";
    }


}
