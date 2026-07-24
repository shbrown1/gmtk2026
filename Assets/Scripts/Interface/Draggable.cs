using System.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour, IDraggable
{
    [SerializeField] float _snapDuration = 0.15f;

    public Vector3 _homePosition;
    Plane _dragPlane;
    Camera _camera;
    Coroutine _snapRoutine;

    void Awake()
    {
        _camera = Camera.main;
    }

    public void BeginDrag()
    {
        if (_snapRoutine != null) { StopCoroutine(_snapRoutine); _snapRoutine = null; }
        _dragPlane = new Plane(-_camera.transform.forward, transform.position);
    }

    public void UpdateDrag(Vector2 screenPos)
    {
        Ray ray = _camera.ScreenPointToRay(screenPos);
        if (_dragPlane.Raycast(ray, out float enter))
            transform.position = ray.GetPoint(enter);
    }

    public void EndDrag()
    {
        _snapRoutine = StartCoroutine(SnapHome());
    }

    IEnumerator SnapHome()
    {
        Vector3 start = transform.position;
        float t = 0f;
        while (t < _snapDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, _homePosition, Mathf.SmoothStep(0f, 1f, t / _snapDuration));
            yield return null;
        }
        transform.position = _homePosition;
    }
}
