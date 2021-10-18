using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3 : MonoBehaviour
{
    enum State {
        Spawning,
        Grounded,
        Jumping,
        DoubleJumping,
        Falling,
        Dashing,
        Sliding,
        GroundPounding,
        Climbing,
        Grappling
    }
    private State currentState;
    [SerializeField]
    private float moveSpeed = 8f;
    [SerializeField]
    private float gravity = 9.81f;
    [SerializeField]
    private float jumpSpeed = 2.5f;
    [SerializeField]
    private float dashSpeed = 8f;
    [SerializeField]
    private float maxDashTime = 0.5f;
    [SerializeField]
    private float doubleJumpMultiplier = 1f;
    [SerializeField]
    private float slideSpeed = 4f;
    private CharacterController controller;
    private float directionY;
    private float dashTime = 0f;
    private float jumpTime = 0f;
    [SerializeField]
    private float maxJumpTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * x + transform.forward * z;

        switch(currentState) {
            case State.Grounded:
                if (Input.GetButtonDown("Jump")) {
                    ChangeState(State.Jumping);
                } else {
                    controller.Move(direction * moveSpeed * Time.deltaTime);
                }
            break;
            case State.Jumping:
                if (Input.GetButtonDown("Jump")) {
                    ChangeState(State.DoubleJumping);
                } else if (Input.GetKeyDown(KeyCode.LeftShift)) {
                    ChangeState(State.Dashing);
                } else {
                    direction.y = directionY;
                    controller.Move(direction * moveSpeed * Time.deltaTime);
                    jumpTime += Time.deltaTime;
                    if (jumpTime >= maxJumpTime) {
                        ChangeState(State.Falling);
                    }
                }
            break;
            case State.DoubleJumping:
                direction.y = directionY;
                controller.Move(direction * moveSpeed * Time.deltaTime);
                jumpTime += Time.deltaTime;
                if (jumpTime >= maxJumpTime) {
                    ChangeState(State.Falling);
                }
            break;
            case State.Falling:
                directionY -= gravity * Time.deltaTime;
                direction.y = directionY;
                controller.Move(direction * moveSpeed * Time.deltaTime);
            break;
            case State.Dashing:
                controller.Move(direction * dashSpeed * Time.deltaTime);
                dashTime += Time.deltaTime;
                if (dashTime >= maxDashTime) {
                    ChangeState(State.Falling);
                }
            break;

        }
    }

    void ChangeState(State newState) {
        currentState = newState;

        switch(newState) {
            case State.Jumping:
                directionY = jumpSpeed;
            break;
            case State.DoubleJumping:
                directionY = jumpSpeed * doubleJumpMultiplier;
            break;
            case State.Falling:
                jumpTime = 0f;
            break;
            case State.Dashing:
                dashTime = 0f;
            break;
        }
    }
}
