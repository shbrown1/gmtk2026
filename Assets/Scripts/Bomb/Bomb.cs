using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private BombCountdown bombCountdown;
    private readonly List<IPuzzle> puzzles = new();
    [SerializeField] private int bombCountDownLength;
    [SerializeField] private List<MonoBehaviour> puzzleObjects = new();

    void Start()
    {
        bombCountdown = GetComponentInChildren<BombCountdown>();
        bombCountdown.Init(bombCountDownLength);

        puzzles.Clear();

        foreach (var puz in puzzleObjects)
        {
            if (puz is IPuzzle puzzle)
            {
                puzzles.Add(puzzle);
            }
        }
    }

    private void Update()
    {
        if (puzzles.Count > 0 && puzzles.TrueForAll(p => p.IsSolved))
        {
            DefuseBomb();
        }
    }

    private void DefuseBomb()
    {
        Debug.Log("BOMB DEFUSED!");
    }
}
