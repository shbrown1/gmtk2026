using System.Collections;
using System.Linq;
using UnityEngine;

public class RemovableObject : MonoBehaviour, IClickable
{
    [SerializeField] private Transform bigObject; // assign in Inspector, or auto-found in Awake
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveDuration = 0.25f;

    private bool _isRemoved = false;
    private bool isAnimating = false;

    private Vector3 restingLocalPos;
    private Vector3 poppedLocalPos;
    private Coroutine moveRoutine;

    private void Awake()
    {
        if (bigObject == null)
            bigObject = GetComponentInParent<RotatableObject>()?.transform;
    }

    private void Start()
    {
        restingLocalPos = transform.localPosition;

        if (bigObject == null)
        {
            Debug.LogWarning($"{name}: No BigObject reference set, cannot compute pop direction.");
            poppedLocalPos = restingLocalPos;
            return;
        }

        // Direction from BigObject's center to this child, in world space.
        Vector3 worldOutward = (transform.position - bigObject.position).normalized;

        // Convert that world-space direction into the parent's local space,
        // since we animate via localPosition.
        Vector3 localOutward = transform.parent != null
            ? transform.parent.InverseTransformDirection(worldOutward)
            : worldOutward;

        poppedLocalPos = restingLocalPos + localOutward * moveDistance;
    }

    public void OnClick()
    {
        if (isAnimating) return;

        RemovableObject[] removableChildren = GetComponentsInChildren<RemovableObject>().Where(comp => comp.gameObject != this.gameObject).ToArray();

        if (removableChildren.Length > 0)
        {
            // Checks children to ensure they've been removed
            foreach (RemovableObject child in removableChildren)
            {
                if (!child.IsRemoved) return;
            }
        }

        Vector3 target = _isRemoved ? restingLocalPos : poppedLocalPos;
        transform.localScale *= _isRemoved ? (1 / 1.2f) : 1.2f; // Scale down when popping out, scale up when returning
        _isRemoved = !_isRemoved;

        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveTo(target));
    }

    private IEnumerator MoveTo(Vector3 targetLocalPos)
    {
        isAnimating = true;
        Vector3 start = transform.localPosition;
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float pct = Mathf.SmoothStep(0f, 1f, t / moveDuration);
            transform.localPosition = Vector3.Lerp(start, targetLocalPos, pct);
            yield return null;
        }

        transform.localPosition = targetLocalPos;
        isAnimating = false;
    }

    public bool IsRemoved { get { return _isRemoved; } }
}