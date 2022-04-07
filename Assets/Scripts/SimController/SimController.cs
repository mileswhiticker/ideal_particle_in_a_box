using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    private int startingParticles = 1;
    private float timeMax = 60;
    private float startingTemp = 300f;  //kelvin
    public Text particleVelocity;
    public Text particleMass;
    public Text temp;
    private float timeRate = 100;
    public float Temp()
    {
        return startingTemp;
    }
    public List<Particle> particles = new List<Particle>();
    public GameObject particlePrefab;
    public GameObject wallPrefab;
    private float curLength = 3;
    private Vector3 boundsMin = new Vector3(-1, -1, -1);
    public Vector3 BoundsMin()
    {
        return boundsMin;
    }
    private Vector3 boundsMax = new Vector3(1, 1, 1);
    public Vector3 BoundsMax()
    {
        return boundsMax;
    }
    private Vector3 defaultVelocity = new Vector3(2, 0, 2);
    public Vector3 DefaultVelocity()
    {
        return defaultVelocity;
    }
    public float TimeRate()
    {
        return timeRate;
    }
    public float SimDeltaTime()
    {
        //dont overshoot maximum sim duration
        float simDeltaTime = Time.deltaTime * TimeRate();
        if(simTime + simDeltaTime >= timeMax)
        {
            simDeltaTime = timeMax - simTime;
        }
        return simDeltaTime;
    }

    public bool isSimRunning = false;

    public System.DateTime ApplicationStartTime;
    void Start()
    {
        Initialize();
        Log.logID = "" + Random.Range(1111111,9999999);
        ApplicationStartTime = System.DateTime.UtcNow;
        Log.AddLine("Sim started " + ApplicationStartTime);
        isSimRunning = true;
    }

    public Text timeText;
    public Text pressureText;
    //
    public float simTime = 0;
    public float avgForceLeft = 0;
    public float avgForceRight = 0;
    public float avgForceTop = 0;
    public float avgForceBottom = 0;
    //
    public Text leftForceText;
    public Text rightForceText;
    public Text topForceText;
    public Text bottomForceText;
    //
    public float avgPressureLeft = 0;
    public float avgPressureRight = 0;
    public float avgPressureTop = 0;
    public float avgPressureBottom = 0;
    //
    public Text leftPressureText;
    public Text rightPressureText;
    public Text topPressureText;
    public Text bottomPressureText;
    public float averagePressure;

    bool logCreated = false;
    void Update()
    {
        if(simTime < timeMax || timeMax <= 0)
        {
            simTime += SimDeltaTime();
            timeText.text = "Time: " + simTime;

            //calculate average force on each wall
            avgForceLeft = totalImpulseLeft / simTime;
            avgForceRight = totalImpulseRight / simTime;
            avgForceTop = totalImpulseTop / simTime;
            avgForceBottom = totalImpulseBottom / simTime;
            //
            leftForceText.text = "Force: " + avgForceLeft;
            rightForceText.text = "Force: " + avgForceRight;
            topForceText.text = "Force: " + avgForceTop;
            bottomForceText.text = "Force: " + avgForceBottom;

            //use force and walldims to calculate average pressure
            leftPressureText.text = "Pressure: " + (avgForceLeft / (boundsMax.x - boundsMin.x));
            rightPressureText.text = "Pressure: " + (avgForceRight / (boundsMax.x - boundsMin.x));
            topPressureText.text = "Pressure: " + (avgForceTop / (boundsMax.z - boundsMin.z));
            bottomPressureText.text = "Pressure: " + (avgForceBottom / (boundsMax.z - boundsMin.z));

            //calculate the average linear pressure on the 2D box
            averagePressure = (avgForceLeft / (boundsMax.x - boundsMin.x) + (avgForceRight / (boundsMax.x - boundsMin.x)) + (avgForceTop / (boundsMax.z - boundsMin.z)) + (avgForceBottom / (boundsMax.z - boundsMin.z))) / 4;
            pressureText.text = "Avg pressure: " + averagePressure;
        }
        else
        {
            isSimRunning = false;
            if(!logCreated)
            {
                Log.AddLine("Length:" + curLength + ", pressure:" + averagePressure);
                logCreated = true;
            }
        }
    }
}
