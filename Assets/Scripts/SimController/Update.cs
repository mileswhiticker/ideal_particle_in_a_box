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
            if (!logCreated)
            {
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
                }
                Log.AddLine(dataLine);

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
                }
            }
        }
    }
}
