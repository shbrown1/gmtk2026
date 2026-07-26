using System.Collections;
using System.Linq;
using UnityEngine;

public class Switch : MonoBehaviour, IClickable
{
    [SerializeField] private float animationTime = 0.3f;
    [SerializeField] private float springBackDelay = 0.15f;
    [SerializeField] private float springBackTime = 0.25f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve springBackCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private Vector3 switchedOnLocalEuler = new Vector3(0f, -120f, 0f);
    [SerializeField] private AudioClip flipSound;

    private Quaternion _offLocalRot;
    private Quaternion _onLocalRot;
    private bool isOn;
    private bool _isAnimating;
    private Coroutine _rotateRoutine;

    private bool canTurnOn
    {
        get
        {
            var batteryFlap = FindAnyObjectByType<BatteryFlapClickable>();
            var circuitBoard = FindAnyObjectByType<CircuitBoard>();
            var screws = FindObjectsByType<Screw>();

            return circuitBoard != null && circuitBoard.IsInserted &&
                   batteryFlap != null && batteryFlap.battery1PlacementTransform == null && batteryFlap.battery2PlacementTransform == null &&
                   screws.All(s => !s.IsRemoved);
        }
    }

    private void Awake()
    {
        _offLocalRot = transform.localRotation;
        _onLocalRot = _offLocalRot * Quaternion.Euler(switchedOnLocalEuler);
    }

    public void OnClick()
    {
        if (_isAnimating) return;

        if (_rotateRoutine != null) StopCoroutine(_rotateRoutine);

        if (!isOn)
        {
            if (canTurnOn)
            {
                isOn = true;
                _rotateRoutine = StartCoroutine(RotateTo(_onLocalRot, rotationCurve, animationTime));
                GetComponentInParent<Gzimo>()?.StartElapsedTimer();
                var gizmo = GetComponentInParent<Gzimo>();
                foreach (var col in gizmo.GetComponentsInChildren<Collider>())
                    Destroy(col);
                FindAnyObjectByType<ConveyorBelt>()?.GizmoCompleted(gizmo);
            }
            else
            {
                _rotateRoutine = StartCoroutine(SpringBack());
            }
        }
        else
        {
            isOn = false;
            _rotateRoutine = StartCoroutine(RotateTo(_offLocalRot, rotationCurve, animationTime));
        }

        if (flipSound != null) AudioController.instance.PlaySound(flipSound, .5f);
    }

    private IEnumerator SpringBack()
    {
        _isAnimating = true;

        // Snap to on position
        yield return RotateToImmediate(_onLocalRot, rotationCurve, animationTime);

        yield return new WaitForSeconds(springBackDelay);

        // Spring back to off
        yield return RotateToImmediate(_offLocalRot, springBackCurve, springBackTime);

        _isAnimating = false;
    }

    private IEnumerator RotateTo(Quaternion target, AnimationCurve curve, float duration)
    {
        _isAnimating = true;
        yield return RotateToImmediate(target, curve, duration);
        _isAnimating = false;
    }

    private IEnumerator RotateToImmediate(Quaternion target, AnimationCurve curve, float duration)
    {
        Quaternion start = transform.localRotation;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float pct = curve.Evaluate(t / duration);
            transform.localRotation = Quaternion.Slerp(start, target, pct);
            yield return null;
        }

        transform.localRotation = target;
    }

    public bool IsOn => isOn;
}
