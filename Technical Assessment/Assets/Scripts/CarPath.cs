using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPath : MonoBehaviour
{
    public Color lineColor;
    public Color nodeColor;
    public float sphereRadius;
    private List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos() // Visualization of the pathway for a car to travel
    {
        Gizmos.color = lineColor;
        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++) // Adds all the nodes to the path
        {
            if (pathTransforms[i] != transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        for (int i = 0; i < nodes.Count; i++) // Assigns which node is the current or previous one
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i > 0)
            {
                previousNode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1)
            {
                previousNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawWireSphere(currentNode, sphereRadius);
            Gizmos.DrawLine(previousNode, currentNode);
        }
    }
}
