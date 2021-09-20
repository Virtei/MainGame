using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGrapple : MonoBehaviour
{
    public Grapple grapple;
    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        if (!grapple.IsGrappling()) {
            desiredRotation = transform.parent.rotation;
        } else {
            desiredRotation = Quaternion.LookRotation(grapple.GetGrapplePoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
