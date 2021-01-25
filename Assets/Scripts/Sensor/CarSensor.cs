using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSensor : MonoBehaviour
{
    // Public Variables used throughout the code
    public CarController CC;
    public GameObject go;
    public Renderer rend;

    void Start()
    {
        //find car
        go = GameObject.FindGameObjectWithTag("Car"); // Finds the GameObject of the Car
        CC = (CarController)go.GetComponent(typeof(CarController)); // Gets the Car's CarController script
    }
    //function that moves car
    void turnDecision()
    {
        CC.intersection = false;
        if (CC.decision[CC.index] == 0)
        {
            CC.left = true;
        }
        else if (CC.decision[CC.index] == 1)
        {
            CC.forward = true;
        }
        else if (CC.decision[CC.index] == 2)
        {
            CC.right = true;
        }
        CC.index++;
        CC.fourWayStopCheck = true;
    }

    //when car sees another car
    void OnTriggerEnter(Collider other)
    {
        if (CC.fourWayStopCheck == true && other.gameObject.layer == 8)
        {
            if (this.gameObject.tag == "CarSensorLeft" && other.gameObject.layer == 8)
            {
                CC.carInIntersectionLeft = true;
            }
            if (this.gameObject.tag == "CarSensorForward" && other.gameObject.layer == 8)
            {
                CC.carInIntersectionForward = true;
            }
            if (this.gameObject.tag == "CarSensorRight" && other.gameObject.layer == 8)
            {
                CC.carInIntersectionRight = true;
            }
            if (this.gameObject.tag == "CarSensorIntersection" && other.gameObject.layer == 8)
            {
                CC.carInIntersection = true;
            }
        }
        if (this.gameObject.tag == "CarSensorIntersection" && other.gameObject.layer == 8)
        {
            CC.carInIntersection = true;
        }
    }

    //when car hits intersection stop all car sensors from checking more cars. the cars currently seen are those to wait fo at the four way stop
    private void OnTriggerStay(Collider other)
    {
        if (CC.fourWayStopCheck == true && other.gameObject.layer == 8 && CC.intersection == true)
        {
            CC.fourWayStopCheck = false;
        }
        if (this.gameObject.tag == "CarSensorIntersection" && other.gameObject.layer == 8 && CC.intersection==true)
        {
            CC.carInIntersection = true;
        }
    }

    //once car is at intersection when all determined cars have left it is time to move.
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            if (this.gameObject.tag == "CarSensorLeft" && other.gameObject.layer == 8)
            {
                CC.carInIntersectionLeft = false;
            }
            else if (this.gameObject.tag == "CarSensorForward" && other.gameObject.layer == 8)
            {
                CC.carInIntersectionForward = false;
            }
            else if (this.gameObject.tag == "CarSensorRight" && other.gameObject.layer == 8)
            {
                CC.carInIntersectionRight = false;
            }
            else if (this.gameObject.tag == "CarSensorIntersection" && other.gameObject.layer == 8)
            {
                CC.carInIntersection = false;
            }

            if (CC.carInIntersection == false && CC.carInIntersectionLeft == false && CC.carInIntersectionForward == false && CC.carInIntersectionRight == false && CC.intersection==true)
            {
                Invoke("turnDecision", 1);
            }
        }
       
    }
}
