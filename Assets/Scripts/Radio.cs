using System.Collections;
using UnityEngine;

public class Radio : MonoBehaviour
{
    private bool musicPlaying = false;
    [SerializeField] private AudioClip radioSong;
    [SerializeField] private Battery battery1;
    [SerializeField] private Battery battery2;
    [SerializeField] private CassetteTape cassette;
    [SerializeField] private CassetteHolderClickable cassetteHolder;

    void Update()
    {
        if (battery1.inserted && battery2.inserted && cassette.inserted && !musicPlaying && !cassetteHolder.IsOpen) StartCoroutine(PlaySong());
    }

    private IEnumerator PlaySong()
    {
        musicPlaying = true;
        yield return new WaitForSeconds(1.15f);
        AudioController.instance.PlayBackgroundMusic(radioSong);
    }
}
