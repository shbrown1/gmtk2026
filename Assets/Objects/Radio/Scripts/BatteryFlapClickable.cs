using System.Collections;
using System.Linq;
using UnityEngine;

public class BatteryFlapClickable : MonoBehaviour, IClickable
{
    [SerializeField] private float animationTime = 0.3f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private Vector3 openLocalEuler = new Vector3(160f, 0f, 0f);
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    public Transform battery1PlacementTransform;
    public Transform battery2PlacementTransform;

    private Quaternion _closedLocalRot;
    private Quaternion _openLocalRot;
    private bool _isOpen;
    private bool _isAnimating;
    private Coroutine _rotateRoutine;

    public bool IsOpen => _isOpen;

    private void Start()
    {
        _closedLocalRot = transform.localRotation;
        _openLocalRot = _closedLocalRot * Quaternion.Euler(openLocalEuler);
    }

    public void OnClick()
    {
        if (_isAnimating) return;

        Screw[] removableChildren = GetComponentsInChildren<Screw>()
            .Where(comp => comp.gameObject != this.gameObject).ToArray();

        if (removableChildren.Length > 0)
        {
            foreach (Screw child in removableChildren)
                if (!child.IsRemoved) return;
        }

        _isOpen = !_isOpen;
        Quaternion target = _isOpen ? _openLocalRot : _closedLocalRot;

        if (_rotateRoutine != null) StopCoroutine(_rotateRoutine);
        _rotateRoutine = StartCoroutine(RotateTo(target));

        AudioClip clip = _isOpen ? openSound : closeSound;
        if (clip != null) AudioController.instance.PlaySound(clip, .5f);
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
}