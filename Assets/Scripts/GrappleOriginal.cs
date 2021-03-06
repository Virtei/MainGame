using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleOriginal : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera2, player;
    [SerializeField]
    private float pullSpeed = 20f;
    private float maxDistance = 100f;
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;

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
        Crosshair crosshair = FindObjectOfType<Crosshair>();
        if (Physics.Raycast(crosshair.transform.position, Quaternion.Euler(0, 0, 0) * gunTip.forward, out hit, maxDistance, whatIsGrappleable)) {
        //if (crosshair.GetGrapplePoint() != Vector3.zero) {
            if (hit.collider.CompareTag($"MovableObject")) {
                if (hit.collider.gameObject.GetComponent<Rigidbody>() == null) {
                    return;
                }
                Vector3 pullDirection = -gunTip.forward;
                //Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                hit.collider.attachedRigidbody.velocity = pullDirection * pullSpeed;
            } else {
                
            }
            
            grapplePoint = hit.point;
            //grapplePoint = crosshair.GetGrapplePoint();
            joint = player.gameObject.AddComponent<SpringJoint>();
            //joint = crosshair.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            //float distanceFromPoint = Vector3.Distance(crosshair.gameObject.transform.position, grapplePoint);
            
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
        /*if (Physics.Raycast(gunTip.position, Quaternion.Euler(0, 0, 0) * gunTip.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }*/
    }

    void DrawRope() {
        if (!joint) return;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }

    void StopGrapple() {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }
}
