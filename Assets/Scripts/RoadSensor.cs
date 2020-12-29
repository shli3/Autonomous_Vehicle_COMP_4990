using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSensor : MonoBehaviour
{
    public CarController CC;
    public GameObject go;
    public bool see = false;
    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.FindGameObjectWithTag("Car"); // Finds the GameObject of the Car
        CC = (CarController)go.GetComponent(typeof(CarController)); // Gets the Car's CarController script
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==9)
        {
            see = true;
            CC.roadPost = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            see = true;
            CC.roadPost = other.gameObject;
        }
    }
}
