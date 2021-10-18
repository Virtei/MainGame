using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public LayerMask whatIsGrappleable;
    [SerializeField]
    private Transform cameraAnchor;
    [SerializeField]
    private Texture2D image;
    [SerializeField]
    private int size;
    [SerializeField]
    private float maxAngle;
    [SerializeField]
    private float minAngle;
    private float lookHeight;
    private Vector3 screenPosition;

    public void LookHeight(float value) {
        lookHeight += value;
        if (lookHeight > maxAngle || lookHeight < minAngle) {
            lookHeight -= value;
        }
    }

    public Vector3 GetGrapplePoint() {
        RaycastHit hit;
        //if (Physics.Raycast(screenPosition, transform.forward, out hit, 100f, whatIsGrappleable)) {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f, whatIsGrappleable)) {
            return hit.point;
        }
        return Vector3.zero;
    }

    public Vector3 GetCrosshairPosition() {
        return transform.position;
    }

    void OnGUI() {
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        screenPosition.y = Screen.height - screenPosition.y;
        GUI.DrawTexture(new Rect(screenPosition.x - size / 2, screenPosition.y - lookHeight - size / 2, size, size), image);
    }
}
