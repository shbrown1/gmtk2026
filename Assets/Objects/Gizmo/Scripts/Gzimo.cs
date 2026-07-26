using TMPro;
using UnityEngine;

public class Gzimo : MonoBehaviour
{
    [SerializeField] private TMP_Text timerDisplay;

    private float _elapsed;
    private bool _running;

    public void StartElapsedTimer()
    {
        _elapsed = 0f;
        _running = true;
        timerDisplay.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!_running) return;
        _elapsed += Time.deltaTime;
        timerDisplay.text = _elapsed.ToString("F2");
    }
}
