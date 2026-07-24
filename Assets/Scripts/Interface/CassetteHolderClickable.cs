using System.Collections;
using UnityEngine;

public class CassetteHolderClickable : MonoBehaviour, IClickable
{
    [SerializeField] private float animationTime = 0.3f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private Vector3 openLocalEuler = new Vector3(40f, 0f, 0f);
    [SerializeField] private AudioClip openSound;

    private Vector3 _closedLocalEuler;
    private bool _isOpen;
    private bool _isAnimating;
    private Coroutine _rotateRoutine;

    private void Start()
    {
        _closedLocalEuler = transform.localEulerAngles;
    }

    public void OnClick()
    {
        if (_isAnimating) return;

        _isOpen = !_isOpen;
        Vector3 target = _isOpen ? openLocalEuler : _closedLocalEuler;

        if (_rotateRoutine != null) StopCoroutine(_rotateRoutine);
        _rotateRoutine = StartCoroutine(RotateTo(target));

        AudioController.instance.PlaySound(openSound);
    }

    private IEnumerator RotateTo(Vector3 target)
    {
        _isAnimating = true;
        Vector3 start = transform.localEulerAngles;
        float t = 0f;

        while (t < animationTime)
        {
            t += Time.deltaTime;
            float pct = rotationCurve.Evaluate(t / animationTime);
            transform.localEulerAngles = Vector3.Lerp(start, target, pct);
            yield return null;
        }

        transform.localEulerAngles = target;
        _isAnimating = false;
    }

    public bool IsOpen => _isOpen;
}
