using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float delay = 0.5f;
    private float delayStart;
    private float tolerance;
    public Transform[] points;
    private int destinationPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        tolerance = speed * Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentDestination = points[destinationPoint].position;
        if (transform.position != currentDestination) {
            Vector3 direction = currentDestination - transform.position;
            transform.position += (direction / direction.magnitude) * speed * Time.deltaTime;
            if (direction.magnitude < tolerance) {
                transform.position = currentDestination;
                delayStart = Time.time;
            }
        } else {
            if (Time.time - delayStart > delay) {
                destinationPoint = (destinationPoint + 1) % points.Length;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.Equals(PlayerManager.instance.player.GetComponent<Collider>())) {
            PlayerManager.instance.player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.Equals(PlayerManager.instance.player.GetComponent<Collider>())) {
            PlayerManager.instance.player.transform.parent = null;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (Transform transform in points) {
            Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
        }
    }
}
