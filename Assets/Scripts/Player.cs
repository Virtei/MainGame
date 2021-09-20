using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Rigidbody rig;

    void Update ()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    void Move ()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * x + transform.forward * z;
        direction *= moveSpeed;
        direction.y = rig.velocity.y;

        rig.velocity = direction;
    }

    void Jump ()
    {
        if (CanJump())
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool CanJump ()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            return hit.collider != null;
        }

        return false;
    }
}
