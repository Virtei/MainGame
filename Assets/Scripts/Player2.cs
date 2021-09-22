using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float gravity = 9.81f;
    [SerializeField]
    private float jumpSpeed = 3.5f;
    [SerializeField]
    private float doubleJumpMultiplier = 0.5f;
    private CharacterController controller;
    private float directionY;
    private bool canDoubleJump = false;

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
            if (Input.GetButtonDown("Jump")) {
                directionY = jumpSpeed;
            }
        } else {
            if (Input.GetButtonDown("Jump") && canDoubleJump) {
                directionY = jumpSpeed * doubleJumpMultiplier;
                canDoubleJump = false;
            }
        }

        directionY -= gravity * Time.deltaTime;

        direction.y = directionY;

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
}