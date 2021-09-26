using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float lookRadius = 10f;
    public float contactRadius = 1f;
    private bool hasContacted = false;
    Transform target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius) {
            agent.SetDestination(target.position);
        }
        if (distance <= contactRadius && !hasContacted) {
            FindObjectOfType<AudioManager>().Play("Death1");
            hasContacted = true;
            PlayerManager.instance.player.GetComponent<Player2>().TakeDamage();
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void OnCollisionEnter(Collision collision) {
        //FindObjectOfType<AudioManager>().Play("Death1");
    }
}
