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
    public GameObject deadEnd;
    public GameObject gps;
    public GPSController gpc;
    public Road r;
    public Road rn;

    private void Awake()
    {
        deadEnd = GameObject.Find("DeadEnd");
        gps = GameObject.Find("GPS");
        gpc = (GPSController)gps.GetComponent(typeof(GPSController));
        r = (Road)road.GetComponent(typeof(Road));
    }

    private void Start()
    {
        enabledRoad = gpc.enabledRoad;
        enabled = r.enabled;
        if(this.tag=="Left")
        {
            nextRoad = r.roadLeft;
        }
        else if(this.tag == "Right")
        {
            nextRoad = r.roadRight;
        }
        else if (this.tag == "Forward")
        {
            nextRoad = r.roadForward;
        }
        rn = (Road)nextRoad.GetComponent(typeof(Road));
        nextEnabled = rn.enabled;
    }

    private void OnMouseDown()
    {
        if (this.gameObject.tag == "Left" && gpc.enabledRoad == road && nextRoad!=null)
        {
            Debug.Log("left");

            enabledRoad = nextRoad;
            gpc.enabledRoad = enabledRoad;
            gpc.enabled.SetActive(false);
            if(nextRoad!=deadEnd)
            {
                enabled = nextEnabled;
                gpc.enabled = enabled;
                gpc.enabled.SetActive(true);
            }
            gpc.turns[gpc.i] = 0;
            gpc.i++;
        }
        else if (this.gameObject.tag == "Right" && gpc.enabledRoad == road && nextRoad != null)
        {
            Debug.Log("right");
            enabledRoad = nextRoad;
            gpc.enabledRoad = enabledRoad;
            gpc.enabled.SetActive(false);
            if (nextRoad != deadEnd)
            {
                enabled = nextEnabled;
                gpc.enabled = enabled;
                gpc.enabled.SetActive(true);
            }
            gpc.turns[gpc.i] = 2;
            gpc.i++;
        }
        else if (this.gameObject.tag == "Forward" && gpc.enabledRoad == road && nextRoad != null)
        {
            Debug.Log("forward");
            enabledRoad = nextRoad;
            gpc.enabledRoad = enabledRoad;
            gpc.enabled.SetActive(false);
            if (nextRoad != deadEnd)
            {
                enabled = nextEnabled;
                gpc.enabled = enabled;
                gpc.enabled.SetActive(true);
            }
            gpc.turns[gpc.i] = 1;
            gpc.i++;
        }
    }
}
