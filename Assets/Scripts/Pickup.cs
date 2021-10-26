using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    void Start() {
        PlayerManager.instance.player.GetComponent<Player2>().totalPickups += 1;
        GameObject.Find("Pickups").GetComponent<PickupUI>().UpdateTotalPickups();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.Equals(PlayerManager.instance.player.GetComponent<Collider>())) {
            PlayerManager.instance.player.GetComponent<Player2>().GetPickup();
            Destroy(gameObject);
        }
    }
}
