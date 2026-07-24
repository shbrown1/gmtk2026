using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject ClickToStartText;

    float _blinkOnDuration = 1f;
    float _blinkOffDuration = 1f;

    float _blinkTimer;
    bool _textVisible = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }

        _blinkTimer += Time.deltaTime;
        float interval = _textVisible ? _blinkOnDuration : _blinkOffDuration;
        if (_blinkTimer >= interval)
        {
            _blinkTimer = 0f;
            _textVisible = !_textVisible;
            ClickToStartText.SetActive(_textVisible);
        }
    }
}
