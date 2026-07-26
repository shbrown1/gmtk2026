using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dynamite : MonoBehaviour
{
    private TimerScript timer;
    private int currentIndex = 0;
    private bool isDefused;
    private bool isDetonated;

    [SerializeField] private int countdownTime = 30;
    private Firewall fireWall;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip fastMusic;


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
        fireWall = FindAnyObjectByType<Firewall>(FindObjectsInactive.Include);
        AudioController.instance.PlayBackgroundMusic(fastMusic, 0.7f);
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
        StartCoroutine(GoToWinScreen());
    }

    private void Detonate()
    {
        isDetonated = true;
        explosion.Play();
        fireWall.gameObject.SetActive(true);
        StartCoroutine(GoToGameOverScreen());
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        AudioController.instance.PlaySound(fireSound);
        AudioController.instance.PlaySound(explosionSound);
    }

    private IEnumerator GoToGameOverScreen()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Should go to game over screen");
        SceneManager.LoadScene("Game Over Screen");
    }

    private IEnumerator GoToWinScreen()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Winner Screen");
    }
}
