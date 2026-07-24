using System.Collections;
using UnityEngine;

public class FillerObject : MonoBehaviour, IDraggable
{
    [SerializeField] private string fillerType = "Default";
    [SerializeField] private float snapDuration = 0.15f;

    public string FillerType => fillerType;
    public bool IsPlaced => currentSlot != null;

    private Transform homeParent;
    private Vector3 homeLocalPos;   // relative to homeParent — stable under rotation
    private ContainerSlot currentSlot;
    private Plane dragPlane;
    private Camera cam;
    private Coroutine moveRoutine;

    private void Awake()
    {
        cam = Camera.main;
        homeParent = transform.parent;
        homeLocalPos = transform.localPosition;
    }

    private Vector3 HomeWorldPos =>
        homeParent != null ? homeParent.TransformPoint(homeLocalPos) : homeLocalPos;

    public void BeginDrag()
    {
        if (moveRoutine != null) { StopCoroutine(moveRoutine); moveRoutine = null; }

        if (currentSlot != null)
        {
            currentSlot.RemoveFiller();
            currentSlot = null;
        }

        dragPlane = new Plane(-cam.transform.forward, transform.position);
    }

    public void UpdateDrag(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        if (dragPlane.Raycast(ray, out float enter))
            transform.position = ray.GetPoint(enter);
    }

    public void EndDrag()
    {
        ContainerSlot best = FindBestContainer();
        if (best != null && best.TryAccept(this))
        {
            currentSlot = best;
            // Reparent so it stays visually attached to its new container,
            // and continues to move/rotate correctly with it afterward.
            transform.SetParent(best.SlotTransform, worldPositionStays: true);
            SnapTo(best.SlotTransform.position);
        }
        else
        {
            // Return home — reparent back first so future rotations
            // of the original container carry this object correctly.
            transform.SetParent(homeParent, worldPositionStays: true);
            //SnapTo(HomeWorldPos);
        }
    }

    private ContainerSlot FindBestContainer()
    {
        ContainerSlot best = null;
        float bestDist = float.MaxValue;

        foreach (var slot in ContainerSlot.All)
        {
            if (slot.IsOccupied || slot.AcceptedType != fillerType) continue;

            float dist = Vector3.Distance(transform.position, slot.SlotTransform.position);
            if (dist <= slot.CatchRadius && dist < bestDist)
            {
                bestDist = dist;
                best = slot;
            }
        }
        return best;
    }

    private void SnapTo(Vector3 targetWorldPos)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveTo(targetWorldPos));
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float t = 0f;
        while (t < snapDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, Mathf.SmoothStep(0f, 1f, t / snapDuration));
            yield return null;
        }
        transform.position = target;
    }
}