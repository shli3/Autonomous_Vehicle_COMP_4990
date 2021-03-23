using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public CarController CC;
    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        // find car
        go = GameObject.FindGameObjectWithTag("Car"); // Finds the GameObject of the Car
        CC = (CarController)go.GetComponent(typeof(CarController)); // Gets the Car's CarController script
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "Collision" && other.gameObject.layer == 8 && CC.forward == true && CC.left == false && CC.right == false)
        {
            CC.forward = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (this.gameObject.tag == "Collision" && other.gameObject.layer == 8 && CC.forward == false && CC.intersection == false && CC.left == false && CC.right == false)
        {
            CC.forward = true;
            CC.right = false;
            CC.left = false;
        }
    }
}
