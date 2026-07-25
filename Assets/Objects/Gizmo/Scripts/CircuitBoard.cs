using System.Collections;
using UnityEngine;

public class CircuitBoard : MonoBehaviour, IDraggable
{
    [SerializeField] float _alignRadius = 2f;
    [SerializeField] float _fullAlignDistance = 1.5f;
    [SerializeField] float _insertDuration = 0.4f;
    [SerializeField] AnimationCurve _insertCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] AudioClip _insertSound;

    CircuitBoardFlapClickable _holder;
    Quaternion _homeRotation;
    bool _inserted;

    void Awake()
    {
        _holder = FindFirstObjectByType<CircuitBoardFlapClickable>();
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
        if (_holder == null || !_holder.IsOpen) return;

        float dist = Vector3.Distance(transform.position, _holder.circuitBoardPlacementTransform.position);
        float t = Mathf.Clamp01(Mathf.InverseLerp(_alignRadius, _fullAlignDistance, dist));

        transform.position = Vector3.Lerp(transform.position, _holder.circuitBoardPlacementTransform.position, t);

        Quaternion target = _holder.circuitBoardPlacementTransform.rotation;
        if (Quaternion.Dot(_homeRotation, target) < 0f)
            target = new Quaternion(-target.x, -target.y, -target.z, -target.w);
        transform.rotation = Quaternion.Slerp(_homeRotation, target, t);
    }

    public void EndDrag()
    {
        if (_inserted || _holder == null || _holder.circuitBoardPlacementTransform == null) return;

        float dist = Vector3.Distance(transform.position, _holder.circuitBoardPlacementTransform.position);
        if (dist <= _fullAlignDistance)
            StartCoroutine(InsertBoard());
    }

    IEnumerator InsertBoard()
    {
        _inserted = true;

        if (_insertSound != null) AudioController.instance.PlaySound(_insertSound, .5f);

        Draggable draggable = GetComponent<Draggable>();
        if (draggable != null) Destroy(draggable);

        transform.SetParent(_holder.circuitBoardPlacementTransform);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        float elapsed = 0f;
        Vector3 endLocalPos = new Vector3(0f, -.5f, 0f);
        while (elapsed < _insertDuration)
        {
            elapsed += Time.deltaTime;
            float t = _insertCurve.Evaluate(elapsed / _insertDuration);
            transform.localPosition = Vector3.Lerp(Vector3.zero, endLocalPos, t);
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1,1, 1.09f), t);
            yield return null;
        }

        _holder.OnCircuitBoardInserted();
    }
}
