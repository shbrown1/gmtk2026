using System.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour, IDraggable
{
    [SerializeField] float _snapDuration = 0.15f;

    public Vector3 homePosition;
    public Vector3 homeRotation;
    Plane _dragPlane;
    Camera _camera;
    Coroutine _snapRoutine;

    void Awake()
    {
        _camera = Camera.main;
        homePosition = transform.position;
        homeRotation = transform.eulerAngles;
    }

    public void BeginDrag()
    {
        if (_snapRoutine != null) { StopCoroutine(_snapRoutine); _snapRoutine = null; }
        _dragPlane = new Plane(Vector3.back, transform.position);
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
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(homeRotation);
        float t = 0f;
        while (t < _snapDuration)
        {
            t += Time.deltaTime;
            float s = Mathf.SmoothStep(0f, 1f, t / _snapDuration);
            transform.position = Vector3.Lerp(startPos, homePosition, s);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, s);
            yield return null;
        }
        transform.position = homePosition;
        transform.rotation = targetRot;
    }
}
