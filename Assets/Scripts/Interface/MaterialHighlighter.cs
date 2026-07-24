using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialHighlighter : MonoBehaviour, IHoverable
{
    [SerializeField] private Color highlightColor = Color.yellow;

    private Renderer objectRenderer;
    private Color originalColor;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    public void OnHoverEnter()
    {
        objectRenderer.material.color = highlightColor;
    }

    public void OnHoverExit()
    {

        objectRenderer.material.color = originalColor;
    }
}