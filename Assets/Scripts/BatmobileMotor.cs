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

    public float combatBoost;
    public float driveBoost;
    public float driveBoostTime;

    public GameObject Weapon;

    void Start () {
        PlayerInput = GetComponent<BatmobileInput>();
        batmobileRB = GetComponent<Rigidbody>();

	}


    void Drive()
    {
        //Adjust steer angle accordingly with speed
        maxTurnAngle = 25;//Mathf.Pow(steerFactor, batmobileRB.velocity.magnitude / 8) * 35 < 25 ? 25: Mathf.Pow(steerFactor, batmobileRB.velocity.magnitude / 8) * 35;
        if (PlayerInput.boost && driveBoostTime > 0)
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
            batmobileRB.velocity = Vector3.LerpUnclamped(batmobileRB.velocity, Vector3.zero, Time.fixedDeltaTime * 5);
            FL.brakeTorque = breakingTorque;
            FR.brakeTorque = breakingTorque;
        }
        if (PlayerInput.boost && combatBoost > 0)
        {
            batmobileRB.MovePosition(batmobileRB.transform.position + transform.TransformDirection(PlayerInput.combatInput) * combatMoveSpeed * combatBoost * Time.fixedDeltaTime);
            combatBoost -= Time.fixedDeltaTime * 15;
        }
        else
            batmobileRB.MovePosition(batmobileRB.transform.position + transform.TransformDirection(PlayerInput.combatInput) * combatMoveSpeed * Time.fixedDeltaTime);

        if (PlayerInput.combatInput.magnitude != 0)
            batmobileRB.rotation = Quaternion.Lerp(batmobileRB.rotation, Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up), Time.fixedDeltaTime * 5);// (Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up));

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
        if(!PlayerInput.boost && driveBoostTime < 3)
            driveBoostTime += Time.deltaTime * 0.7f;
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
