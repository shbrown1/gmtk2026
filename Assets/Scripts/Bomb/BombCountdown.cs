using TMPro;
using UnityEngine;

public class BombCountdown : MonoBehaviour
{
    private TMP_Text countdownText;

    void Start()
    {
        countdownText = GetComponent<TMP_Text>();
    }
}
