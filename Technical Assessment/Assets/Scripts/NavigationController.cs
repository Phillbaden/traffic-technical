using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
    public float movementSpeed = 1;
    public float rotationSpeed = 120;
    public float stopDistance = 2.5f;
    public Vector3 destination;
    public bool reachedDestination;
    public bool carInFront = false;

    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = Random.Range(0.2f, 0.5f); // Assigns random speeds to each pedestrian on spawn, to better simulate actual traffic
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != destination) // Rotates the pedestrians and moves them to the next waypoint in the chain
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopDistance)
            {
                if (!carInFront)
                {
                    reachedDestination = false;
                    Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.zero);
                }
            }
            else
            {
                reachedDestination = true;
            }
        }

        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.white);
        // Small optimization in traffic avoidance. The peds will stop if they see a car in front of them for a few seconds then try to move out of the way using a coroutine. Requires refinement
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 1f) && hit.transform.CompareTag("Vehicle"))
        {
            StartCoroutine(WalkTimer());
        }
        else
        {
            carInFront = false;
        }
    }

    public void SetDestination(Vector3 destination) // Method to set the destination of the next waypoint
    {
        this.destination = destination;
        reachedDestination = false;
    }

    IEnumerator WalkTimer()
    {
        carInFront = true;
        yield return new WaitForSeconds(2f);
        carInFront = false;
        yield return new WaitForSeconds(1f);
    }
}
