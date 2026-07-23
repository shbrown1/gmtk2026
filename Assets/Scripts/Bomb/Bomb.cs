using UnityEngine;

public class Bomb : MonoBehaviour
{
    private BombCountdown bombCountdown;

    void Start()
    {
        bombCountdown = GetComponentInChildren<BombCountdown>();
    }
}
