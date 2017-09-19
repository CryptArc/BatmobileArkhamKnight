using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatmobileInput : MonoBehaviour {


    public float accelerationInput;
    public float turnInput;
    public bool handbrake;

    public bool combatMode;
    public Vector3 combatInput;
	

    void Start()
    {
        handbrake = false;
        combatMode = false;
    }

	void Update () {
        accelerationInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.Space))
        {
            handbrake = true;
        }
        else
        {
            handbrake = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            combatMode = true;
            combatInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
        else
        {
            combatMode = false;
            combatInput = Vector3.zero;
        }
    }
}
