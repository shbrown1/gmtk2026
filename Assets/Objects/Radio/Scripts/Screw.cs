using System.Collections;
using System.Linq;
using UnityEngine;

public class Screw : MonoBehaviour, IClickable
{
    [SerializeField] private Vector3 removedLocalOffset = new Vector3(0f, 0f, 0.5f);
    [SerializeField] private float removedScale = 1.4f;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private AudioClip onActivateClip;

    [SerializeField] private float spinRevolutions = 2f;

    private bool _isRemoved = false;
    private bool isAnimating = false;
    private Vector3 restingLocalPos;
    private Quaternion restingLocalRot;

    private Coroutine moveRoutine;

    private void Start()
    {
        restingLocalPos = transform.localPosition;
        restingLocalRot = transform.localRotation;
    }

    public void OnClick()
    {
        if (isAnimating) return;
        if (GetComponentInParent<CircuitBoardFlapClickable>()?.IsOpen == true) return;
        if (GetComponentInParent<BatteryFlapClickable>()?.IsOpen == true) return;

        Vector3 target = _isRemoved ? restingLocalPos : removedLocalOffset;
        // Scale down when popping out, scale up when returning  
        transform.localScale *= _isRemoved ? (1.0f / removedScale) : removedScale;
        _isRemoved = !_isRemoved;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        float spinDir = _isRemoved ? -1f : 1f;
        moveRoutine = StartCoroutine(MoveTo(target, spinDir));

        if (onActivateClip != null)
            AudioController.instance.PlaySound(onActivateClip, .5f);
    }

    private IEnumerator MoveTo(Vector3 targetLocalPos, float spinDir)
    {
        isAnimating = true;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        float totalSpin = spinDir * spinRevolutions * 360f;
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float pct = Mathf.SmoothStep(0f, 1f, t / moveDuration);
            transform.localPosition = Vector3.Lerp(startPos, targetLocalPos, pct);
            transform.localRotation = startRot * Quaternion.Euler(0f, 0f, totalSpin * pct);
            yield return null;
        }

        transform.localPosition = targetLocalPos;
        transform.localRotation = restingLocalRot;
        isAnimating = false;
    }

    public bool IsRemoved => _isRemoved;
}
