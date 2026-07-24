using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private BombCountdown bombCountdown;
    private readonly List<IPuzzle> puzzles = new();
    [SerializeField] private int bombCountDownLength;
    [SerializeField] private List<GameObject> puzzleObjects = new();

    void Start()
    {
        bombCountdown = GetComponentInChildren<BombCountdown>();
        bombCountdown.Init(bombCountDownLength);

        puzzles.Clear();

        foreach (var puz in puzzleObjects)
        {
            if (puz.TryGetComponent(out IPuzzle puzzle))
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
        bombCountdown.DefuseBomb();
        Debug.Log("BOMB DEFUSED!");
    }
}