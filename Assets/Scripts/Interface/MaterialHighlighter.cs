using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialHighlighter : MonoBehaviour, IHoverable
{
    [SerializeField] private float brightnessBoost = 0.3f;

    private Renderer objectRenderer;
    private Color originalColor;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    public void OnHoverEnter()
    {
        objectRenderer.material.color = originalColor + new Color(brightnessBoost, brightnessBoost, brightnessBoost, 0f);
    }

    public void OnHoverExit()
    {
        objectRenderer.material.color = originalColor;
    }
}