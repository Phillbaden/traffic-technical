using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TJunction : MonoBehaviour
{
    public List<VehicleController> cars = new List<VehicleController>();
    public float oldBrakeRange = 1.25f;

    private void Update()
    {
        if (cars.Count > 1) // Checks if there is more than one car at the junction, and stops them if they are not first
        {
            cars[0].gameObject.GetComponent<VehicleController>().inJunction = false;
            for (int i = 1; i < cars.Count; i++)
            {
                cars[i].gameObject.GetComponent<VehicleController>().inJunction = true;
            }
        }
        else if (cars.Count == 1) // If there is only one car, it is allowed to drive uninterrupted. Ideally, one would add a coroutine to allow the car to stop for a while regardless, for safety.
        {
            cars[0].gameObject.GetComponent<VehicleController>().inJunction = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle")) // Once the car enters the Junction, they are added to a list
        {
            VehicleController car = other.gameObject.GetComponent<VehicleController>();
            cars.Add(car);
            StartCoroutine(BrakeBypass(cars[0])); // A small optimization, if they get to close to another car, they might detect the car and be permanently stuck. This is overcome by disabling braking for a few seconds if they are designated to drive during the junction.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Vehicle")) // The cars are removed from the list once they leave the junction
        {
            VehicleController car = other.gameObject.GetComponent<VehicleController>();
            car.inJunction = false;
            cars.Remove(car);
        }
    }

    IEnumerator BrakeBypass(VehicleController car)
    {
        car.brakingRange = 0.0f;
        yield return new WaitForSeconds(2.0f);
        car.brakingRange = oldBrakeRange;
    }
}
