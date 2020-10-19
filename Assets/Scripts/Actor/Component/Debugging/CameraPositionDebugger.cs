using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionDebugger : MonoBehaviour
{
    private GameControls _controls;

    private Transform _transform;

    private Camera _mainCamera;

    public float zPos;

    // Start is called before the first frame update
    void Awake()
    {
        _controls = new GameControls();
        _transform = transform;

        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = _controls.Default.MousePosition.ReadValue<Vector2>();

        mousePosition.z = -_mainCamera.transform.localPosition.z;

        mousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);

        mousePosition.z = _transform.position.z;

        _transform.position = mousePosition;
    }
}
