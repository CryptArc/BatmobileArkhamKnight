using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatmobileMotor : MonoBehaviour {

    private BatmobileInput PlayerInput;

    private Rigidbody batmobileRB;

    public WheelCollider FL;
    public WheelCollider FR;
    public WheelCollider RL;
    public WheelCollider RR;

    public ParticleSystem BoostIgnition;
    public ParticleSystem Afterburner;

    public GameObject Weapon;

    public float maxTorque;
    public float maxTurnAngle;

    public float steerFactor;

    public float breakingTorque;
    
    private float appliedTorque;
    private float appliedTurnAngle;

    public float combatMoveSpeed;

    public float combatBoost;
    public float driveBoost;

    private float driveBoostTime;
    public float maxDriveBoostTime;

    public bool isBoostable;
    private bool afterburnerShot;


    void Start () {
        PlayerInput = GetComponent<BatmobileInput>();
        batmobileRB = GetComponent<Rigidbody>();
        driveBoostTime = maxDriveBoostTime;
    }


    void Drive()
    {
        //Adjust steer angle accordingly with speed
        maxTurnAngle = 25;//Mathf.Pow(steerFactor, batmobileRB.velocity.magnitude / 8) * 35 < 25 ? 25: Mathf.Pow(steerFactor, batmobileRB.velocity.magnitude / 8) * 35;


        /*
          1. Apply a boost force when player presses boost button, there is enough fuel, and there is some velocity of the batmobile
          2. Consume fuel(driveBoostTime)
         */
        if (PlayerInput.boost && isBoostable && (int)batmobileRB.velocity.magnitude > 0)
        {           
            batmobileRB.AddForce(batmobileRB.transform.forward * driveBoost, ForceMode.VelocityChange);
            driveBoostTime -= Time.fixedDeltaTime * 2; 
        }


        if (batmobileRB.velocity.magnitude < 150)
            appliedTorque = maxTorque * PlayerInput.accelerationInput;
        else
            appliedTorque = 0;


        appliedTurnAngle = maxTurnAngle * PlayerInput.turnInput;


        if (appliedTorque == 0)
        {
            RL.brakeTorque = breakingTorque;
            RR.brakeTorque = breakingTorque;
            FL.brakeTorque = breakingTorque;
            FR.brakeTorque = breakingTorque;
        }
        else
        {
            RL.brakeTorque = 0;
            RR.brakeTorque = 0;
            FL.brakeTorque = 0;
            FR.brakeTorque = 0;
        }


        if (appliedTorque != 0)
        {
            FL.motorTorque = appliedTorque;
            FR.motorTorque = appliedTorque;
            RL.motorTorque = appliedTorque;
            RR.motorTorque = appliedTorque;
        }

        //Handle idle rotation of batmobile
        if (appliedTorque == 0 && appliedTurnAngle != 0 && (int)batmobileRB.velocity.magnitude == 0)
        {
            batmobileRB.rotation *= Quaternion.AngleAxis(PlayerInput.turnInput, Vector3.up);
        }
        else if (appliedTurnAngle != 0)
        {
            FL.steerAngle = appliedTurnAngle;
            FR.steerAngle = appliedTurnAngle;
        }
        

        if (PlayerInput.handbrake)
        {
            RL.brakeTorque = breakingTorque;
            RR.brakeTorque = breakingTorque;
        }
        else
        {
            RL.brakeTorque = 0;
            RR.brakeTorque = 0;
        }
    }

    void CombatMovement()
    {
        //Halt batmobile smoothly from drive mode to combat mode
        if (batmobileRB.velocity.magnitude > 0)
        {
            batmobileRB.velocity = Vector3.LerpUnclamped(batmobileRB.velocity, Vector3.zero, Time.fixedDeltaTime * 5);
            FL.brakeTorque = breakingTorque;
            FR.brakeTorque = breakingTorque;
        }

        //Handle dodging movement
        if (PlayerInput.boost && combatBoost > 0)
        {
            //Get absolute value of dodge axes, use max(x,z) as the direction of combat dodge
            var dirToDodge = Mathf.Abs(PlayerInput.combatInput.x) > Mathf.Abs(PlayerInput.combatInput.z) ? 0 : 1;

            //In each of the cases, normalizing the dodge direction to have a uniform effect
            //Dodge in X-axis
            if (dirToDodge == 0)
            {
                batmobileRB.MovePosition(batmobileRB.transform.position + transform.TransformDirection(new Vector3(PlayerInput.combatInput.x/Mathf.Abs(PlayerInput.combatInput.x),0, 0)) * combatMoveSpeed * combatBoost * Time.fixedDeltaTime);
            }
            //Dodge in Z axis
            else if(dirToDodge == 1)
            {
                batmobileRB.MovePosition(batmobileRB.transform.position + transform.TransformDirection(new Vector3(0, 0, PlayerInput.combatInput.z/ Mathf.Abs(PlayerInput.combatInput.z))) * combatMoveSpeed * combatBoost * Time.fixedDeltaTime);
            }
            combatBoost -= Time.fixedDeltaTime * 15;
        }
        //Handle combat movement
        else
            batmobileRB.MovePosition(batmobileRB.transform.position + transform.TransformDirection(PlayerInput.combatInput) * combatMoveSpeed * Time.fixedDeltaTime);


        //Rotate batmobile to face the camera's forward vector only when there is combat input else batmobile faces the previous camera's forward vector
        if (PlayerInput.combatInput.magnitude != 0)
            batmobileRB.rotation = Quaternion.Lerp(batmobileRB.rotation, Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up), Time.fixedDeltaTime * 5);


        //Set weapon rotation to face camera's forward vector
        Weapon.transform.rotation = Camera.main.transform.rotation;
    }

    void FixedUpdate () {

        if(PlayerInput.combatMode)
            CombatMovement();
        else
            Drive();

    }

    void Update()
    {
        if (Input.GetButtonUp("Boost"))
        {
            combatBoost = 4f;
        }

        /*When player is not using the boost button, refill the boost fuel, shut off afterburner and ignition particle effects
          Or else check if there is enough fuel to boost(isBoostable) and the batmobile has some velocity and activate afterburner and ignition particles
         */
        if (PlayerInput.boost == false)
        {
            BoostIgnition.Stop();
            Afterburner.Stop();
            driveBoostTime += Time.deltaTime * 0.7f;
        }
        else if(PlayerInput.boost && isBoostable && (int)batmobileRB.velocity.magnitude > 0)
        {

            if (Afterburner.isStopped && afterburnerShot)
            {
                Afterburner.Play();
                afterburnerShot = false;
            }

            if(BoostIgnition.isStopped)
                BoostIgnition.Play();
        }

        //The isBoostable indicates when the boost fuel is empty and the ignition particles is shut down
        if(driveBoostTime < 0)
        {
            isBoostable = false;
            BoostIgnition.Stop();
        }
        //A minimum of 2 units of fuel is required to have a boost effect
        else if(driveBoostTime > 2.0f)
        {
            isBoostable = true;
        }

        //Afterburner shot is only avaliable on full fuel at maxDriveBoostTime
        if(driveBoostTime > maxDriveBoostTime)
        {
            afterburnerShot = true;
        }

        //driveBoostTime can take values between 0 and maxDriveBoostTime
        driveBoostTime = Mathf.Clamp(driveBoostTime,0,maxDriveBoostTime);

    }

    void OnDrawGizmos()
    {
        if (PlayerInput != null)
        {
            if (PlayerInput.combatMode)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(gameObject.transform.position + new Vector3(0, .5f, 0), 10);
        }
    }
}
