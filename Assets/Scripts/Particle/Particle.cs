using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Particle : MonoBehaviour
{
    public SimController myController;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public float speed = 0;
    bool isRunning = false;
    public Text floatText;
    public void Initialize()
    {
        mass = DefaultMass();
        Transform canvasTransform = this.transform.GetChild(0);
        Transform textTransform = canvasTransform.GetChild(0);
        floatText = textTransform.gameObject.GetComponent<Text>();
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
