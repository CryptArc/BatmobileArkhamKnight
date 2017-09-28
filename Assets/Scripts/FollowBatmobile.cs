using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBatmobile : MonoBehaviour {
    //Script references
    public GameObject Batmobile;
    public BatmobileInput PlayerInput;

    //Camera settings
    public float yawAngle;
    public float pitchAngle;
    public float dampening;
    public float cameraDepthOffset;
    public float cameraHeightOffset;
    public Vector3 cameraPosition;


    void FixedUpdate()
    {
        if (PlayerInput.combatMode)
        {
            //Convert mouse x input to rotation along camera's y axis and multiply it with current camera's rotation
            Camera.main.transform.rotation *= Quaternion.AngleAxis(PlayerInput.lookInput, Vector3.up);

            //Calculate camera position based on Camera's rotation and offset it from Batmobile's position
            cameraPosition = new Vector3(Batmobile.transform.position.x + Mathf.Sin(Camera.main.transform.localEulerAngles.y * Mathf.PI / 180) * cameraDepthOffset, cameraHeightOffset, Batmobile.transform.position.z + Mathf.Cos(Camera.main.transform.localEulerAngles.y * Mathf.PI / 180) * cameraDepthOffset); 

        }
        else
        {
            //Set camera to face the forward vector of Batmobile's forward vector
            Camera.main.transform.forward = Batmobile.transform.forward;

            //Calculate camera position depending on the rotation of Batmobile
            cameraPosition = new Vector3(Batmobile.transform.position.x + Mathf.Sin(Batmobile.transform.localEulerAngles.y * Mathf.PI / 180) * cameraDepthOffset, cameraHeightOffset, Batmobile.transform.position.z + Mathf.Cos(Batmobile.transform.localEulerAngles.y * Mathf.PI / 180) * cameraDepthOffset);
                        
        }
        //Set camera's postion in a smooth interpolation
        Camera.main.transform.position = Vector3.Lerp(gameObject.transform.position, cameraPosition, dampening * Time.fixedDeltaTime);

        if (Batmobile.GetComponent<BatmobileMotor>().isBoostable && PlayerInput.boost && !PlayerInput.combatMode)
        {
            CameraShake();
        }
       

    }

    void CameraShake()
    {
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + Random.Range(-0.075f, 0.075f),
                                                      Camera.main.transform.position.y,
                                                      Camera.main.transform.position.z + Random.Range(-0.05f, 0.05f));
    }

    void Update()
    {
        if (PlayerInput.combatMode)
            dampening = 7.0f;
        else
            dampening = 15.0f;
    }
}
