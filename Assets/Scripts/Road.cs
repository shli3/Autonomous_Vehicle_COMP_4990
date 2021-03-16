using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject roadLeft;
    public GameObject roadRight;
    public GameObject roadForward;
    public GameObject enabledRoad;
    public GameObject enabled;
    public GameObject gps;
    public GPSController gpc;
    int i = 0;

    private void Awake()
    {
        gps = GameObject.Find("GPS");
        gpc = (GPSController)gps.GetComponent(typeof(GPSController));
    }

    private void Start()
    {
        enabledRoad = gpc.enabledRoad;
    }

}
