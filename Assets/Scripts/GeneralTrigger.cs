using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTrigger : MonoBehaviour
{
    enum triggerType {
        MoveObject,
        MoveObjectOnce
    }

    [SerializeField]
    private triggerType type = triggerType.MoveObject;
    [SerializeField]
    private Transform[] targets;
    
    private void OnTriggerEnter(Collider other) {
        if (other.Equals(PlayerManager.instance.player.GetComponent<Collider>())) {
            //PlayerManager.instance.player.transform.parent = transform;
            if (type == triggerType.MoveObject) {
                foreach (Transform target in targets) {
                    target.gameObject.GetComponent<MovingPlatform>().Activate();
                }
            }
            if (type == triggerType.MoveObjectOnce) {
                foreach (Transform target in targets) {
                    target.gameObject.GetComponent<MovingPlatform>().ActivateOnce();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.Equals(PlayerManager.instance.player.GetComponent<Collider>())) {
            //PlayerManager.instance.player.transform.parent = transform;
            if (type == triggerType.MoveObject) {
                foreach (Transform target in targets) {
                    //target.gameObject.GetComponent<MovingPlatform>().Deactivate();
                }
            }
        }
    }
}
