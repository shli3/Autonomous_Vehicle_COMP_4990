using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopLight : MonoBehaviour
{
    public Material[] mat = new Material[2]; // Array to hold different materials
    public int current = 0; // Counter int to keep track of which color is currently on
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        Invoke("ChangeColor", 2); // 2 second delay between each change of color
    }

    void ChangeColor()
    {
        // Simple if else to change the color every 2 seconds recursively
        if (current == 0)
        {
            rend.sharedMaterial = mat[1];
            current = 1;
        }
        else if (current == 1)
        {
            rend.sharedMaterial = mat[0];
            current = 0;
        }

        Invoke("ChangeColor", 2);
    }
}
