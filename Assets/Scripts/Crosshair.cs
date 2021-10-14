using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
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

    public void LookHeight(float value) {
        lookHeight += value;
        if (lookHeight > maxAngle || lookHeight < minAngle) {
            lookHeight -= value;
        }
    }

    void OnGUI() {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        screenPosition.y = Screen.height - screenPosition.y;
        GUI.DrawTexture(new Rect(screenPosition.x - size / 2, screenPosition.y - lookHeight - size / 2, size, size), image);
    }
}
