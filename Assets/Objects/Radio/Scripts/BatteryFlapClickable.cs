using System.Collections;
using System.Linq;
using UnityEngine;

public class BatteryFlapClickable : MonoBehaviour, IClickable
{
    [SerializeField] private float animationTime = 0.3f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private Vector3 openLocalEuler = new Vector3(90f, 0f, 0f);

    private Quaternion _closedLocalRot;
    private Quaternion _openLocalRot;
    private bool _isOpen;
    private bool _isAnimating;
    private Coroutine _rotateRoutine;

    private void Start()
    {
        _closedLocalRot = transform.localRotation;
        _openLocalRot = _closedLocalRot * Quaternion.Euler(openLocalEuler);
    }

    public void OnClick()
    {
        if (_isAnimating) return;

        RemovableObject[] removableChildren = GetComponentsInChildren<RemovableObject>()
            .Where(comp => comp.gameObject != this.gameObject).ToArray();

        if (removableChildren.Length > 0)
        {
            foreach (RemovableObject child in removableChildren)
                if (!child.IsRemoved) return;
        }

        _isOpen = !_isOpen;
        Quaternion target = _isOpen ? _openLocalRot : _closedLocalRot;

        if (_rotateRoutine != null) StopCoroutine(_rotateRoutine);
        _rotateRoutine = StartCoroutine(RotateTo(target));
    }

    private IEnumerator RotateTo(Quaternion target)
    {
        _isAnimating = true;
        Quaternion start = transform.localRotation;
        float t = 0f;

        while (t < animationTime)
        {
            t += Time.deltaTime;
            float pct = rotationCurve.Evaluate(t / animationTime);
            transform.localRotation = Quaternion.Slerp(start, target, pct);
            yield return null;
        }

        transform.localRotation = target;
        _isAnimating = false;
    }

    public bool IsOpen => _isOpen;
}