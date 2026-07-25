using UnityEngine;

public class Dynamite : MonoBehaviour
{
    private TimerScript timer;
    private int currentIndex = 0;
    private bool isDefused;
    private bool isDetonated;

    [SerializeField] private int countdownTime = 30;

    private readonly WireColor[] correctOrder =
    {
        WireColor.Blue,
        WireColor.Purple,
        WireColor.Red
    };

    private void Start()
    {
        timer = GetComponentInChildren<TimerScript>();
        timer.Init(countdownTime);
    }

    public void OnWireCut(CuttableWire wire)
    {
        CheckCutWires(wire);
    }

    private bool CheckCutWires(CuttableWire cutWire)
    {
        if (isDefused || isDetonated)
            return false;

        if (cutWire.color != correctOrder[currentIndex])
        {
            Detonate();
            return false;
        }

        currentIndex++;

        if (currentIndex >= correctOrder.Length)
        {
            Defuse();
            return true;
        }

        return false;
    }

    private void Defuse()
    {
        isDefused = true;
        timer.DefuseBomb();
    }

    private void Detonate()
    {
        isDetonated = true;
        Debug.Log("Wrong wire cut! Boom!");
    }
}
