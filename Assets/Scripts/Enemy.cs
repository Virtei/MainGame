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
    private float timeSinceDamage = 0f;
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
        if (hasContacted) {
            timeSinceDamage += Time.deltaTime;
            if (timeSinceDamage >= invulnerabilityPeriod) {
                timeSinceDamage = 0f;
                hasContacted = false;
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.DrawWireSphere(transform.position, contactRadius);
    }

    void OnCollisionEnter(Collision collision) {
        //FindObjectOfType<AudioManager>().Play("Death1");
    }
}
