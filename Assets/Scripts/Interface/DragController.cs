using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] private LayerMask fillerLayerMask = ~0;

    private Camera cam;
    private IDraggable[] currentDragging;

    private void Awake() => cam = Camera.main;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryBeginDrag();
        }
        else if (currentDragging != null && Input.GetMouseButton(0))
        {
            foreach (var d in currentDragging) d.UpdateDrag(Input.mousePosition);
        }
        else if (currentDragging != null && Input.GetMouseButtonUp(0))
        {
            foreach (var d in currentDragging) d.EndDrag();
            currentDragging = null;
        }
    }

    private void TryBeginDrag()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, fillerLayerMask))
        {
            IDraggable[] draggables = hit.collider.GetComponentsInParent<IDraggable>();
            if (draggables.Length > 0)
            {
                currentDragging = draggables;
                foreach (var d in draggables) d.BeginDrag();
            }
        }
    }
}