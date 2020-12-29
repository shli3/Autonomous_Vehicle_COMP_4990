using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //car variables
    public bool forward = true;
    public bool left = false;
    public bool right = false;
    string forwardDirection = "North";
    //temporary way to chose turns at intersections to be raplaced with gps
    public int[] decision = { 2, 2, 2, 2 };
    public int index = 0;
    //speed of the turn
    float waitTime = 0.5f;
    //time for smooth turn
    int x;

    //vectors for movement speed and turns
    Vector3 start;
    Vector3 moveSpeedNorth = new Vector3(0, 0, 0.05f);
    Vector3 moveSpeedSouth = new Vector3(0, 0, -0.05f);
    Vector3 moveSpeedEast = new Vector3(0.05f, 0, 0);
    Vector3 moveSpeedWest = new Vector3(-0.05f, 0, 0);
    Vector3 current;
    Vector3 postTurn;
    Vector3 intersect;
    Vector3 turn;
    Vector3 rightMid;
    Vector3 middle;

    //road currently on and road to turn onto at intersection
    public GameObject roadCurrent;
    public GameObject roadPost;
    //road sensors
    GameObject sensorFirstSign;
    public SensorFirstIteration sensorFirstSignScript;
    GameObject sensorFirstLight;
    public SensorFirstIteration sensorFirstLightScript;
    GameObject leftRoadSensor;
    public RoadSensor leftSensorScript;
    GameObject rightRoadSensor;
    public RoadSensor rightSensorScript;
    GameObject currentRoadSensor;
    public CurrentRoad currentSensorScript;

    //move to first half of left turn
    private void leftMove()
    {
        turn = new Vector3(((intersect.x - current.x) / 4), 0, ((intersect.z - current.z) / 4));
        transform.position += turn;
        transform.Rotate(0, -11.25f, 0, Space.Self);
    }
    //move to second half of left turn
    private void leftMove2()
    {
        turn = new Vector3((postTurn.x - intersect.x) / 4, 0, (postTurn.z - intersect.z) / 4);
        transform.position += turn;
        transform.Rotate(0, -11.25f, 0, Space.Self);
    }
    //move first half of right turn
    private void rightMove()
    {
        turn = new Vector3(((rightMid.x - current.x) / 4), 0, ((rightMid.z - current.z) / 4));
        transform.position += turn;
        transform.Rotate(0, +11.25f, 0, Space.Self);
    }
    //move second half of right turn
    private void rightMove2()
    {
        turn = new Vector3((postTurn.x - rightMid.x) / 4, 0, (postTurn.z - rightMid.z) / 4);
        transform.position += turn;
        transform.Rotate(0, +11.25f, 0, Space.Self);
    }

    private void returnForwardLeft()
    {
        //change forward facing direction after turn
        if (forwardDirection == "North")
        {
            forwardDirection = "West";
        }
        else if (forwardDirection == "South")
        {
            forwardDirection = "East";
        }
        else if (forwardDirection == "East")
        {
            forwardDirection = "North";
        }
        else
        {
            forwardDirection = "South";
        }
        forward = true;
    }
    private void returnForwardRight()
    {
        //change forward facing direction after turn
        if (forwardDirection == "North")
        {
            forwardDirection = "East";
        }
        else if (forwardDirection == "South")
        {
            forwardDirection = "West";
        }
        else if (forwardDirection == "East")
        {
            forwardDirection = "South";
        }
        else
        {
            forwardDirection = "North";
        }
        forward = true;
    }

    private void act()
    {
        //movement in a straight line
        if (forward == true && left == false && right == false)
        {
            if (forwardDirection == "North")
            {
                transform.position = transform.position + moveSpeedNorth;
            }
            else if (forwardDirection == "South")
            {
                transform.position = transform.position + moveSpeedSouth;
            }
            else if (forwardDirection == "East")
            {
                transform.position = transform.position + moveSpeedEast;
            }
            else
            {
                transform.position = transform.position + moveSpeedWest;
            }
        }

        //left turn function
        else if (left == true && right == false && forward == false)
        {
            //pre turn
            current = transform.position;

            //find intersect of given roads
            if (forwardDirection == "North" || forwardDirection == "South")
            {
                intersect = new Vector3(roadCurrent.transform.position.x, 1, roadPost.transform.position.z);
            }
            else
            {
                intersect = new Vector3(roadPost.transform.position.x, 1, roadCurrent.transform.position.z);
            }

            //find post turn of roads
            if (forwardDirection == "North")
            {
                postTurn = new Vector3(intersect.x - Math.Abs(intersect.z - current.z), 1, intersect.z + Math.Abs(intersect.x - current.x));
            }
            else if (forwardDirection == "South")
            {
                postTurn = new Vector3(intersect.x + Math.Abs(intersect.z - current.z), 1, intersect.z - Math.Abs(intersect.x - current.x));
            }
            else if (forwardDirection == "East")
            {
                postTurn = new Vector3(intersect.x + Math.Abs(intersect.z - current.z), 1, intersect.z + Math.Abs(intersect.x - current.x));
            }
            else
            {
                postTurn = new Vector3(intersect.x - Math.Abs(intersect.z - current.z), 1, intersect.z - Math.Abs(intersect.x - current.x));
            }

            //amount turn per interval
            turn = new Vector3(((intersect.x - current.x) / 4), 0, ((intersect.z - current.z) / 4));

            //make left turn
            for (x = 1; x < 5; x++)
            {
                Invoke("leftMove", x * waitTime);
            }
            current = transform.position;
            for (x = 4; x < 8; x++)
            {
                Invoke("leftMove2", x * waitTime);
            }
            left = false;
            Invoke("returnForwardLeft", x * waitTime);
        }

        //right turn function
        else if (right == true && left == false && forward == false)
        {
            //pre turn
            current = transform.position;

            //find intersect of given roads
            if (forwardDirection == "North" || forwardDirection == "South")
            {
                intersect = new Vector3(roadCurrent.transform.position.x, 1, roadPost.transform.position.z);
            }
            else
            {
                intersect = new Vector3(roadPost.transform.position.x, 1, roadCurrent.transform.position.z);
            }

            //find post turn of roads
            if (forwardDirection == "North")
            {
                postTurn = new Vector3(intersect.x + Math.Abs(intersect.z - current.z), 1, intersect.z - Math.Abs(intersect.x - current.x));
            }
            else if (forwardDirection == "South")
            {
                postTurn = new Vector3(intersect.x - Math.Abs(intersect.z - current.z), 1, intersect.z + Math.Abs(intersect.x - current.x));
            }
            else if (forwardDirection == "East")
            {
                postTurn = new Vector3(intersect.x - Math.Abs(intersect.z - current.z), 1, intersect.z - Math.Abs(intersect.x - current.x));
            }
            else
            {
                postTurn = new Vector3(intersect.x + Math.Abs(intersect.z - current.z), 1, intersect.z + Math.Abs(intersect.x - current.x));
            }

            //middle of pre and post
            middle = new Vector3((current.x + postTurn.x) / 2, 1, (current.z + postTurn.z) / 2);
            //middle of right turn 
            rightMid = new Vector3((intersect.x + middle.x) / 2, 1, (intersect.z + middle.z) / 2);
            //amount turn per interval
            turn = new Vector3(((rightMid.x - current.x) / 4), 0, ((rightMid.z - current.z) / 4));

            //do right turn
            for (x = 1; x < 5; x++)
            {
                Invoke("rightMove", x * waitTime);
            }
            current = transform.position;
            for (x = 4; x < 8; x++)
            {
                Invoke("rightMove2", x * waitTime);
            }
            right = false;
            Invoke("returnForwardRight", x * waitTime);
        }

        //at intersection
       /* else if(forward == false && left == false && right == false && (sensorFirstLightScript.intersection==true|| sensorFirstSignScript.intersection == true))
        {
            //transform.position = new Vector3(transform.position.x,1,roadPost.transform.position.z-8);
            if(decision[index]==0)
            {
                left = true;
            }
            else if (decision[index] == 1)
            {
                forward = true;
            }
            else 
            {
                right = true;            
            }
           // index++;
        }*/
    }

    //car finds its initial position
    void Start()
    {
        start = transform.position;
        sensorFirstSign = GameObject.FindGameObjectWithTag("firstSensorSign");
        sensorFirstSignScript =(SensorFirstIteration)sensorFirstSign.GetComponent(typeof(SensorFirstIteration));
        sensorFirstLight = GameObject.FindGameObjectWithTag("firstSensorLight");
        sensorFirstLightScript = (SensorFirstIteration)sensorFirstLight.GetComponent(typeof(SensorFirstIteration));
        leftRoadSensor = GameObject.FindGameObjectWithTag("leftSensor"); // Finds the GameObject of the Car
        leftSensorScript = (RoadSensor)leftRoadSensor.GetComponent(typeof(RoadSensor)); // Gets the Car's CarController script
        rightRoadSensor = GameObject.FindGameObjectWithTag("rightSensor"); // Finds the GameObject of the Car
        rightSensorScript = (RoadSensor)rightRoadSensor.GetComponent(typeof(RoadSensor)); // Gets the Car's CarController script
        currentRoadSensor = GameObject.FindGameObjectWithTag("currentSensor"); // Finds the GameObject of the Car
        currentSensorScript = (CurrentRoad)currentRoadSensor.GetComponent(typeof(CurrentRoad)); // Gets the Car's CarController script
    }

    // Update is called once per frame
    void Update()
    {

        act();
        
    }
}
