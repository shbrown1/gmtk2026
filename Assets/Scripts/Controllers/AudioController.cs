using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    [SerializeField] private AudioSource _soundEffectSource;
    [SerializeField] private AudioSource _backgroundMusicSource;

    void Awake()
    {
        instance = this;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if(clip == null)
            return;

        _soundEffectSource.PlayOneShot(clip, volume);
    }

    public void PlayBackgroundMusic(AudioClip clip, float volume = 1f)
    {
        if(clip == null)
            return;

        _backgroundMusicSource.clip = clip;
        _backgroundMusicSource.volume = volume;
        _backgroundMusicSource.Play();
    }
}
