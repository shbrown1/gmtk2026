using TMPro;
using UnityEngine;

public class PressureGaugePuzzle : MonoBehaviour, IPuzzle
{
    [SerializeField] private float currentPressure = 0.5f;
    [SerializeField] private float minAccepted = 0.35f;
    [SerializeField] private float maxAccepted = 0.65f;
    [SerializeField] private float pressureIncrease = 0.05f;
    [SerializeField] private float pressureLossPerSecond = 0.01f;
    [SerializeField] private TMP_Text gauge;
    
    private bool isSolved;
    public bool IsSolved => isSolved;

    void Update()
    {
        currentPressure -= pressureLossPerSecond * Time.deltaTime;
        currentPressure = Mathf.Clamp01(currentPressure);
        isSolved = currentPressure >= minAccepted && currentPressure <= maxAccepted;

        gauge.text = currentPressure.ToString("0.000");
        if (isSolved)
        {
            gauge.color = Color.green;
        }
        else
        {
            gauge.color = Color.black;
        }
    }

    public void IncreasePressure()
    {
        currentPressure += pressureIncrease;
    }


}