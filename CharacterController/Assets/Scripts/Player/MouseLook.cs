using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameObject cameraController;
    [Range(50, 100)]
    public float mouseSensitivity;
    public float pitch { get { return _pitch; } }
    public float yaw { get { return _yaw; } }

    private float _pitch = 0;
    private float _yaw = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        _yaw += mouseX;

        cameraController.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        transform.Rotate(mouseX * Vector3.up);
    }
}
