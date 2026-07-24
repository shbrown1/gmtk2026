using UnityEngine;
using UnityEngine.EventSystems;

public class RotatableKnob : MonoBehaviour
{
    [SerializeField] private bool useLimits = true;
    [SerializeField] private float minAngle = -135f;
    [SerializeField] private float maxAngle = 135f;
    private Camera mainCamera;

    public float Value { get; private set; } //returns between 0 -> 1 based on current rotation
    void Start()
    {
        mainCamera = Camera.main;
        Debug.Log(mainCamera);
    }

    private void RotateKnob()
    {
        Vector2 knobScreenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 direction = (Vector2)Input.mousePosition - knobScreenPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        if (useLimits)
        {
            angle = Mathf.Clamp(angle, minAngle, maxAngle);
            Value = Mathf.InverseLerp(minAngle, maxAngle, angle);
        }
        else
        {
            Value = Mathf.InverseLerp(-180f, 180f, angle);
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void OnMouseDown()
    {
        Debug.Log("TRY ROTATE");
        RotateKnob();
    }

    public void OnMouseDrag()
    {
        RotateKnob();
    }
}
