using UnityEngine;

public class Dynamite : MonoBehaviour
{
    private TimerScript timer;
    private int currentIndex = 0;
    private bool isDefused;
    private bool isDetonated;

    [SerializeField] private int countdownTime = 30;
    [SerializeField] private GameObject fireWall;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip fireSound;


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

    void Update()
    {
        if (timer.isDone && !isDetonated) Detonate();
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
        explosion.Play();
        fireWall.SetActive(true);
        gameObject.SetActive(false);
        AudioController.instance.PlaySound(fireSound);
        AudioController.instance.PlaySound(explosionSound);
    }
}
