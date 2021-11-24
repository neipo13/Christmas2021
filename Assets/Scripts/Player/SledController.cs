using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SledController : MonoBehaviour
{
    public Rigidbody ballRigidbody;

    public float forwardAccel = 800f;
    public float reverseAccel = 400f;
    public float maxSpeed = 10000f;
    public float turnStrength = 180f;
    public float airTurnStrength = 90f;
    public float gravityForce = 1000f;
    public float groundDrag;
    public float airDrag = 0.2f;
    public float rotateRate = 30f;
    public float maxWheelTurn = 60f;
    public float maxTip = 25f;
    public float tipRate = 15f;
    public float wheelTurnSpeed = 25f;
    public float dustVelocityMagnitude = 1000f;
    public float dustVelocityMinTurn = 90f;

    private float speedInput;
    private float turnInput;
    private bool grounded;
    private float tipAmount = 0f;

    public LayerMask groundMask;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    public Transform wholeCarModel;
    public ParticleSystem dustParticles;
    public TrailRenderer[] skidMarkers;

    void Start()
    {
        ballRigidbody.transform.parent = null; // deatch the rb
    }

    void Update()
    {
        speedInput = 0f;

        //slowly return tip toward 0
        tipAmount = Mathf.Lerp(tipAmount, 0f, tipRate * Time.deltaTime);

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput > 0f)
        {
            speedInput = verticalInput * forwardAccel;
        }
        else if (verticalInput < 0f)
        {
            speedInput = verticalInput * reverseAccel;
        }

        if (grounded)
        {
            turnInput = horizontalInput * turnStrength;
        }
        else
        {
            turnInput = horizontalInput * airTurnStrength;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * Time.deltaTime, 0f));

        if (horizontalInput > 0f)
        {
            tipAmount = Mathf.Lerp(tipAmount, maxTip, tipRate * horizontalInput * Time.deltaTime);
        }
        else if (horizontalInput < 0f)
        {
            tipAmount = Mathf.Lerp(tipAmount, -maxTip, -tipRate * horizontalInput * Time.deltaTime);
        }
        wholeCarModel.localRotation = Quaternion.Euler(tipAmount, wholeCarModel.localRotation.eulerAngles.y, wholeCarModel.localRotation.eulerAngles.z);

        var velocityMagnitude = ballRigidbody.velocity.magnitude;
        if (grounded && Mathf.Abs(horizontalInput) > 0 && velocityMagnitude > dustVelocityMagnitude)
        {
            dustParticles?.Play();
            //for (int i = 0; i < skidMarkers.Length; i++)
            //{
            //    skidMarkers[i].emitting = true;
            //}
        }
        else
        {
            //for (int i = 0; i < skidMarkers.Length; i++)
            //{
            //    skidMarkers[i].emitting = false;
            //}
        }

        transform.position = ballRigidbody.transform.position; //follow the rb
    }

    void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;


        if (Physics.Raycast(groundRayPoint.position, transform.up * -1f, out hit, groundRayLength, groundMask))
        {
            Debug.DrawLine(groundRayPoint.position, hit.point, Color.red);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, rotateRate * Time.deltaTime);
            grounded = true;
        }


        if (grounded)
        {
            ballRigidbody.drag = groundDrag;
            if (Mathf.Abs(speedInput) > 0f && ballRigidbody.velocity.sqrMagnitude < maxSpeed)
            {
                ballRigidbody.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            ballRigidbody.drag = airDrag;
            //ballRigidbody.AddForce(Vector3.up * gravityForce * -1f);
        }
    }
}
