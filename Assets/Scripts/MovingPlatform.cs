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
    [SerializeField]
    private bool isAutomatic = true;
    [SerializeField]
    private bool isFalling = false;
    private bool isActivated;
    public Vector3[] points;
    private int destinationPoint = 0;
    private GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        tolerance = speed * Time.deltaTime;
        map = GameObject.Find("Map");
        isActivated = isAutomatic;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentDestination = points[destinationPoint];
        if (transform.position != currentDestination) {
            Vector3 direction = currentDestination - transform.position;
            transform.position += (direction / direction.magnitude) * speed * Time.deltaTime;
            if (direction.magnitude < tolerance) {
                transform.position = currentDestination;
                delayStart = Time.time;
            }
        } else if (isActivated) {
            if (Time.time - delayStart > delay) {
                destinationPoint = (destinationPoint + 1) % points.Length;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.Equals(PlayerManager.instance.player.GetComponent<Collider>())) {
            if (!isFalling) {
                PlayerManager.instance.player.transform.parent = transform;
            } else {
                PlayerManager.instance.player.GetComponent<Player2>().SlowFall();
                //isActivated = false;
            }
            if (!isAutomatic) {
                isActivated = true;
            }
        }
        if (other.CompareTag($"MovableObject")) {
            if (other.attachedRigidbody == null) {
                return;
            }
            other.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.Equals(PlayerManager.instance.player.GetComponent<Collider>())) {
            if (!isFalling) {
                PlayerManager.instance.player.transform.parent = null;
            }
            if (!isAutomatic) {
                isActivated = false;
            }
        }
        if (other.CompareTag("MovableObject")) {
            if (other.attachedRigidbody == null) {
                return;
            }
            other.gameObject.transform.parent = map.transform;
        }
    }

    void OnDrawGizmos() {
        if (points.Length == 0) {
            return;
        }

        Gizmos.color = Color.red;
        int previous = points.Length - 1;
        foreach (Vector3 position in points) {
            Gizmos.DrawWireCube(position, new Vector3(1, 1, 1));
            Gizmos.DrawLine(points[previous], position);

            previous = (previous + 1) % points.Length;
        }
    }

    public void Activate() {
        isActivated = true;
    }

    public void ActivateOnce() {
        isActivated = true;
        StartCoroutine(WaitForDeactivate());
    }

    public void Deactivate() {
        isActivated = false;
    }

    IEnumerator WaitForDeactivate() {
        if (delay < 0.1f) {
            yield return new WaitForSeconds(0.1f);
        } else {
            yield return new WaitForSeconds(delay);
        }
        isActivated = false;
    }
}
