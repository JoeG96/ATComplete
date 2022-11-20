using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private PlayerControls controls;
    private float mousesensitivity = 100f;
    private float xrotation = 0f;
    private Vector2 mouselook;
    private Transform playerbody;

    private void Awake()
    {
        playerbody = transform.parent;
        controls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look();
    }

    private void Look()
    {
        mouselook = controls.Player.Look.ReadValue<Vector2>();
        float mousex = mouselook.x * mousesensitivity * Time.deltaTime;
        float mousey = mouselook.y * mousesensitivity * Time.deltaTime;
        xrotation -= mousey;
        xrotation = Mathf.Clamp(xrotation, -90f, 90);

        transform.localRotation = Quaternion.Euler(xrotation, 0, 0);
        playerbody.Rotate(Vector3.up * mousex);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}
