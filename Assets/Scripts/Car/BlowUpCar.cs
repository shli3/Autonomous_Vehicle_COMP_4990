using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUpCar : MonoBehaviour
{
    bool move = false;
    private void Update()
    {
        if (move)
        {
            if(this.gameObject.tag=="Car0")
            {
                transform.position += new Vector3(0, 0, 0.02f);
            }
            else if(this.gameObject.tag == "Car1")
            {
                transform.position += new Vector3(0.02f, 0, 0);
            }
            else if (this.gameObject.tag == "Car2")
            {
                transform.position += new Vector3(0, 0, -0.02f);
            }
            else if (this.gameObject.tag == "Car3")
            {
                transform.position += new Vector3(-0.02f, 0, 0);
            }
        }
    }
    private void OnMouseDown()
    {
        move = true;
    }
}