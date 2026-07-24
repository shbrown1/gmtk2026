using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class RotatableObject : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 0.3f;

    private Camera _camera;
    private bool _isDragging;
    private Quaternion _homeRotation;

    private void Start()
    {
        _camera = Camera.main;
        _homeRotation = transform.rotation;
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
            transform.rotation = _homeRotation;

        if (Input.GetMouseButtonDown(1))
        {
            TryBeginDrag(mouse);
        }
        else if (_isDragging && mouse.rightButton.isPressed)
            ApplyRotation(mouse.delta.ReadValue());
        else if (_isDragging)
            _isDragging = false;
    }

    private void TryBeginDrag(Mouse mouse)
    {
        Ray ray = _camera.ScreenPointToRay(mouse.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            _isDragging = true;
    }

    private void ApplyRotation(Vector2 delta)
    {
        transform.Rotate(_camera.transform.up, -delta.x * _sensitivity, Space.World);
        transform.Rotate(_camera.transform.right, delta.y * _sensitivity, Space.World);
    }
}
