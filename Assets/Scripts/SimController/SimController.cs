using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    public Material RedMaterial;
    public Material YellowMaterial;
    public Material GreenMaterial;
    private int startingParticles = 125;
    private float timeMax = 99999;
    private float startingTemp = 300f;  //kelvin
    public float currentTemp = 300;
    public Text particleVelocity;
    public Text particleMass;
    public Text temp;
    public Text sidelength;
    public Text volume;
    public Text avgParticleDist;
    public GameObject ground;
    //private int trialIndex = 0;
    private int trialMax = 1;
    //private int widthIndex = 0;
    private bool doSimulate3D = true;
    public bool DoSimulate3D()
    {
        return doSimulate3D;
    }
    private List<float> trialWidths = new List<float>();
    private List<GameObject> walls = new List<GameObject>();
    private bool doInteractions = true;

    public List<float> horizontalPressures = new List<float>();
    public List<float> skyPressures = new List<float>();
    public List<float> groundPressures = new List<float>();
    public List<float> avgPressures = new List<float>();

    public float netSquaredVelocity = 0;
    public float numContributingSqrVelocity = 0;
    private bool doLogVelocities = false;
    private float currentAvgSqrVelocity = 0;
    public bool DoLogVelocities()
    {
        return doLogVelocities;
    }
    public List<float> avgSqrVelocities = new List<float>();

    private List<int> RayCollisions = new List<int>();
    private List<int> RaysEmitted = new List<int>();
    private int numRayCollissionsThisTick = 0;
    private int numRaysThisTick = 0;
    private float tLeftLogDataRays = 4.9f;
    private float logDataIntervalRays = 5f;

    public List<float> temps = new List<float>();

    public List<float> avgParticleDistSqr = new List<float>();
    public float currentParticleDistSqr = 0;
    public int numParticlesContributingToDistSqr = 0;

    private float tLeftLogData = -0.1f;
    public float TLeftLogData()
    {
        return tLeftLogData;
    }
    private float logDataInterval = 1f;
    public bool DoInteractions()
    {
        return doInteractions;
    }

    private float timeRate = 1f;
    public float Temp()
    {
        return currentTemp;
    }
    public List<Particle> particles = new List<Particle>();
    public GameObject particlePrefab;
    public GameObject wallPrefab;
    private float curLength = 6;
    private Vector3 boundsMin = new Vector3(-1, -1, -1);
    List<float> squaredMeanVelocityHorizontal = new List<float>();
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

    public Text timeText;
    public Text pressureText;
    public Text runningTimeText;
    public Text endTimeText;
    //
    public float simTime = 0;
    public float runningTime = 0;
    //
    public float avgForceLeft = 0;
    public float avgForceRight = 0;
    public float avgForceAbove = 0;
    public float avgForceBelow = 0;
    public float avgForceTop = 0;
    public float avgForceBottom = 0;
    //
    public Text leftForceText;
    public Text aboveForceText;
    public Text belowForceText;
    public Text rightForceText;
    public Text topForceText;
    public Text bottomForceText;
    //
    public float avgPressureLeft = 0;
    public float avgPressureRight = 0;
    public float avgPressureAbove = 0;
    public float avgPressureBelow = 0;
    public float avgPressureTop = 0;
    public float avgPressureBottom = 0;
    //
    public Text leftText;
    public Text rightText;
    public Text aboveImpulseText;
    public Text belowImpulseText;
    public Text topText;
    public Text bottomText;
    //
    public float totalImpulseAbove = 0;
    public float totalImpulseBelow = 0;
    public float totalImpulseLeft = 0;
    public float totalImpulseRight = 0;
    public float totalImpulseTop = 0;
    public float totalImpulseBottom = 0;
    //
    public Text leftPressureText;
    public Text rightPressureText;
    public Text abovePressureText;
    public Text belowPressureText;
    public Text topPressureText;
    public Text bottomPressureText;
    public float averagePressure;
}
