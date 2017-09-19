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

    public float maxTorque;
    public float maxTurnAngle;

    public float steerFactor;

    public float breakingTorque;
    
    private float appliedTorque;
    private float appliedTurnAngle;

    public float combatMoveSpeed;

    void Start () {
        PlayerInput = GetComponent<BatmobileInput>();
        batmobileRB = GetComponent<Rigidbody>();
	}


    void Drive()
    {
        if (!PlayerInput.combatMode)
        {
            //Adjust steer angle accordingly with speed
            maxTurnAngle = Mathf.Pow(steerFactor, batmobileRB.velocity.magnitude / 8) * 35;


            appliedTorque = maxTorque * PlayerInput.accelerationInput;
            appliedTurnAngle = maxTurnAngle * PlayerInput.turnInput;

            Debug.Log(batmobileRB.velocity + " " + batmobileRB.velocity.magnitude);

            if (appliedTorque != 0)
            {
                FL.motorTorque = appliedTorque;
                FR.motorTorque = appliedTorque;
            }

            if (appliedTurnAngle != 0)
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
    }

    void CombatMovement()
    {
        
        if (PlayerInput.combatMode)
        {
            if (batmobileRB.velocity.magnitude > 0)
            {
                //RL.brakeTorque = breakingTorque;
                //RR.brakeTorque = breakingTorque;
                //FL.brakeTorque = breakingTorque;
                //FR.brakeTorque = breakingTorque;

                batmobileRB.velocity = Vector3.zero;
            }
            else
            {
                RL.brakeTorque = 0;
                RR.brakeTorque = 0;
                FL.brakeTorque = 0;
                FR.brakeTorque = 0;
            }
            batmobileRB.MovePosition(batmobileRB.transform.position + PlayerInput.combatInput * combatMoveSpeed * Time.fixedDeltaTime);
        }
    }

    void FixedUpdate () {

        Drive();
        CombatMovement();

    }
}
