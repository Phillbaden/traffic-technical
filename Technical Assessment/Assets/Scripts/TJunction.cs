using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TJunction : MonoBehaviour
{
    public List<VehicleController> cars = new List<VehicleController>();

    private void Update()
    {
        if (cars.Count > 1)
        {
            cars[0].gameObject.GetComponent<VehicleController>().inJunction = false;
            for (int i = 1; i < cars.Count; i++)
            {
                cars[i].gameObject.GetComponent<VehicleController>().inJunction = true;
            }
        }
        else if (cars.Count == 1)
        {
            cars[0].gameObject.GetComponent<VehicleController>().inJunction = false;
        }     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle"))
        {
            VehicleController car = other.gameObject.GetComponent<VehicleController>();
            cars.Add(car);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle"))
        {
            VehicleController car = other.gameObject.GetComponent<VehicleController>();
            car.inJunction = false;
            cars.Remove(car);
        }
    }
}
