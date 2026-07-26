using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        bool allScrewsInserted = FindObjectsByType<Screw>().All(s => !s.IsRemoved);
        if (battery1.inserted && battery2.inserted && cassette.inserted && allScrewsInserted && !musicPlaying && !cassetteHolder.IsOpen) StartCoroutine(PlaySong());
    }

    private IEnumerator PlaySong()
    {
        musicPlaying = true;
        AudioController.instance.PlaySound(_loadingSound, .5f);
        yield return new WaitForSeconds(_loadingSound.length);
        AudioController.instance.PlayBackgroundMusic(radioSong);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GizmoScene");
    }
}
