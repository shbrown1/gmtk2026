using System.Collections;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] float _blackScreenDuration = 2f;
    [SerializeField] AudioClip _lightSwitchSound;
    [SerializeField] AudioClip _officeAmbience;

    Camera _cam;
    GameObject _overlay;

    void Awake()
    {
        _cam = Camera.main;
        CreateOverlay();
    }

    void Start()
    {
        StartCoroutine(RevealAfterDelay());
    }

    void Update()
    {

    }

    void CreateOverlay()
    {
        _overlay = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(_overlay.GetComponent<Collider>());
        _overlay.transform.SetParent(_cam.transform);

        float dist = _cam.nearClipPlane + 0.001f;
        float h = 2f * dist * Mathf.Tan(_cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float w = h * _cam.aspect;

        _overlay.transform.localPosition = new Vector3(0f, 0f, dist);
        _overlay.transform.localRotation = Quaternion.identity;
        _overlay.transform.localScale = new Vector3(w, h, 1f);

        var mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = Color.black;
        _overlay.GetComponent<Renderer>().material = mat;
    }

    IEnumerator RevealAfterDelay()
    {
        yield return new WaitForSeconds(_blackScreenDuration);
        _overlay.SetActive(false);

        AudioController.instance.PlaySound(_lightSwitchSound, 0.5f);
        AudioController.instance.PlayBackgroundMusic(_officeAmbience, 0.5f);
    }
}
