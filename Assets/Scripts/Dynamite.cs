using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    private TimerScript timer;
    [SerializeField] private int countdownTime = 30;
    [SerializeField] private List<CuttableWire> wires;
    void Start()
    {
        timer = GetComponentInChildren<TimerScript>();
        timer.Init(countdownTime);
    }

}
