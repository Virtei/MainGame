using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple2 : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera2, player;
    private float maxDistance = 100f;
    private Vector3 currentGrapplePosition;
    private bool isGrappling = false;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        } else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }

    void LateUpdate() {
        DrawRope();
    }

    void StartGrapple() {
        RaycastHit hit;
        //if (Physics.Raycast(camera2.position, camera2.forward, out hit, maxDistance, whatIsGrappleable)) {
        if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            isGrappling = true;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            lineRenderer.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

     void DrawRope() {
        //if (!joint) return;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    public bool IsGrappling() {
        return isGrappling;
        //return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }

    void StopGrapple() {
        isGrappling = false;
        lineRenderer.positionCount = 0;
        //Destroy(joint);
    }
}
