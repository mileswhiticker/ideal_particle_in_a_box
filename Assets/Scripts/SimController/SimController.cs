using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    private int startingParticles = 6;
    private float startingTemp = 293f;  //kelvin
    public float Temp()
    {
        return startingTemp;
    }
    public List<Particle> particles = new List<Particle>();
    public GameObject particlePrefab;
    public GameObject wallPrefab;
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

    private float timeRate = 1;
    public float TimeRate()
    {
        return timeRate;
    }

    private float tau = 0.0f;
    private float timeMax = 1;
    private float timeStep = 1;
    private float stepMax = 1;

    public bool isSimRunning = false;

    void Start()
    {
        Initialize();
        isSimRunning = true;
    }

    public Text timeText;
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
    void Update()
    {
        simTime += Time.deltaTime;
        timeText.text = "" + simTime;

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
    }
}
