using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSafeSensor : MonoBehaviour
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
        if (/*CC.decision[CC.index] == 0 &&*/ other.gameObject.layer == 8 && this.gameObject.tag == "SafeForward")
        {
            CC.safeLeft++;
        }
        if (other.gameObject.layer == 8 && this.gameObject.tag == "SafeIntersection")
        {
            CC.safeIntersection++;
        }

    }

    //when car hits intersection stop all car sensors from checking more cars. the cars currently seen are those to wait fo at the four way stop
    private void OnTriggerStay(Collider other)
    {
      
    }

    //once car is at intersection when all determined cars have left it is time to move.
    private void OnTriggerExit(Collider other)
    {
        if (/*CC.decision[CC.index] == 0 &&*/ other.gameObject.layer == 8 && this.gameObject.tag == "SafeForward")
        {
            Debug.Log("uuuuuuuuuuuuuh");
            CC.safeLeft--;
            if(CC.safeLeft == 0)
            {
                CC.carInIntersectionForward = false;
            }
        }
        if (other.gameObject.layer == 8 && this.gameObject.tag == "SafeIntersection")
        {
            CC.safeIntersection--;
            if (CC.safeIntersection == 0)
            {
                CC.carInIntersection = false;
            }
        }
    }
}
