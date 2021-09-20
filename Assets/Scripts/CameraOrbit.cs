using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public float lookSensitivity;
    public float minXLook;
    public float maxXLook;
    public Transform cameraAnchor;

    public bool invertXRotation;

    private float currentXRotation;

    void Start () 
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate () 
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        transform.eulerAngles += Vector3.up * x * lookSensitivity;


        if (invertXRotation)
            currentXRotation += y * lookSensitivity;
        else
            currentXRotation -= y * lookSensitivity;

        currentXRotation = Mathf.Clamp(currentXRotation, minXLook, maxXLook);

        Vector3 clampedAngle = cameraAnchor.eulerAngles;
        clampedAngle.x = currentXRotation;

        cameraAnchor.eulerAngles = clampedAngle;
    }
}
