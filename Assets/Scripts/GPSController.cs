using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSController : MonoBehaviour
{
    public GameObject enabledRoad;
    public GameObject enabled;
    public GameObject[] roads = new GameObject[24];
    public int[] turns = new int[100];
    public int i = 0;
    Vector3 erv = new Vector3(0, 1, 0);
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0;i<24;i++)
        {
            roads[i] = GameObject.Find("Street "+i);
        }
    }

    private void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x!=enabled.transform.position.x || this.transform.position.z != enabled.transform.position.z)
        {
            this.transform.position = new Vector3(enabled.transform.position.x, 190, enabled.transform.position.z);
        }
    }

}
