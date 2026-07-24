using UnityEngine;

public class DefuseBombPuzzle : MonoBehaviour, IPuzzle
{
    private bool isSolved;
    public bool IsSolved => isSolved;

    public void OnMouseDown()
    {
        isSolved = true;
    }
}
