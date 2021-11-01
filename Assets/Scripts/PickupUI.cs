using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupUI : MonoBehaviour
{
    private int pickups = 0;
    private int totalPickups = 0;
    Text pickupsText;
    
    void Awake()
    {
        pickupsText = GetComponent<Text>();
        pickupsText.text = "Pickups: " + pickups.ToString() + "/" + totalPickups.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        pickups = PlayerManager.instance.player.GetComponent<Player2>().pickups;
        totalPickups = PlayerManager.instance.player.GetComponent<Player2>().totalPickups;
        //pickupsText = GetComponent<Text>();
        //pickupsText.text = "Pickups: " + pickups.ToString() + "/" + totalPickups.ToString();
    }

    public void UpdatePickups()
    {
        pickups = PlayerManager.instance.player.GetComponent<Player2>().pickups;
        pickupsText.text = "Pickups: " + pickups.ToString() + "/" + totalPickups.ToString();
    }

    public void UpdateTotalPickups() {
        totalPickups = PlayerManager.instance.player.GetComponent<Player2>().totalPickups;
        pickupsText.text = "Pickups: " + pickups.ToString() + "/" + totalPickups.ToString();
    }
}
