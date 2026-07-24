using System.Collections;
using UnityEngine;

public class CassetteTape : MonoBehaviour, IDraggable
{
    [SerializeField] float _alignRadius = 2f;
    [SerializeField] float _fullAlignDistance = 1.5f;
    [SerializeField] float _insertDuration = 0.4f;
    [SerializeField] AnimationCurve _insertCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    CassetteHolderClickable _cassetteHolder;
    Quaternion _homeRotation;
    bool _inserted;

    void Awake()
    {
        _cassetteHolder = FindFirstObjectByType<CassetteHolderClickable>();
    }

    void Start()
    {
        _homeRotation = transform.rotation;
    }

    public void BeginDrag()
    {
        if (_inserted) return;
    }

    public void UpdateDrag(Vector2 screenPos)
    {
        if (_inserted) return;
        if (_cassetteHolder == null || !_cassetteHolder.IsOpen) return;

        float dist = Vector3.Distance(transform.position, _cassetteHolder.cassettePlacementTransform.position);
        float t = Mathf.Clamp01(Mathf.InverseLerp(_alignRadius, _fullAlignDistance, dist));

        transform.position = Vector3.Lerp(transform.position, _cassetteHolder.cassettePlacementTransform.position, t);

        Quaternion target = _cassetteHolder.cassettePlacementTransform.rotation;
        if (Quaternion.Dot(_homeRotation, target) < 0f)
            target = new Quaternion(-target.x, -target.y, -target.z, -target.w);
        transform.rotation = Quaternion.Slerp(_homeRotation, target, t);
    }

    public void EndDrag()
    {
        if (_inserted || _cassetteHolder.cassettePlacementTransform == null) return;

        float dist = Vector3.Distance(transform.position, _cassetteHolder.cassettePlacementTransform.position);
        if (dist <= _fullAlignDistance)
            StartCoroutine(InsertCassette());
    }

    IEnumerator InsertCassette()
    {
        _inserted = true;

        Draggable draggable = GetComponent<Draggable>();
        if (draggable != null) Destroy(draggable);

        Vector3 worldScale = transform.lossyScale;
        transform.SetParent(_cassetteHolder.cassettePlacementTransform);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        float elapsed = 0f;
        Vector3 endLocalPos = new Vector3(0f, -0.001050f, -0.00981f);
        while (elapsed < _insertDuration)
        {
            elapsed += Time.deltaTime;
            float t = _insertCurve.Evaluate(elapsed / _insertDuration);
            transform.localPosition = Vector3.Lerp(Vector3.zero, endLocalPos, t);
            yield return null;
        }
    }
}
