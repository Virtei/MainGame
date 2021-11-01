using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private float lookRadius = 10f;
    [SerializeField]
    [Range(0, 360)]
    private float lookAngle = 210f;
    [SerializeField]
    private float lookHeight = 5f;
    [SerializeField]
    private float contactRadius = 2f;
    private bool hasContacted = false;
    [SerializeField]
    private float invulnerabilityPeriod = 2f;
    [SerializeField]
    private float returnToPatrolPeriod = 5f;
    private float timeSinceDamage = 0f;
    private float timeSinceAlerted = 0f;
    private bool isAlerted = false;
    private Transform target;
    public Vector3[] points;
    private int destinationPoint = 0;
    private int previousPoint;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        //agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        //agent.enabled = false;
        if (points.Length != 0) {
            previousPoint = points.Length - 1;
        }
        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (!isAlerted) {
            if (!agent.pathPending && agent.remainingDistance < 0.5f) {
                GotoNextPoint();
            }
        } else {
            timeSinceAlerted += Time.deltaTime;
            if (timeSinceAlerted >= returnToPatrolPeriod) {
                Debug.Log("Return to patrol");
                isAlerted = false;
                timeSinceAlerted = 0f;
                ReturntoPreviousPoint();
            }
        }
        if (distance <= lookRadius) {
            //Quaternion look = Quaternion.LookRotation(transform.position - target.position).normalized;
            //float dotProduct = Vector2.Dot(transform.right, look.eulerAngles);
            Vector3 look = (target.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(look, transform.forward);
            float dotLimit = Mathf.Cos(lookAngle / 2 * Mathf.Deg2Rad);

            //Debug.Log("Dot: " + Mathf.Acos(dotProduct) * Mathf.Rad2Deg);
            //Debug.Log("DotLimit: " + Mathf.Acos(dotLimit) * Mathf.Rad2Deg);
            //Debug.Log("DotLimit: " + dotLimit);
            if (dotProduct > dotLimit) {
                //Debug.Log("Can see");
                isAlerted = true;
                agent.SetDestination(target.position);
            } else {
                //Debug.Log("Can't see");
            }
        }
        if (distance <= contactRadius && !hasContacted) {
            //FindObjectOfType<AudioManager>().Play("Death1");
            hasContacted = true;
            PlayerManager.instance.player.GetComponent<Player2>().TakeDamage();
        }
        if (hasContacted) {
            timeSinceDamage += Time.deltaTime;
            if (timeSinceDamage >= invulnerabilityPeriod) {
                timeSinceDamage = 0f;
                hasContacted = false;
            }
        }
        /*float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius) {
            agent.SetDestination(target.position);
        }
        if (distance <= contactRadius && !hasContacted) {
            FindObjectOfType<AudioManager>().Play("Death1");
            hasContacted = true;
            PlayerManager.instance.player.GetComponent<Player2>().TakeDamage();
        }
        if (hasContacted) {
            timeSinceDamage += Time.deltaTime;
            if (timeSinceDamage >= invulnerabilityPeriod) {
                timeSinceDamage = 0f;
                hasContacted = false;
            }
        }*/
    }

    void GotoNextPoint() {
        if (points.Length == 0) {
            return;
        }

        agent.destination = points[destinationPoint];
        previousPoint = destinationPoint;
        destinationPoint = (destinationPoint + 1) % points.Length;
    }

    void ReturntoPreviousPoint() {
        if (points.Length == 0) {
            return;
        }

        agent.destination = points[previousPoint];
    }

    public float GetLookRadius() {
        return lookRadius;
    }

    public float GetLookAngle() {
        return lookAngle;
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

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;

        float halfFov = lookAngle / 2f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFov, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFov, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Gizmos.DrawRay(transform.position, leftRayDirection * lookRadius);
        Gizmos.DrawRay(transform.position, rightRayDirection * lookRadius);
        //Gizmos.DrawLine(transform.position + leftRayDirection * lookRadius, transform.position + rightRayDirection * lookRadius);

        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, contactRadius);
        //Gizmos.DrawRay(transform.position, agent.destination);
        Gizmos.DrawLine(transform.position, agent.destination);
    }

    void OnCollisionEnter(Collision collision) {
        //FindObjectOfType<AudioManager>().Play("Death1");
    }
}
