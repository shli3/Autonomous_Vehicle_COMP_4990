using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUpCar : MonoBehaviour
{
    private void OnMouseDown()
    {
        transform.position += new Vector3(1, 0, 0);
        //Destroy(gameObject);
    }
}
