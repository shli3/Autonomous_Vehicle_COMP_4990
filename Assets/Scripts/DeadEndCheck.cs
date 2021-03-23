using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEndCheck : MonoBehaviour
{
    // Public Variables used throughout the code
    public CarController CC;
    public GameObject go;

    void Start()
    {
        //find car
        go = GameObject.FindGameObjectWithTag("Car"); // Finds the GameObject of the Car
        CC = (CarController)go.GetComponent(typeof(CarController)); // Gets the Car's CarController script
    }
    private void Update()
    {
        if (CC.deadEndNum == 0)
        {
            CC.forward = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            CC.deadEndNum++;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            CC.deadEndNum--;
            if(CC.deadEndNum==0)
            {
                CC.forward = false;
            }
        }
    }
}
