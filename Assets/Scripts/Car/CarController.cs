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
    float forwardDirect = 0;
    //temporary way to chose turns at intersections to be raplaced with gps
    public int[] decision = { 0, 0, 0, 0 };
    public int index = 0;
    //speed of the turn
    float waitTime = 0.15f;
    //time for smooth turn
    int x;

    public bool intersection = false;
    public bool atLight = false;
    public bool at2Way = false;
    public bool at4Way = false;
    public bool fourWayStopCheck = true;
    public bool carInIntersectionLeft = false;
    public bool carInIntersectionForward = false;
    public bool carInIntersectionRight = false;
    public bool carInIntersection = false;
    public int safeLeft = 0;
    public int safeRight = 0;

    //vectors for movement speed and turns
    Vector3 start;
    Vector3 moveSpeedNorth = new Vector3(0, 0, 0.05f);
    Vector3 moveSpeedSouth = new Vector3(0, 0, -0.05f);
    Vector3 moveSpeedEast = new Vector3(0.05f, 0, 0);
    Vector3 moveSpeedWest = new Vector3(-0.05f, 0, 0);
    Vector3 movementVector = new Vector3(0, 0, 0.05f);
    Vector3 current;
    Vector3 postTurn;
    Vector3 postTurnVector;
    Vector3 intersect;
    Vector3 intersectionVector;
    Vector3 turn;
    Vector3 turnDegreeVector;
    Vector3 rightMid;
    Vector3 middle;

    float x1;
    float y1;
    float x2;
    float y2;
    float l;
    float ang;
    float m1;
    float m2;
    float b1;
    float b2;
    float intersectX;
    float radAngle;
    bool ninteyTurn  = false;


    //road currently on and road to turn onto at intersection
    public GameObject roadCurrent;
    public GameObject roadPost;
    public GameObject LeftLane;
    
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

    //finds the position of the center of intersection and where the car must turn to before making a turn.
    private void findIntersection()
    {
        //good
       if (roadCurrent.transform.eulerAngles.y == 0 && roadPost.transform.eulerAngles.y == 90)
       {
            //Debug.Log(roadCurrent.transform.position.x);
            intersectionVector = new Vector3(roadCurrent.transform.position.x,1f,roadPost.transform.position.z);
       }
        //good
        else if (roadCurrent.transform.eulerAngles.y == 90 && roadPost.transform.eulerAngles.y == 0)
       {
           // Debug.Log("hellooo");
            intersectionVector = new Vector3(roadPost.transform.position.x, 1f, roadCurrent.transform.position.z);
       }
        //good
        else if (roadCurrent.transform.eulerAngles.y == 0)
       {
           // Debug.Log(roadCurrent.transform.position.x);
            //Debug.Log(Mathf.Tan(roadPost.transform.eulerAngles.y * Mathf.PI / 180));
           // Debug.Log(roadPost.transform.eulerAngles.y);
            b2 = roadPost.transform.position.z - ((Mathf.Tan(roadPost.transform.eulerAngles.y * Mathf.PI / 180))* roadPost.transform.position.x);
           // Debug.Log(b2);
            intersectionVector = new Vector3(roadCurrent.transform.position.x, 1f, ((Mathf.Tan(roadPost.transform.eulerAngles.y * Mathf.PI / 180))* roadCurrent.transform.position.x)+b2);
       }
        //good
        else if (roadCurrent.transform.eulerAngles.y == 90)
       {
            b2 = roadPost.transform.position.z - ((Mathf.Tan(roadPost.transform.eulerAngles.y * Mathf.PI / 180)) * roadPost.transform.position.x);
            intersectionVector = new Vector3((roadCurrent.transform.position.z-b2)/(Mathf.Tan(roadPost.transform.eulerAngles.y * Mathf.PI / 180)), 1f,roadCurrent.transform.position.z);
       }
       //good
       else if (roadPost.transform.eulerAngles.y == 0)
       {
            b1 = roadCurrent.transform.position.z - ((Mathf.Tan(roadCurrent.transform.eulerAngles.y * Mathf.PI / 180)) * roadCurrent.transform.position.x);
            intersectionVector = new Vector3(roadPost.transform.position.x, 1f, ((Mathf.Tan(roadCurrent.transform.eulerAngles.y * Mathf.PI / 180)) * roadPost.transform.position.x) + b2);
          //  Debug.Log(intersectionVector + " intersection");
        }
       else if (roadPost.transform.eulerAngles.y == 90)
       {
            b1 = roadCurrent.transform.position.z - ((Mathf.Tan(roadCurrent.transform.eulerAngles.y * Mathf.PI / 180)) * roadCurrent.transform.position.x);
            intersectionVector = new Vector3((roadPost.transform.position.z - b1) / (Mathf.Tan(roadCurrent.transform.eulerAngles.y * Mathf.PI / 180)), 1f, roadPost.transform.position.z);
       }
       else
       {
            b1 = roadCurrent.transform.position.z - ((Mathf.Tan(roadCurrent.transform.eulerAngles.y * Mathf.PI / 180)) * roadCurrent.transform.position.x);
            b2 = roadPost.transform.position.z - ((Mathf.Tan(roadPost.transform.eulerAngles.y * Mathf.PI / 180)) * roadPost.transform.position.x);
            intersectX = (b2 - b1) / ((Mathf.Tan(roadCurrent.transform.eulerAngles.y * Mathf.PI / 180)) - (Mathf.Tan(roadPost.transform.eulerAngles.y * Mathf.PI / 180)));
            intersectionVector = new Vector3(intersectX, 1f, ((Mathf.Tan(roadCurrent.transform.eulerAngles.y * Mathf.PI / 180))*intersectX)+b1);
       }
        
       l = Mathf.Sqrt(Mathf.Pow(intersectionVector.x - transform.position.x,2) + Mathf.Pow(intersectionVector.y - transform.position.y,2));
       if (right == true)
       {
            if (roadPost.transform.eulerAngles.y > roadCurrent.transform.eulerAngles.y)
            {
                radAngle = (180 - (Math.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y))) * Mathf.PI / 180;
            }
            else
            {
                radAngle = ((Math.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y))) * Mathf.PI / 180;
            }
        }
       else
       {
            if(roadPost.transform.eulerAngles.y> roadCurrent.transform.eulerAngles.y)
            {
                radAngle = (-(Mathf.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y))) * Mathf.PI / 180;
            }
            else
            {
                radAngle = (-(180-(Mathf.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y)))) * Mathf.PI / 180;
            }
       }
       postTurnVector = new Vector3(Mathf.Cos(radAngle) * (LeftLane.transform.position.x - intersectionVector.x) - Mathf.Sin(radAngle) * (LeftLane.transform.position.z - intersectionVector.z) + intersectionVector.x, 1f, Mathf.Sin(radAngle) * (LeftLane.transform.position.x - intersectionVector.x) + Mathf.Cos(radAngle) * (LeftLane.transform.position.z - intersectionVector.z) + intersectionVector.z);
        //Debug.Log(postTurnVector+" post turn");
       // Debug.Log(-(Mathf.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y)));
    }

    //move to first half of left turn
    private void leftTurnMove()
    {
        turn = new Vector3(((intersectionVector.x - current.x) / 4), 0, ((intersectionVector.z - current.z) / 4));
        transform.position += turn;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - turnDegreeVector.y, 0);
    }
    //move to second half of left turn
    private void leftTurnMove2()
    {
        turn = new Vector3((postTurnVector.x - intersectionVector.x) / 4, 0, (postTurnVector.z - intersectionVector.z) / 4);
        transform.position += turn;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - turnDegreeVector.y, 0);

    }
    //move first half of right turn
    private void rightTurnMove()
    {
        turn = new Vector3(((rightMid.x - current.x) / 4), 0, ((rightMid.z - current.z) / 4));
        transform.position += turn;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + turnDegreeVector.y, 0);
    }
    //move second half of right turn
    private void rightTurnMove2()
    {
        turn = new Vector3((postTurnVector.x - rightMid.x) / 4, 0, (postTurnVector.z - rightMid.z) / 4);
        transform.position += turn;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + turnDegreeVector.y, 0);
    }

    //decides forward direction after left turn and sets movement vector to fit correct direction.
    private void returnForwardLeft()
    {
        forwardDirect = transform.eulerAngles.y;
        //Debug.Log(forwardDirect);

        if (forwardDirect <= 90)
        {
            movementVector = new Vector3(0.5f * (forwardDirect / 90), 0, 0.5f - (0.5f * (forwardDirect / 90)));
        }
        else if (forwardDirect > 90 && forwardDirect < 180)
        {
            movementVector = new Vector3(0.5f - (0.5f * ((forwardDirect - 90) / 90)), 0, -(0.5f * ((forwardDirect - 90) / 90)));
        }
        else if (forwardDirect == 180)
        {
            movementVector = new Vector3(0, 0, -0.5f);
        }
        else if (forwardDirect > 180 && forwardDirect < 270)
        {
            movementVector = new Vector3(-0.5f * ((forwardDirect - 180) / 90), 0, -0.5f * ((forwardDirect - 180) / 90));
        }
        else if(forwardDirect == 270)
        {
            movementVector = new Vector3(-0.5f, 0,0);
        }
        else
        {
            movementVector = new Vector3(-(0.5f * ((forwardDirect - 270) / 90)), 0, 0.5f - (0.5f * ((forwardDirect - 270) / 90)));
        }

        movementVector = movementVector / 4;
        forward = true;
    }
   
    //decides forward direction after right turn and sets movement vector to fit correct direction.
    private void returnForwardRight()
    {
        forwardDirect = transform.eulerAngles.y;
        if (forwardDirect <= 90)
        {
            movementVector = new Vector3(0.5f * (forwardDirect / 90), 0, 0.5f - (0.5f * (forwardDirect / 90)));
        }
        else if (forwardDirect > 90 && forwardDirect <= 180)
        {
            movementVector = new Vector3(0.5f - (0.5f * ((forwardDirect - 90) / 90)), 0, -(0.5f * ((forwardDirect - 90) / 90)));
        }
        else if (forwardDirect > 180 && forwardDirect < 270)
        {
            movementVector = new Vector3(0.5f * ((forwardDirect - 180) / 90), 0, -(0.5f - (0.5f * ((forwardDirect - 180) / 90))));
        }
        else if (forwardDirect == 270)
        {
            movementVector = new Vector3(-0.5f, 0, 0);
        }
        else
        {
            movementVector = new Vector3(-(0.5f * ((forwardDirect - 270) / 90)), 0, 0.5f - (0.5f * ((forwardDirect - 270) / 90)));
        }

        movementVector = movementVector / 4;
        forward = true;
    }

    //the actions the car will make when going left right or straight.
    void action()
    {
        //if car is to go straight
        if (forward == true && left == false && right == false)
        {
            transform.position = transform.position + movementVector;
        }

        //if the car is to go left
        else if (left == true && right == false && forward == false)
        {
            findIntersection();
            current = transform.position;
            //amount turn per interval
            turn = new Vector3(((intersectionVector.x - current.x) / 4), 0, ((intersectionVector.z - current.z) / 4));
            if (roadPost.transform.eulerAngles.y > roadCurrent.transform.eulerAngles.y)
            {
                turnDegreeVector = new Vector3(0, ((180 - Mathf.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y)) / 8), 0);
            }
            else
            {
                turnDegreeVector = new Vector3(0, Mathf.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y) / 8, 0);
            }

            //make left turn
            for (x = 1; x < 5; x++)
            {
                Invoke("leftTurnMove", x * waitTime);
            }
            current = transform.position;
            for (x = 4; x < 8; x++)
            {
                Invoke("leftTurnMove2", x * waitTime);
            }
            left = false;
            Invoke("returnForwardLeft", x * waitTime);
        }

        //if the car is to go right
        else if (right == true && left == false && forward == false)
        {
            findIntersection();
            current = transform.position;

            //middle of pre and post
            middle = new Vector3((current.x + postTurnVector.x) / 2, 1, (current.z + postTurnVector.z) / 2);
            //middle of right turn 
            rightMid = new Vector3((intersectionVector.x + middle.x) / 2, 1, (intersectionVector.z + middle.z) / 2);
            //amount turn per interval
            turn = new Vector3(((rightMid.x - current.x) / 4), 0, ((rightMid.z - current.z) / 4));

            if (roadPost.transform.eulerAngles.y > roadCurrent.transform.eulerAngles.y)
            {
                turnDegreeVector = new Vector3(0, (Mathf.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y) / 8), 0);
            }
            else
            {
                turnDegreeVector = new Vector3(0, (180-Mathf.Abs(roadPost.transform.eulerAngles.y - roadCurrent.transform.eulerAngles.y)) / 8, 0);
            }

            //do right turn
            for (x = 1; x < 5; x++)
            {
                Invoke("rightTurnMove", x * waitTime);
            }
            current = transform.position;
            for (x = 4; x < 8; x++)
            {
                Invoke("rightTurnMove2", x * waitTime);
            }
            right = false;
            Invoke("returnForwardRight", x * waitTime);
        }
    }

    //car finds its initial position
    //turn on sensors
    void Start()
    {
        start = transform.position;
        sensorFirstSign = GameObject.FindGameObjectWithTag("firstSensorSign");
        sensorFirstSignScript =(SensorFirstIteration)sensorFirstSign.GetComponent(typeof(SensorFirstIteration));
        sensorFirstLight = GameObject.FindGameObjectWithTag("firstSensorLight");
        sensorFirstLightScript = (SensorFirstIteration)sensorFirstLight.GetComponent(typeof(SensorFirstIteration));
        leftRoadSensor = GameObject.FindGameObjectWithTag("leftSensor");
        leftSensorScript = (RoadSensor)leftRoadSensor.GetComponent(typeof(RoadSensor));
        rightRoadSensor = GameObject.FindGameObjectWithTag("rightSensor");
        rightSensorScript = (RoadSensor)rightRoadSensor.GetComponent(typeof(RoadSensor));
        currentRoadSensor = GameObject.FindGameObjectWithTag("currentSensor");
        currentSensorScript = (CurrentRoad)currentRoadSensor.GetComponent(typeof(CurrentRoad));
    }

    // Update is called once per frame
    void Update()
    {

        //act();
        action();

    }
}
