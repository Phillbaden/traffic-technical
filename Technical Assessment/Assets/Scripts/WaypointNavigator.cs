﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    NavigationController controller;
    public Waypoint currentWaypoint;
    int direction;

    private void Awake()
    {
        controller = GetComponent<NavigationController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        controller.SetDestination(currentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update() // The entire method is used simply to assign the next waypoint the peds need to traverse. 
    {
        if (controller.reachedDestination)
        {
            bool shouldBranch = false; 

            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0) // Branching is decided randomly
            {
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
            }

            if (shouldBranch) // Branching will change the order of the waypoints depending on the direction they are moving
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
            }
            else
            {
                if (direction == 0)
                {
                    if (currentWaypoint.nextWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                        direction = 1;
                    }
                }
                else if (direction == 1)
                {
                    if (currentWaypoint.previousWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        direction = 0;
                    }
                }
            }
            controller.SetDestination(currentWaypoint.GetPosition());
        }
    }
}
