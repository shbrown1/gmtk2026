using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    [SerializeField] private AudioSource _soundEffectSource;
    [SerializeField] private AudioSource _backgroundMusicSource;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if(clip is null)
            return;

        _soundEffectSource.PlayOneShot(clip, volume);
    }

    public void PlayBackgroundMusic(AudioClip clip, float volume = 1f)
    {
        if(clip is null)
            return;

        if (_backgroundMusicSource.clip == clip && _backgroundMusicSource.isPlaying)
            return;

        _backgroundMusicSource.clip = clip;
        _backgroundMusicSource.volume = volume;
        _backgroundMusicSource.loop = true;
        _backgroundMusicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        _backgroundMusicSource.Stop();
        _backgroundMusicSource.clip = null;
        
    }
}
