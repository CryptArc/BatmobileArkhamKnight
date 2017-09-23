using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatmobileInput : MonoBehaviour {


    public float accelerationInput;
    public float turnInput;
    public bool handbrake;
    public bool lightFire;
    public bool heavyFire;
    public bool boost;

    public bool combatMode;
    public Vector3 combatInput;

    public float lookInput;
    public float sensitivity;

    public Animator turret;

    void Start()
    {
        handbrake = false;
        combatMode = false;
    }

	void Update () {
        
        
        lookInput = Input.GetAxis("Horizontal Look") * sensitivity;

        boost = Input.GetButton("Boost");

        if (Input.GetAxis("Brake")!=0)
        {
            handbrake = true;
        }
        else
        {
            handbrake = false;
        }

        if (Input.GetAxisRaw("Combat")!=0)
        {
            combatMode = true;
            lightFire = Input.GetButton("Light Fire");
            heavyFire = Input.GetAxisRaw("Heavy Fire") != 0 ? true : false;
            combatInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
        else
        {
            accelerationInput = Input.GetAxis("Accel");
            turnInput = Input.GetAxis("Horizontal");
            combatMode = false;
            combatInput = Vector3.zero;
        }

        if (lightFire)
        {
            turret.SetBool("Fire", true);
        }
        else
        {
            turret.SetBool("Fire", false);
        }
    }

}
