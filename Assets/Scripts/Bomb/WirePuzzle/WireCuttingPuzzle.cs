using System.Collections.Generic;
using UnityEngine;

public class WireCuttingPuzzle : MonoBehaviour, IPuzzle
{
    private bool isSolved;
    public bool IsSolved => isSolved;

    [SerializeField] private List<Wire> wires = new();

    void Update()
    {
        CheckForDefuse();
    }

    private void CheckForDefuse()
    {
        foreach (Wire wire in wires)
        {
            if (!wire.isWireCut && wire.isWireToCut)
            {
                return;
            }
        }

        isSolved = true;
    }
}
