using System.Collections;
using UnityEngine;

public class Switch : MonoBehaviour, IClickable
{
    [SerializeField] private float animationTime = 0.3f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private Vector3 switchedOnLocalEuler = new Vector3(0f, -120f, 0f);
    [SerializeField] private AudioClip flipSound;

    private Quaternion _closedLocalRot;
    private Quaternion _openLocalRot;
    private bool isOn;
    private bool _isAnimating;
    private Coroutine _rotateRoutine;

    private void Start()
    {
        _closedLocalRot = transform.localRotation;
        _openLocalRot = _closedLocalRot * Quaternion.Euler(switchedOnLocalEuler);
    }

    public void OnClick()
    {
        if (_isAnimating) return;

        isOn = !isOn;
        Quaternion target = isOn ? _openLocalRot : _closedLocalRot;

        if (_rotateRoutine != null) StopCoroutine(_rotateRoutine);
        _rotateRoutine = StartCoroutine(RotateTo(target));

        AudioController.instance.PlaySound(flipSound, .5f);
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

    public bool IsOn => isOn;
}
