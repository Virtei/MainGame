using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupUI : MonoBehaviour
{
    private int pickups;
    Text pickupsText;

    // Start is called before the first frame update
    void Start()
    {
        pickups = PlayerManager.instance.player.GetComponent<Player2>().pickups;
        pickupsText = GetComponent<Text>();
        pickupsText.text = "Pickups: " + pickups.ToString();
    }

    public void UpdatePickups()
    {
        pickups = PlayerManager.instance.player.GetComponent<Player2>().pickups;
        pickupsText.text = "Pickups: " + pickups.ToString();
    }
}
