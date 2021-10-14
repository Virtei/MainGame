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
        if (Input.GetMouseButtonDown(1)) {
            cameraAnchor.transform.position += transform.right * 1;
        } else if (Input.GetMouseButtonUp(1)) {
            cameraAnchor.transform.position -= transform.right * 1;
        } /*else if (!Input.GetMouseButton(1)) {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            transform.eulerAngles += Vector3.up * x * lookSensitivity;
            //transform.eulerAngles.Set(transform.eulerAngles.x, x * lookSensitivity, transform.eulerAngles.z);
            //Quaternion newRotation = Quaternion.Euler(Vector3.up * x * lookSensitivity);
            //Quaternion newRotation = Quaternion.Euler(transform.eulerAngles.x, x * lookSensitivity, transform.eulerAngles.z);
            //transform.rotation = newRotation;


            if (invertXRotation)
                currentXRotation += y * lookSensitivity;
            else
                currentXRotation -= y * lookSensitivity;

            currentXRotation = Mathf.Clamp(currentXRotation, minXLook, maxXLook);

            Vector3 clampedAngle = cameraAnchor.eulerAngles;
            clampedAngle.x = currentXRotation;

            cameraAnchor.eulerAngles = clampedAngle;
        }*/

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
