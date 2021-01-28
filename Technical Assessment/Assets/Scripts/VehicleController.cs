using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public Transform path;
    public Transform leftDetector;
    public Transform rightDetector;
    public Transform middleDetector;
    private List<Transform> nodes;
    private int currentNode = 0;
    public float maxSteerAngle = 45f;
    public float maxBrakeTorque = 150f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBR;
    public WheelCollider wheelBL;
    public float torque = 10f;
    public float maxMotorTorque = 30f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    public Vector3 centreOfMass;
    public bool isBraking = false;
    public float brakingRange = 10.0f;
    public bool inJunction;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    private void Update()
    {
        bool ray1 = Physics.Raycast(middleDetector.position, middleDetector.TransformDirection(Vector3.forward), out RaycastHit hit1, brakingRange);
        bool ray2 = Physics.Raycast(leftDetector.position, leftDetector.TransformDirection(Vector3.forward), out RaycastHit hit2, brakingRange);
        bool ray3 = Physics.Raycast(rightDetector.position, rightDetector.TransformDirection(Vector3.forward), out RaycastHit hit3, brakingRange);

        Debug.DrawRay(middleDetector.position, middleDetector.TransformDirection(Vector3.forward), Color.white);
        Debug.DrawRay(leftDetector.position, leftDetector.TransformDirection(Vector3.forward), Color.white);
        Debug.DrawRay(rightDetector.position, rightDetector.TransformDirection(Vector3.forward), Color.white);

        if ((ray1 && (hit1.transform.CompareTag("Pedestrian") || hit1.transform.CompareTag("Vehicle"))) 
            || (ray2 && (hit2.transform.CompareTag("Pedestrian") || hit2.transform.CompareTag("Vehicle"))) 
            || (ray3 && (hit3.transform.CompareTag("Pedestrian") || hit1.transform.CompareTag("Vehicle"))))
        {
            isBraking = true;
        }
        else if (inJunction)
        {
            isBraking = true;
        }
        else
        {
            isBraking = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Steering();
        Driving();
        CheckWaypointDistance();
        Braking();
    }

    private void Steering()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }
    
    private void Driving()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }

    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.05f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void Braking()
    {
        if (isBraking)
        {
            wheelBL.brakeTorque = maxBrakeTorque;
            wheelBR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheelBL.brakeTorque = 0;
            wheelBR.brakeTorque = 0;
        }
    }
}
