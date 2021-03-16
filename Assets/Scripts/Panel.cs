using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public GameObject road;
    public GameObject nextRoad;
    public GameObject nextEnabled;
    public GameObject enabledRoad;
    public GameObject enabled;
    public GameObject gps;
    public GPSController gpc;
    public Road r;

    private void Awake()
    {
        gps = GameObject.Find("GPS");
        gpc = (GPSController)gps.GetComponent(typeof(GPSController));
        r = (Road)road.GetComponent(typeof(Road));
    }

    private void Start()
    {
        enabledRoad = gpc.enabledRoad;
        enabled = r.enabled;
    }

    private void OnMouseDown()
    {
        if (this.gameObject.tag == "Left" && gpc.enabledRoad == road)
        {
            Debug.Log("left");

            enabledRoad = nextRoad;
            gpc.enabledRoad = enabledRoad;
            gpc.enabled.SetActive(false);
            enabled = nextEnabled;
            gpc.enabled = enabled;
            gpc.enabled.SetActive(true);
            gpc.turns[gpc.i] = 0;
            gpc.i++;
        }
        else if (this.gameObject.tag == "Right" && gpc.enabledRoad == road)
        {
            Debug.Log("right");
            enabledRoad = nextRoad;
            gpc.enabledRoad = enabledRoad;
            gpc.enabled.SetActive(false);
            enabled = nextEnabled;
            gpc.enabled = enabled;
            gpc.enabled.SetActive(true);
            gpc.turns[gpc.i] = 2;
            gpc.i++;
        }
        else if (this.gameObject.tag == "Forward" && gpc.enabledRoad == road)
        {
            Debug.Log("forward");
            enabledRoad = nextRoad;
            gpc.enabledRoad = enabledRoad;
            gpc.enabled.SetActive(false);
            enabled = nextEnabled;
            gpc.enabled = enabled;
            gpc.enabled.SetActive(true);
            gpc.turns[gpc.i] = 1;
            gpc.i++;
        }
    }
}
