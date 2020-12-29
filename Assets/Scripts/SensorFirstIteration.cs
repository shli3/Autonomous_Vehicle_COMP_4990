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
    public bool intersection = false;

    void Start()
    {
        go = GameObject.FindGameObjectWithTag("Car"); // Finds the GameObject of the Car
        CC = (CarController)go.GetComponent(typeof(CarController)); // Gets the Car's CarController script
    }

    // When the sensor collides with stop sign
    void OnTriggerEnter(Collider other)
    {
        intersection = true;
        // Use the renderer to analyse tha color rather than tags, allows more flexibility   
        rend = other.GetComponent<Renderer>();
        rend.enabled = true;
        if (rend.material.color == red && other.gameObject.layer == 11 && this.gameObject.tag=="firstSensorSign")
        {
            CC.forward = false;
            if (CC.decision[CC.index] == 0)
            {
                CC.left = true;
            }
            else if (CC.decision[CC.index] == 1)
            {
                CC.forward = true;
            }
            else
            {
                CC.right = true;
            }
             CC.index++;
        }
        /* Continue moving on green
        else if (rend.material.color == green)
        {
            CC.forward = true;
        }*/
    }

    // When the sensor collides with stop light
    private void OnTriggerStay(Collider other)
    {
        intersection = true;
        // Use the renderer to analyse tha color rather than tags, allows more flexibility   
        rend = other.GetComponent<Renderer>();
        rend.enabled = true;

        // Stop on red (light or stop sign)
        if (rend.material.color == red && other.gameObject.layer==10)
        {
            CC.forward = false;
        }
        // Continue moving on green
        else if (rend.material.color == green)
        {
            CC.forward = true;
        }
    }

    private void OnTriggerExit()
    {
        intersection = false;
    }
}
