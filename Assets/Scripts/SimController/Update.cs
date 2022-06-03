using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    private bool logCreated = false;
    void Update()
    {
        PollInput();

        if (simTime < timeMax || timeMax <= 0)
        {
            if(tLeftLogData <= 0)
            {
                tLeftLogData = logDataInterval;
            }
            tLeftLogData -= Time.deltaTime;
            simTime += SimDeltaTime();  //1/60 sec by default
            runningTime += SimDeltaTime();

            timeText.text = "Trial time: " + simTime;
            runningTimeText.text = "Running time: " + runningTime;

            //update most of the simulation values
            UpdateValues();

            //update other stuff
            UpdateCosmicRays();
            UpdateTemp();
        }
        else
        {
            //stop this trial and get ready for the next one
            isSimRunning = false;
            FinishTrial();
        }
    }
}
