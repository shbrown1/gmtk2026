using System.Collections;
using UnityEngine;

public class Radio : MonoBehaviour
{
    private bool musicPlaying = false;
    [SerializeField] private AudioClip radioSong;
    [SerializeField] private Battery battery1;
    [SerializeField] private Battery battery2;
    [SerializeField] private AudioClip _loadingSound;
    [SerializeField] private CassetteTape cassette;
    [SerializeField] private CassetteHolderClickable cassetteHolder;

    void Update()
    {
        if (battery1.inserted && battery2.inserted && cassette.inserted && !musicPlaying && !cassetteHolder.IsOpen) StartCoroutine(PlaySong());
    }

    private IEnumerator PlaySong()
    {
        musicPlaying = true;
        AudioController.instance.PlaySound(_loadingSound, .5f);
        yield return new WaitForSeconds(_loadingSound.length);
        AudioController.instance.PlayBackgroundMusic(radioSong);
    }
}
