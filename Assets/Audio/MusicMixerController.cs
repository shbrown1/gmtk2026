using UnityEngine;
using UnityEngine.Audio;

public class MusicMixerController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioMixer audioMixer;
    public float targetSpeed = 1f;

    void Start()
    {
        audioSource.pitch = targetSpeed;
    }

    private void Update()
    {
        audioSource.pitch = targetSpeed;

        float pitchInversion = 1f / targetSpeed;
        audioMixer.SetFloat("Pitch", pitchInversion);
    }

    public void SetTargetSpeed(float speed)
    {
        targetSpeed = speed;
    }
}
