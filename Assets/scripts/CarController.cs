using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    Rigidbody carRigidbody;

    public float acc = 40f;
    public float maxSpeed = 20f;
    public float driftFactor = 0.95f;

    private float x = 0f;
    private float z = 0f;
    private float velocityVsForward;
    private float rotationAngle;
    private float turnFactor = 3.5f;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        manageInput();

        manageAcceleration();

        KillOrthogonalVelocity();

        manageHorizontalControls();
    }

    void manageInput()
    {
        x = Input.GetAxis("Vertical");
        z = Input.GetAxis("Horizontal");
    }

    void manageAcceleration()
    {
        if (x == 0)
        {
            carRigidbody.drag = Mathf.Lerp(carRigidbody.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            carRigidbody.drag = 0;
        }

        //Caculate how much "forward" we are going in terms of the direction of our velocity
        velocityVsForward = Vector3.Dot(transform.forward, carRigidbody.velocity);

        //Limit so we cannot go faster than the max speed in the "forward" direction
        if (velocityVsForward > maxSpeed && acc > 0)
            return;

        //Limit so we cannot go faster than the 50% of max speed in the "reverse" direction
        if (velocityVsForward < -maxSpeed * 0.5f && acc < 0)
            return;

        //Limit so we cannot go faster in any direction while accelerating
        if (carRigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed && acc > 0)
            return;

        //Create a force for the engine
        Vector3 engineForceVector = transform.forward * x * acc;

        //Apply force and pushes the car forward
        carRigidbody.AddForce(engineForceVector, ForceMode.Force);
    }

    void manageHorizontalControls()
    {
        //Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (carRigidbody.velocity.magnitude / 2);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //Update the rotation angle based on input
        rotationAngle += z * turnFactor * minSpeedBeforeAllowTurningFactor;

        //Apply steering by rotating the car object
        carRigidbody.MoveRotation(Quaternion.Euler(new Vector3(0f, rotationAngle, 0f)));
    }

    void KillOrthogonalVelocity()
    {
        //Get forward and right velocity of the car
        Vector3 forwardVelocity = transform.forward * Vector3.Dot(carRigidbody.velocity, transform.forward);
        Vector3 rightVelocity = transform.right * Vector3.Dot(carRigidbody.velocity, transform.right);

        //Kill the orthogonal velocity (side velocity) based on how much the car should drift. 
        carRigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }
}
