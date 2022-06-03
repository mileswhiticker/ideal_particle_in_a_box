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
            simTime += SimDeltaTime();  //1/60 sec by default
            runningTime += SimDeltaTime();

            timeText.text = "Trial time: " + simTime;
            runningTimeText.text = "Running time: " + runningTime;

            //update most of the simulation values
            UpdateValues();

            //update other stuff
            UpdateCosmicRays();
            UpdateTemp();

            tLeftLogData -= Time.deltaTime;
        }
        else
        {
            //stop this trial and get ready for the next one
            isSimRunning = false;
            if (!logCreated)
            {
                //log this trial
                /*Log.AddLine("errors");
                foreach (float curError in this.sqrVelocityError)
                {
                    Log.AddLine("" + curError + ",...");
                }
                */

                Log.AddLine("");
                /*Log.AddLine("uncertainty propogation factor");
                float modifiedBoltzmann = 1.38f * Mathf.Pow(10, -3) * 4;
                float propogatefactor = 0.5f * Particle.DefaultMass() * 2 / (3 * modifiedBoltzmann);
                Log.AddLine("" + propogatefactor);
                */
                logCreated = true;

                /*
                //Log.AddLine("Length:" + curLength + ", pressure:" + averagePressure);
                string dataLine = "" + averagePressure;
                logCreated = true;
                trialIndex++;

                //are we at max trials for this width?
                if (trialIndex >= trialMax)
                {
                    //go to next width
                    widthIndex++;

                    //do we have new widths to trial?
                    if (widthIndex < trialWidths.Count)
                    {
                        //set the new wall length and reset the trial counter
                        trialIndex = 0;
                        curLength = trialWidths[widthIndex];
                        dataLine += ";...";
                    }
                    else
                    {
                        dataLine += ",...";
                    }
                }*/
                //Log.AddLine(dataLine);

                /*
                //should we trial the current width?
                if (trialIndex < trialMax)
                {
                    //next trial for this width
                    Initialize();
                    isSimRunning = true;
                }
                else
                {
                    //we are finished
                    foreach (float rms in squaredMeanVelocityHorizontal)
                    {
                        Log.AddLine("" + rms + ",...");
                    }
                }*/
            }
        }
    }
}
