using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCube : MonoBehaviour
{
    private float rotationStep = 90f;
    [SerializeField] private float rotationDuration = 0.25f;

    private bool isRotating;
    private Quaternion targetRotation;
    void Update()
    {
        if (!isRotating) HandleRotateCube();
    }

    private void HandleRotateCube()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard is null) return;

        if (keyboard.leftArrowKey.wasPressedThisFrame || keyboard.aKey.wasPressedThisFrame)
        {
            StartRotation(Vector3.up, -rotationStep);
        } 
        else if (keyboard.rightArrowKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame)
        {
            StartRotation(Vector3.up, rotationStep);
        } 
        else if (keyboard.upArrowKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame)
        {
            StartRotation(Vector3.right, rotationStep);
        } 
        else if (keyboard.downArrowKey.wasPressedThisFrame || keyboard.sKey.wasPressedThisFrame)
        {
            StartRotation(Vector3.right, -rotationStep);
        }
    }

    private void StartRotation(Vector3 axis, float angle)
    {
        Quaternion delta = Quaternion.AngleAxis(angle, axis);
        targetRotation = delta * transform.rotation;
        StartCoroutine(RotateToTarget());
    }

    private IEnumerator RotateToTarget()
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }
}
