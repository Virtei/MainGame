using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
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
    private bool canDoubleJump = false;
    private bool canAirDash = false;
    private bool isDashing = false;

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

        //Vector3 direction = new Vector3(x, 0, z);
        Vector3 direction = transform.right * x + transform.forward * z;

        if (controller.isGrounded) {
            canDoubleJump = true;
            canAirDash = true;
            if (Input.GetButtonDown("Jump")) {
                directionY = jumpSpeed;
            }
        } else {
            if (Input.GetButtonDown("Jump") && canDoubleJump) {
                directionY = jumpSpeed * doubleJumpMultiplier;
                canDoubleJump = false;
            } else if (Input.GetKeyDown(KeyCode.LeftShift) && canAirDash) {
                isDashing = true;
            } else {
                directionY -= gravity * Time.deltaTime;
            }
        }

        if (isDashing) {
            controller.Move(direction * dashSpeed * Time.deltaTime);
            dashTime += Time.deltaTime;
            if (dashTime >= maxDashTime) {
                dashTime = 0f;
                canAirDash = false;
                isDashing = false;
            }
        } else {
            direction.y = directionY;

            controller.Move(direction * moveSpeed * Time.deltaTime);
        }
    }
}
