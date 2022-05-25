using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Particle : MonoBehaviour
{
    public SimController myController;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public float speed = 0;
    bool isRunning = false;
    public void Initialize()
    {
        mass = DefaultMass();
    }
    float tLeftStep = 0.5f;
    public bool doDebug = false;
    public static float stepDelay = 0.0f;
    void Update()
    {
        if (isRunning && myController != null && myController.isSimRunning)
        {
            //reduce the step update rate
            tLeftStep -= Time.deltaTime;
            if (tLeftStep <= 0)
            {
                tLeftStep = stepDelay;
                IntegrateMovement();
            }
        }
    }
}
