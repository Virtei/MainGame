using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float lookRadius = 10f;
    [SerializeField]
    private float contactRadius = 1f;
    private bool hasContacted = false;
    [SerializeField]
    private float invulnerabilityPeriod = 2f;
    [SerializeField]
    private float returnToPatrolPeriod = 5f;
    private float timeSinceDamage = 0f;
    private float timeSinceAlerted = 0f;
    private bool isAlerted = false;
    Transform target;
    NavMeshAgent agent;
    public Transform[] points;
    private int destinationPoint = 0;
    private int previousPoint;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
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
            isAlerted = true;
            agent.SetDestination(target.position);
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

        agent.destination = points[destinationPoint].position;
        previousPoint = destinationPoint;
        destinationPoint = (destinationPoint + 1) % points.Length;
    }

    void ReturntoPreviousPoint() {
        if (points.Length == 0) {
            return;
        }

        agent.destination = points[previousPoint].position;
    }

    void OnDrawGizmos() {
        if (points.Length == 0) {
            return;
        }
        
        Gizmos.color = Color.red;
        int previous = points.Length - 1;
        foreach (Transform transform in points) {
            Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
            Gizmos.DrawLine(points[previous].position, transform.position);

            previous = (previous + 1) % points.Length;
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, contactRadius);
        //Gizmos.DrawRay(transform.position, agent.destination);
        Gizmos.DrawLine(transform.position, agent.destination);
    }

    void OnCollisionEnter(Collision collision) {
        //FindObjectOfType<AudioManager>().Play("Death1");
    }
}
