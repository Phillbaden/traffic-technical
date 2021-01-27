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
    Vector3 velocity;
    Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = Random.Range(0.2f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;
            velocity = velocity.normalized;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }
}
