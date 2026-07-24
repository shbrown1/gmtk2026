using UnityEngine;

public class RotatableKnob : MonoBehaviour
{
    [SerializeField] private bool useLimits = true;
    [SerializeField] private float minAngle = -135f;
    [SerializeField] private float maxAngle = 135f;

    private Camera mainCamera;
    private bool isDragging;

    public float Value { get; private set; } // 0 -> 1

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No Main Camera found!");
        }
    }

    private void Update()
    {
        // Begin dragging if we clicked this object
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    RotateKnob();
                }
            }
        }

        // Rotate while dragging
        if (isDragging && Input.GetMouseButton(0))
        {
            RotateKnob();
        }

        // Stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
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
            angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
            Value = Mathf.InverseLerp(-180f, 180f, angle);
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}