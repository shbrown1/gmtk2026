using UnityEngine;

public class GameController : MonoBehaviour
{
    //for debugging/development
    public readonly bool timerEnabled = false;

    public static GameController instance;

    void Awake()
    {
        instance = this;
    }

}
