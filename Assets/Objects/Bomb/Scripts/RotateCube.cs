using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCube : MonoBehaviour
{
    [SerializeField] private float _rotationStep = 90f;
    [SerializeField] private float _rotationDuration = 0.25f;

    private bool _isRotating;
    private Quaternion _targetRotation;

    private void Update()
    {
        if (!_isRotating) HandleInput();
    }

    private void HandleInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.leftArrowKey.wasPressedThisFrame || keyboard.aKey.wasPressedThisFrame)
            StartRotation(Vector3.up, -_rotationStep);
        else if (keyboard.rightArrowKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame)
            StartRotation(Vector3.up, _rotationStep);
        else if (keyboard.upArrowKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame)
            StartRotation(Vector3.right, _rotationStep);
        else if (keyboard.downArrowKey.wasPressedThisFrame || keyboard.sKey.wasPressedThisFrame)
            StartRotation(Vector3.right, -_rotationStep);
    }

    private void StartRotation(Vector3 localAxis, float angle)
    {
        _targetRotation = transform.localRotation * Quaternion.AngleAxis(angle, localAxis);
        StartCoroutine(RotateToTarget());
    }

    private IEnumerator RotateToTarget()
    {
        _isRotating = true;
        Quaternion startRotation = transform.localRotation;
        float elapsed = 0f;

        while (elapsed < _rotationDuration)
        {
            elapsed += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(startRotation, _targetRotation, Mathf.Clamp01(elapsed / _rotationDuration));
            yield return null;
        }

        transform.localRotation = _targetRotation;
        _isRotating = false;
    }
}
