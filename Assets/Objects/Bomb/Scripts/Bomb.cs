using UnityEngine;

public class Bomb : MonoBehaviour
{
    private BombCountdown bombCountdown;
    [SerializeField] private int bombCountDownLength;

    void Start()
    {
        //trying to set this up where all the elements of the bomb are controlled from the bomb object, and then passed down, maybe this is a bad design decision but idk
        //The idea is you can just create a bomb object in the editor and configure it from there, then it passes info into child objects/puzzles, easier for building a bunch of levels ideally
        bombCountdown = GetComponentInChildren<BombCountdown>();
        bombCountdown.Init(bombCountDownLength);
    }
}
