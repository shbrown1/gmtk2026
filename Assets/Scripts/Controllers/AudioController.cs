using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    [SerializeField] private AudioSource source;

    void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
