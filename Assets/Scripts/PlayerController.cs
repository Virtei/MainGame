using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State {
        Grounded,
        Jumping,
        Falling,
        Dashing
    }

    State currentState;
    float dashTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState) {
            case State.Grounded:
            if (Input.GetButtonDown("Jump")) {

            } else {
                
            }
            break;
            case State.Jumping:
            break;
            case State.Falling:
            break;
            case State.Dashing:
            break;

        }
    }

    void ChangeState(State newState) {
        currentState = newState;

        switch(newState) {
            case State.Jumping:
            break;
            case State.Dashing:
                dashTime = 0;
            break;
        }
    }
}
