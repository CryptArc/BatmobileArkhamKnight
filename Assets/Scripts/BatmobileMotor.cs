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
    
    public float appliedTorque;
    public float appliedTurnAngle;

    public float combatMoveSpeed;
    
    void Start () {
        PlayerInput = GetComponent<BatmobileInput>();
        batmobileRB = GetComponent<Rigidbody>();

	}


    void Drive()
    {
        //Adjust steer angle accordingly with speed
        maxTurnAngle = Mathf.Pow(steerFactor, batmobileRB.velocity.magnitude / 8) * 35;

        appliedTorque = maxTorque * PlayerInput.accelerationInput;
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

    void CombatMovement()
    {
        if (batmobileRB.velocity.magnitude > 0)
        {
            batmobileRB.velocity = Vector3.zero;
        }

        batmobileRB.MovePosition(batmobileRB.transform.position + transform.TransformDirection(PlayerInput.combatInput) * combatMoveSpeed * Time.fixedDeltaTime);
        batmobileRB.MoveRotation(Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up));
    }

    void FixedUpdate () {

        if(PlayerInput.combatMode)
            CombatMovement();
        else
            Drive();
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
