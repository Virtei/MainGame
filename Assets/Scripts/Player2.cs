using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private ParticleSystem jumpParticleSys;
    [SerializeField]
    private ParticleSystem airDashParticleSys;
    public int health = 3;
    public int pickups = 0;
    public int totalPickups {get; set;}
    [SerializeField]
    private float moveSpeed = 8f;
    [SerializeField]
    private float wallClimbSpeed = 8f;
    [SerializeField]
    private float gravity = 9.81f;
    private float gravity2;
    [SerializeField]
    private float fallMultiplier = 2f;
    [SerializeField]
    private float jumpSpeed = 2.5f;
    private float initialJumpVelocity;
    [SerializeField]
    private float dashSpeed = 32f;
    [SerializeField]
    private float maxJumpHeight = 1f;
    [SerializeField]
    private float maxJumpTime = 0.5f;
    [SerializeField]
    private float maxDashTime = 0.25f;
    [SerializeField]
    private float maxSlideTime = 0.25f;
    [SerializeField]
    private float doubleJumpMultiplier = 1f;
    [SerializeField]
    private float slideSpeed = 32f;
    private CharacterController controller;
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
    private bool isFalling = false;
    private bool isDashing = false;
    private bool isSliding = false;
    private bool isWallClimbing = false;
    private bool hasSpawned = true;
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

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioManager = FindObjectOfType<AudioManager>();
        anim = GetComponentInChildren<Animator>();
        
        //particleSys = GetComponentInChildren<ParticleSystem>();
        jumpParticleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        airDashParticleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        directions = new Vector3[] { 
            //Vector3.right, 
            //Vector3.right + Vector3.forward,
            Vector3.forward, 
            //Vector3.left + Vector3.forward, 
            //Vector3.left
        };
        //StartCoroutine(WaitForLoad());
        gravity2 = (-2 * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / (maxJumpTime / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned) {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Vector3 direction = new Vector3(x, 0, z);
        Vector3 direction = transform.right * x + transform.forward * z;

        canWallClimb = false;
        if (canAttach2()) {
            //Debug.Log("canAttach");
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
        }

        if (controller.isGrounded) {
            canDoubleJump = true;
            canAirDash = true;
            if (Input.GetButtonDown("Jump")) {
                directionY = jumpSpeed;
                //directionY = initialJumpVelocity;
                canSlide = true;
                isJumping = true;
                audioManager.Play("Jump1");
                jumpParticleSys.Play();
            } else if (Input.GetKeyDown(KeyCode.LeftShift) && canSlide) {
                isSliding = true;
                audioManager.Play("Slide1");
            } else {
                isJumping = false;
                jumpParticleSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        } else {
            //if (canWallClimb) {
            //if (canWallClimb && Input.GetAxis("Vertical") > 0) {
            if (canWallClimb && Input.GetKey(KeyCode.W)) {
                isWallClimbing = true;
                jumpParticleSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            } else {
                //isWallClimbing = false;
            }
            if (Input.GetButtonDown("Jump") && canDoubleJump) {
                directionY = jumpSpeed * doubleJumpMultiplier;
                //directionY = initialJumpVelocity * doubleJumpMultiplier;
                canDoubleJump = false;
                audioManager.Play("DoubleJump1");
                jumpParticleSys.Play();
            } else if (Input.GetKeyDown(KeyCode.LeftShift) && canAirDash) {
                isDashing = true;
                audioManager.Play("AirDash1");
                airDashParticleSys.Play();
            } else if (Input.GetKeyDown(KeyCode.LeftControl)) {
                directionY -= gravity;
                audioManager.Play("GroundPound1");
            } else {
                directionY -= gravity * Time.deltaTime;
            }
        }

        if (GameObject.Find("Gun").GetComponent<Grapple2>().IsGrappling()) {
            // grapple towards point
            //direction = GameObject.Find("Gun").GetComponent<Grapple2>().GetGrapplePoint() - transform.position;

            direction = GameObject.Find("Gun").GetComponent<Grapple>().rigidBody.transform.position;
            //direction = Vector3.MoveTowards(transform.position, GameObject.Find("Gun").GetComponent<Grapple2>().GetGrapplePoint(), 2f);
            controller.Move(direction * grappleSpeed * Time.deltaTime);
        } else if (isDashing) {
            controller.Move(direction * dashSpeed * Time.deltaTime);
            dashTime += Time.deltaTime;
            if (dashTime >= maxDashTime) {
                dashTime = 0f;
                canAirDash = false;
                isDashing = false;
                airDashParticleSys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        } else if (isSliding) {
            controller.Move(direction * slideSpeed * Time.deltaTime);
            slideTime += Time.deltaTime;
            if (slideTime >= maxSlideTime) {
                slideTime = 0f;
                canSlide = false;
                isSliding = false;
            }
        } else if (isWallClimbing) {
            //Vector3 lastPosition = transform.position;
            //controller.Move(Vector3.up * wallClimbSpeed * Time.deltaTime);
            if (!Input.GetKey(KeyCode.W)) {
                isWallClimbing = false;
            } else if (!canWallClimb) {
                controller.Move(Vector3.up);
                isWallClimbing = false;
                /*distanceTravelled += Vector3.Distance(lastPosition, transform.position);
                Debug.Log(distanceTravelled.ToString());
                if (distanceTravelled >= wallDeattachDistance) {
                    distanceTravelled = 0f;
                    isWallClimbing = false;
                }*/
            } else {
                controller.Move(Vector3.up * wallClimbSpeed * Time.deltaTime);
            }
        } else {
            direction.y = directionY;

            controller.Move(direction * moveSpeed * Time.deltaTime);
        }
        
        // play walk animation when there is some movement input
        if (x != 0f || z != 0f)
        {
            anim.Play("Walk");
        }
        else
        {
            anim.Play("Idle");
        }
    }
    
    public void TakeDamage() {
        if (health > 0) {
            health -= 1;
            if (health == 0) {
                hasSpawned = false;
                audioManager.Play("Destroyed1");
                Debug.Log("Respawn");
                //transform.position = spawnPoint.transform.position;
                transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
                health = 3;
                StartCoroutine(WaitForLoad());
            } else {
                audioManager.Play("Death1");
            }
            Debug.Log("Health = " + health.ToString());
            GameObject.Find("Health").GetComponent<HealthBar>().UpdateHealth();
        }
    }

    public void GetPickup() {
        pickups++;
        GameObject.Find("Pickups").GetComponent<PickupUI>().UpdatePickups();
        audioManager.Play("Pickup1");
    }

    public void Spawn() {
        hasSpawned = false;
        Debug.Log("Respawn");
        transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
        StartCoroutine(WaitForLoad());
    }

    public void SlowFall() {
        StartCoroutine(ChangeGravity(gravity / 2));
    }

    bool canAttach() {
        if (isJumping) {
            jumpTime += Time.deltaTime;
            if (jumpTime >= maxJumpTime) {
                jumpTime = 0f;
            }
            return false;
        }

        return true;
    }

    bool canAttach2() {
        return true;
    }

    IEnumerator WaitForLoad() {
        yield return new WaitForSeconds(0.25f);
        hasSpawned = true;
    }

    IEnumerator WaitForSlide() {
        yield return new WaitForSeconds(2f);
        canSlide = true;
    }

    IEnumerator ChangeGravity(float newGravity) {
        float oldGravity = gravity;
        gravity = newGravity;
        yield return new WaitForSeconds(2f);
        gravity = oldGravity;
    }

    IEnumerator EmitParticles() {
        jumpParticleSys.Play();
        yield return new WaitForSeconds(2f);
        jumpParticleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.CompareTag($"MovableObject")) {
            if (hit.collider.gameObject.GetComponent<Rigidbody>() == null) {
                return;
            }
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            hit.collider.attachedRigidbody.velocity = pushDirection * pushSpeed;
        }
    }
}
