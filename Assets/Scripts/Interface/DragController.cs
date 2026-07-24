using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] private LayerMask fillerLayerMask = ~0;

    private Camera cam;
    private FillerObject currentDragging;

    private void Awake() => cam = Camera.main;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryBeginDrag();
        }
        else if (currentDragging != null && Input.GetMouseButton(0))
        {
            currentDragging.UpdateDrag(Input.mousePosition);
        }
        else if (currentDragging != null && Input.GetMouseButtonUp(0))
        {
            currentDragging.EndDrag();
            currentDragging = null;
        }
    }

    private void TryBeginDrag()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, fillerLayerMask))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");
            FillerObject filler = hit.collider.GetComponent<FillerObject>();
            if (filler != null)
            {
                Debug.Log($"Begin dragging {filler.name}");
                currentDragging = filler;
                filler.BeginDrag();
            }
        }
    }
}