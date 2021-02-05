using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorFirstIteration : MonoBehaviour
{
    // Public Variables used throughout the code
    public CarController CC;
    public GameObject go;
    public Renderer rend;
    public Color red = new Color(1f, 0f, 0f, 1f);
    public Color green = new Color(0f, 1f, 0f, 1f);

    void Start()
    {
        //find car
        go = GameObject.FindGameObjectWithTag("Car"); // Finds the GameObject of the Car
        CC = (CarController)go.GetComponent(typeof(CarController)); // Gets the Car's CarController script
    }

    //Used after car hits intersection to decide where to tell the car to go. It will be called with an invoke to make the
    //cars stop noticable. 
    void turnDecision()
    {
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

    // When the sensor collides with stop sign
    void OnTriggerEnter(Collider other)
    {        
        // Use the renderer to analyse tha color rather than tags, allows more flexibility   
        rend = other.GetComponent<Renderer>();

        //if the sign sensor collider collides with a sign or the car sensor goes off
        if (rend.material.color == red && other.gameObject.layer == 11 && other.gameObject.tag=="StopSign" && this.gameObject.tag == "firstSensorSign" &&
           (other.transform.rotation.eulerAngles.y-1 < go.transform.rotation.eulerAngles.y && other.transform.rotation.eulerAngles.y+1 > go.transform.rotation.eulerAngles.y))
        {
            //is the car at an intersection
            CC.at4Way = true;
            CC.intersection = true;
            CC.forward = false;
            if(CC.carInIntersection == false && CC.carInIntersectionLeft == false && CC.carInIntersectionForward == false && CC.carInIntersectionRight == false)
            {
               Invoke("turnDecision", 1);
            }
        }

        //if the light sensor collider collides with a red light
        else if (rend.material.color == red && other.gameObject.layer == 10 && other.gameObject.tag == "StopLight" && this.gameObject.tag == "firstSensorLight" &&
                (other.transform.rotation.eulerAngles.y < go.transform.rotation.eulerAngles.y + 1 && other.transform.rotation.eulerAngles.y > go.transform.rotation.eulerAngles.y - 1))
        {
            CC.atLight = true;
            //is the car at an intersection
            CC.intersection = true;
            CC.forward = false;
            //Invoke("turnDecision", 1);
        }

        //if the light sensor collider collides with a green light
        else if (rend.material.color == green && other.gameObject.layer == 10 && other.gameObject.tag == "StopLight" && this.gameObject.tag == "firstSensorLight" &&
                (other.transform.rotation.eulerAngles.y < go.transform.rotation.eulerAngles.y + 1 && other.transform.rotation.eulerAngles.y > go.transform.rotation.eulerAngles.y - 1))
        {
            CC.atLight = true;
            CC.intersection = true;
            CC.forward = false;
            if (CC.decision[CC.index] == 0 && CC.safeLeft == 0)
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
        }

    }

    //light change from red to green
    private void OnTriggerStay(Collider other)
    {
        if (rend.material.color == green && other.gameObject.layer == 10 && other.gameObject.tag == "StopLight" && this.gameObject.tag == "firstSensorLight" && CC.intersection == true &&
           (other.transform.rotation.eulerAngles.y < go.transform.rotation.eulerAngles.y + 1 && other.transform.rotation.eulerAngles.y > go.transform.rotation.eulerAngles.y - 1))
        {
            if (CC.decision[CC.index] == 0 && CC.safeLeft==0)
            {
                CC.left = true;
                CC.index++;
                CC.intersection = false;
            }
            else if (CC.decision[CC.index] == 1)
            {
                CC.forward = true;
                CC.index++;
                CC.intersection = false;
            }
            else if (CC.decision[CC.index] == 2)
            {
                CC.right = true;
                CC.index++;
                CC.intersection = false;
            }
            //CC.index++;
            //CC.intersection = false;
        }
    }

    //car has exited the intersection
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer ==10 ||other.gameObject.layer == 11)
        {
            CC.intersection = false;
            CC.atLight = false;
            CC.at2Way = false;
            CC.at4Way = false;
        }
    }
}
