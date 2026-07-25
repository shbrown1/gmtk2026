using System.Collections;
using UnityEngine;

public class Battery : MonoBehaviour, IDraggable
{
    [SerializeField] float _alignRadius = 2f;
    [SerializeField] float _fullAlignDistance = 1.5f;
    [SerializeField] float _insertDuration = 0.4f;
    [SerializeField] AnimationCurve _insertCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] AudioClip _insertSound;

    BatteryFlapClickable _batteryFlap;
    Transform _targetSlot;
    Quaternion _homeRotation;
    bool _inserted;

    void Awake()
    {
        _batteryFlap = FindFirstObjectByType<BatteryFlapClickable>();
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
        if (_batteryFlap == null || !_batteryFlap.IsOpen) return;

        _targetSlot = NearestSlot();
        if (_targetSlot == null) return;

        float dist = Vector3.Distance(transform.position, _targetSlot.position);
        float t = Mathf.Clamp01(Mathf.InverseLerp(_alignRadius, _fullAlignDistance, dist));

        transform.position = Vector3.Lerp(transform.position, _targetSlot.position, t);

        Quaternion target = _targetSlot.rotation;
        if (Quaternion.Dot(_homeRotation, target) < 0f)
            target = new Quaternion(-target.x, -target.y, -target.z, -target.w);
        transform.rotation = Quaternion.Slerp(_homeRotation, target, t);
    }

    public void EndDrag()
    {
        if (_inserted) return;

        _targetSlot = NearestSlot();
        if (_targetSlot == null) return;

        float dist = Vector3.Distance(transform.position, _targetSlot.position);
        if (dist <= _fullAlignDistance)
            StartCoroutine(InsertBattery(_targetSlot));
    }

    IEnumerator InsertBattery(Transform slot)
    {
        _inserted = true;

        if (_insertSound != null)
            AudioController.instance.PlaySound(_insertSound, .5f);

        if (_batteryFlap.battery1PlacementTransform == slot)
            _batteryFlap.battery1PlacementTransform = null;
        else if (_batteryFlap.battery2PlacementTransform == slot)
            _batteryFlap.battery2PlacementTransform = null;

        Draggable draggable = GetComponent<Draggable>();
        if (draggable != null) Destroy(draggable);

        transform.SetParent(slot);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        float elapsed = 0f;
        Vector3 endLocalPos = new Vector3(-.4f, 0f, 0);
        while (elapsed < _insertDuration)
        {
            elapsed += Time.deltaTime;
            float t = _insertCurve.Evaluate(elapsed / _insertDuration);
            transform.localPosition = Vector3.Lerp(Vector3.zero, endLocalPos, t);
            yield return null;
        }
    }

    Transform NearestSlot()
    {
        Transform s1 = _batteryFlap.battery1PlacementTransform;
        Transform s2 = _batteryFlap.battery2PlacementTransform;

        if (s1 == null) return s2;
        if (s2 == null) return s1;

        float d1 = Vector3.Distance(transform.position, s1.position);
        float d2 = Vector3.Distance(transform.position, s2.position);
        return d1 <= d2 ? s1 : s2;
    }
}
