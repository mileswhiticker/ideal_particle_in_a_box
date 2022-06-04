using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class SimController : MonoBehaviour
{
    protected void FinishTrial()
    {
        if (!logCreated)
        {
            logCreated = true;

            //log this trial
            Log.AddLine("emits per tick");
            foreach (float curVal in RaysEmitted)
            {
                Log.AddLine("" + curVal + ",...");
            }
            Log.AddLine("collisions per tick");
            foreach (float curVal in RayCollisions)
            {
                Log.AddLine("" + curVal + ",...");
            }


            /*Log.AddLine("average distance");
            foreach(float curVal in avgParticleDistSqr)
            {
                Log.AddLine("" + curVal + ",...");
            }*/

            /*
            //pressures
            Log.AddLine("skyPressures");
            foreach (float curVal in skyPressures)
            {
                Log.AddLine("" + curVal + ",...");
            }
            Log.AddLine("groundPressures");
            foreach (float curVal in groundPressures)
            {
                Log.AddLine("" + curVal + ",...");
            }
            Log.AddLine("horizontalPressures");
            foreach (float curVal in horizontalPressures)
            {
                Log.AddLine("" + curVal + ",...");
            }
            Log.AddLine("avgPressures");
            foreach (float curVal in avgPressures)
            {
                Log.AddLine("" + curVal + ",...");
            }
            */

            /*Log.AddLine("uncertainty propogation factor");
            float modifiedBoltzmann = 1.38f * Mathf.Pow(10, -3) * 4;
            float propogatefactor = 0.5f * Particle.DefaultMass() * 2 / (3 * modifiedBoltzmann);
            Log.AddLine("" + propogatefactor);
            */

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
