using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControllerWheelCollider : MonoBehaviour
{
    public GameObject carPrefab;
    public VariableJoystick variableJoystick;
    public Button accelerator;
    public Button brake;
    public float motorTorque = 300f;
    public float maxSteer = 20f;
    float maxSpeed = 20f;

    Rigidbody carRigidBody;

    private WheelCollider leftBackCollider;
    private WheelCollider rightBackCollider;
    private WheelCollider leftFrontCollider;
    private WheelCollider rightFrontCollider;

    [SerializeField]
    private bool acceleratorPressed = false;
    private bool brakePressed = false;

    GameManager gameManager;


    float x = 0f;
    float z = 0f;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        acceleratorPressed = gameManager.autoAccelerate.isOn;
    }

    private void Start()
    {
        assignColliders();
        carRigidBody = carPrefab.GetComponent<Rigidbody>();
        carRigidBody.centerOfMass = carPrefab.transform.GetChild(2).localPosition;
    }

    void assignColliders()
    {
        Transform wheelsColliderParent = carPrefab.transform.GetChild(1).GetChild(1);
        leftFrontCollider = wheelsColliderParent.GetChild(0).GetComponent<WheelCollider>();
        rightFrontCollider = wheelsColliderParent.GetChild(1).GetComponent<WheelCollider>();
        leftBackCollider = wheelsColliderParent.GetChild(2).GetComponent<WheelCollider>();
        rightBackCollider = wheelsColliderParent.GetChild(3).GetComponent<WheelCollider>();
    }

    private void FixedUpdate()
    {
        x = variableJoystick.Vertical;
        z = variableJoystick.Horizontal;

        //x = Input.GetAxis("Vertical");
        //z = Input.GetAxis("Horizontal");

        if (carRigidBody.velocity.magnitude <= maxSpeed && acceleratorPressed)
        {
            leftBackCollider.motorTorque = x * motorTorque;
            rightBackCollider.motorTorque = x * motorTorque;
        }
        else
        {
            leftBackCollider.motorTorque = 0;
            rightBackCollider.motorTorque = 0;
        }



        leftFrontCollider.steerAngle = z * maxSteer;
        rightFrontCollider.steerAngle = z * maxSteer;

        manageStopping();

        //cameraTilt();
    }

    public void setAcceleratorPressed(bool value)
    {
        acceleratorPressed = value;
        if(gameManager.autoAccelerate.isOn)
        {
            acceleratorPressed = true;
        }
    }

    public void setBrakePressed(bool value)
    {
        brakePressed = value;
    }

    void manageStopping()
    {
        
        if(!gameManager.autoAccelerate.isOn)
        {
            if (!acceleratorPressed || brakePressed)
            {
                if (brakePressed)
                {
                    carRigidBody.drag = Mathf.Lerp(carRigidBody.drag, 10.0f, Time.fixedDeltaTime * 2);
                }
                else
                {
                    carRigidBody.drag = Mathf.Lerp(carRigidBody.drag, 3.0f, Time.fixedDeltaTime * 3);
                }

            }
            else
            {
                carRigidBody.drag = 0;
            }
        }
        else
        {
            if(x == 0 || brakePressed)
            {
                if (brakePressed)
                {
                    carRigidBody.drag = Mathf.Lerp(carRigidBody.drag, 10.0f, Time.fixedDeltaTime * 2);
                }
                else
                {
                    carRigidBody.drag = Mathf.Lerp(carRigidBody.drag, 3.0f, Time.fixedDeltaTime * 3);
                }
            }
            else
            {
                carRigidBody.drag = 0;
            }
        }
        
    }

    void cameraTilt()
    {
        if (carRigidBody.velocity.magnitude >= 5)
        {
            Camera.main.GetComponent<cameraFollow>().cameraTiltOffset = -z;
        }
    }
}
