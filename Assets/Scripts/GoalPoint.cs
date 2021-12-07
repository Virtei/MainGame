using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.Equals(PlayerManager.instance.player.GetComponent<Collider>())) {
            PlayerManager.instance.player.GetComponent<Player2>().Spawn();
            Debug.Log("GG");
        }
    }
}
