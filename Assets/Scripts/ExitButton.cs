using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public GameObject gps;
    public GPSController gpc;

    private void Start()
    {
        gps = GameObject.Find("GPS");
        gpc = (GPSController)gps.GetComponent(typeof(GPSController));
    }
    private void OnMouseDown()
    {
        if (this.gameObject.layer == 13 || this.gameObject.tag == "Exit")
        {
            Debug.Log("Saved");
            for (int i = 0; i < gpc.turns.Length; i++)
            {
                PlayerPrefs.SetInt("turn" + i, gpc.turns[i]);
            }
            SceneManager.LoadScene(1);
        }
    }
}
