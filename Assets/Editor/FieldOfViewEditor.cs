using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI() {
        Enemy enemy = (Enemy) target;
        Handles.color = Color.red;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-enemy.GetLookAngle() / 2, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * Vector3.forward;
        //Handles.DrawWireDisc(enemy.transform.position, Vector3.up, 360f, enemy.GetLookRadius);
        Handles.DrawWireArc(enemy.transform.position, Vector3.up, leftRayDirection, enemy.GetLookAngle(), enemy.GetLookRadius());
    }
}
