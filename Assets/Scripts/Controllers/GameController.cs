using UnityEngine;

public class GameController : MonoBehaviour
{
    //for debugging/development
    //this setup lets you set the _value in the editor while the public value remains readonly
    [SerializeField] private bool _timerEnabled;
    public bool timerEnabled => _timerEnabled;

    public static GameController instance;

    void Awake()
    {
        instance = this;
    }

}
