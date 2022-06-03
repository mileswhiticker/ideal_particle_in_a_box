using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    public bool doAvgVelocityUpdate = false;
    public float averageVelocitySqr = 0;
    public Material RedMaterial;
    private int startingParticles = 100;
    private float timeMax = 99999;
    private float startingTemp = 300f;  //kelvin
    public float currentTemp = 300;
    public Text particleVelocity;
    public Text particleMass;
    public Text temp;
    public Text sidelength;
    public Text volume;
    //private int trialIndex = 0;
    private int trialMax = 1;
    //private int widthIndex = 0;
    private List<float> trialWidths = new List<float>();
    private List<GameObject> walls = new List<GameObject>();
    private bool doInteractions = true;
    public List<List<float>> squaredVelocities = new List<List<float>>();
    public List<float> sqrVelocityError = new List<float>();
    public float tLeftLogData = 0.1f;
    public float logDataInterval = 0.1f;
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
    void Start()
    {
        ApplicationStartTime = System.DateTime.UtcNow;
        Log.logID = "" + ApplicationStartTime.Day + "_" + ApplicationStartTime.Month + "_" + ApplicationStartTime.Year + "_" + ApplicationStartTime.Ticks;
        Log.AddLine("Sim started " + ApplicationStartTime);

        //what widths are we testing?
        /*
        trialWidths.Add(0.5f);
        trialWidths.Add(1);
        trialWidths.Add(1.5f);
        trialWidths.Add(2);
        trialWidths.Add(2.5f);
        */
        trialWidths.Add(6f);
        
        //output our initial data
        for(int i = 0; i < trialWidths.Count; i++)
        {
            for (int j = 0; j < trialMax; j++)
            {
                Log.AddLine("" + trialWidths[i]);
            }
        }
        
        //start with this one
        curLength = trialWidths[0];

        Initialize();
        endTimeText.text = "Sim end time: " + trialWidths.Count * trialMax * timeMax;
        isSimRunning = true;
    }

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
}
