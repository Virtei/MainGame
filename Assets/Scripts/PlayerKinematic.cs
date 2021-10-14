using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKinematic : MonoBehaviour
{
    public int health = 3;
    [SerializeField]
    private float moveSpeed = 8f;
    [SerializeField]
    private float wallClimbSpeed = 8f;
    [SerializeField]
    private float jumpForce = 8f;
    [SerializeField]
    private float dashForce = 32f;
    [SerializeField]
    private float maxJumpTime = 0.5f;
    [SerializeField]
    private float maxDashTime = 0.25f;
    [SerializeField]
    private float maxSlideTime = 0.25f;
    [SerializeField]
    private float doubleJumpMultiplier = 1f;
    [SerializeField]
    private float slideForce = 32f;
    private Rigidbody rig;
    private AudioManager audioManager;
    private float directionY;
    private float jumpTime = 0f;
    private float dashTime = 0f;
    private float slideTime = 0f;
    private bool canDoubleJump = false;
    private bool canAirDash = false;
    private bool canSlide = true;
    private bool canWallClimb = false;
    private bool isJumping = false;
    private bool isDashing = false;
    private bool isSliding = false;
    private bool isWallClimbing = false;
    [SerializeField]
    private float maxWallDistance = 1f;
    [SerializeField]
    private float wallDeattachDistance = 1f;
    private float distanceTravelled = 0f;
    [SerializeField]
    private float pushSpeed = 5f;
    [SerializeField]
    private float grappleSpeed = 4f;
    Vector3[] directions;
    RaycastHit[] hits;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.isKinematic = true;
        audioManager = FindObjectOfType<AudioManager>();
        directions = new Vector3[] { 
            //Vector3.right, 
            //Vector3.right + Vector3.forward,
            Vector3.forward, 
            //Vector3.left + Vector3.forward, 
            //Vector3.left
        };
    }

    void FixedUpdate()
    {
        Move2();
        //Move();
    }

    /*void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Vector3 direction = new Vector3(x, 0, z);
        Vector3 direction = transform.right * x + transform.forward * z;

        /*if (Input.GetButtonDown("Jump"))
            Jump();

        canWallClimb = false;
        hits = new RaycastHit[directions.Length];
        for (int i = 0; i < directions.Length; i++) {
            Vector3 dir = transform.TransformDirection(directions[i]);
            Physics.Raycast(transform.position, dir, out hits[i], maxWallDistance);
            if (hits[i].collider != null && hits[i].collider.CompareTag($"ClimbableObject")) {
                Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green);
                canWallClimb = true;
            } else {
                Debug.DrawRay(transform.position, dir * maxWallDistance, Color.red);
            }
        }

        if (IsGrounded()) {
            canDoubleJump = true;
            canAirDash = true;
            if (Input.GetButtonDown("Jump")) {
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canSlide = true;
                isJumping = true;
                audioManager.Play("Jump1");
            } else if (Input.GetKeyDown(KeyCode.LeftShift) && canSlide) {
                isSliding = true;
                //rig.AddForce(direction * slideForce, ForceMode.Impulse);
                audioManager.Play("Slide1");
            } else {
                isJumping = false;
            }
        } else {
            if (canWallClimb) {
                isWallClimbing = true;
            } else {
                //isWallClimbing = false;
            }
            if (Input.GetButtonDown("Jump") && canDoubleJump) {
                rig.AddForce(Vector3.up * jumpForce * doubleJumpMultiplier, ForceMode.Impulse);
                canDoubleJump = false;
                audioManager.Play("DoubleJump1");
            } else if (Input.GetKeyDown(KeyCode.LeftShift) && canAirDash) {
                isDashing = true;
                audioManager.Play("AirDash1");
            } else if (Input.GetKeyDown(KeyCode.LeftControl)) {
                //directionY -= gravity;
                audioManager.Play("GroundPound1");
            } else {
                //directionY -= gravity * Time.deltaTime;
            }
        }

        if (isDashing) {
            //controller.Move(direction * dashSpeed * Time.deltaTime);
            rig.AddForce(direction * dashForce * Time.deltaTime, ForceMode.Impulse);
            dashTime += Time.deltaTime;
            if (dashTime >= maxDashTime) {
                dashTime = 0f;
                canAirDash = false;
                isDashing = false;
            }
        } else if (isSliding) {
            //controller.Move(direction * slideSpeed * Time.deltaTime);
            rig.AddForce(direction * slideForce * Time.deltaTime, ForceMode.Impulse);
            slideTime += Time.deltaTime;
            if (slideTime >= maxSlideTime) {
                slideTime = 0f;
                canSlide = false;
                isSliding = false;
            }
        } else if (isWallClimbing) {
            //Vector3 lastPosition = transform.position;
            //controller.Move(Vector3.up * wallClimbSpeed * Time.deltaTime);
            if (!canWallClimb) {
                rig.AddForce(Vector3.up, ForceMode.Impulse);
                isWallClimbing = false;
                /*distanceTravelled += Vector3.Distance(lastPosition, transform.position);
                Debug.Log(distanceTravelled.ToString());
                if (distanceTravelled >= wallDeattachDistance) {
                    distanceTravelled = 0f;
                    isWallClimbing = false;
                }
            } else {
                rig.AddForce(Vector3.up * wallClimbSpeed * Time.deltaTime, ForceMode.Impulse);
            }
        } else {
            Move();
        }
    }*/

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * x + transform.forward * z;
        direction *= moveSpeed;
        direction.y = rig.velocity.y;

        rig.velocity = direction;
    }

    void Move2()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * x + transform.forward * z;
        rig.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        float distanceToGround = GetComponent<Collider>().bounds.extents.y - 1f;
        //Debug.Log(distanceToGround.ToString());
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, 0.1f))
        if (Physics.Raycast(ray, out hit, distanceToGround + 0.1f))
        {
            Debug.DrawRay(transform.position, Vector3.down * (distanceToGround + 0.1f), Color.green);
            return hit.collider != null;
        } else {
            Debug.DrawRay(transform.position, Vector3.down * (distanceToGround + 0.1f), Color.red);
        }

        return false;
    }
}
