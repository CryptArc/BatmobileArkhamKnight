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

    public ParticleSystem weaponFlares;
    public AudioSource lightFireSound;
    public AudioSource heavyFireSound;

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
        else if(Input.GetAxisRaw("Combat") == 0)
        {
            
            accelerationInput = Input.GetAxis("Accel");
            turnInput = Input.GetAxis("Horizontal");
            combatMode = false;
            combatInput = Vector3.zero;
            turret.SetBool("Fire", false);
            weaponFlares.Stop();
            lightFire = false;
            lightFireSound.Stop();
            //heavyFireSound.Stop();
        }

        if (lightFire)
        {
            turret.SetBool("Fire", true);
            if (!weaponFlares.isPlaying)
            {
                weaponFlares.Play();
            }
            if (!lightFireSound.isPlaying)
            {
                lightFireSound.Play();
            }
        }
        else
        {
            turret.SetBool("Fire", false);
            lightFireSound.Stop();
        }
        if (heavyFire)
        {
            if (!heavyFireSound.isPlaying)
            {
                heavyFireSound.Play();
            }
        }
        else
        {
            //heavyFireSound.Stop();
        }
    }

}
