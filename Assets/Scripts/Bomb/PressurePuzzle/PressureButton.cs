using UnityEngine;

public class PressureButton : MonoBehaviour
{
    private PressureGaugePuzzle parent;
    void Start()
    {
        parent = GetComponentInParent<PressureGaugePuzzle>();
        if (parent is null)
        {
            Debug.LogError("Pressure Button has no Parent");
        }
    }
    void OnMouseUp()
    {
        parent.IncreasePressure();
    }
}
