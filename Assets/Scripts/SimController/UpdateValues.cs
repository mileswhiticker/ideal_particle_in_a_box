using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    protected void UpdateValues()
    {
        //calculate average force on each wall
        avgForceLeft = totalImpulseLeft / simTime;
        avgForceRight = totalImpulseRight / simTime;
        avgForceAbove = totalImpulseAbove / simTime;
        avgForceBelow = totalImpulseBelow / simTime;
        avgForceTop = totalImpulseTop / simTime;
        avgForceBottom = totalImpulseBottom / simTime;
        //
        leftForceText.text = "Force: " + avgForceLeft;
        rightForceText.text = "Force: " + avgForceRight;
        aboveForceText.text = "Force: " + avgForceAbove;
        belowForceText.text = "Force: " + avgForceBelow;
        topForceText.text = "Force: " + avgForceTop;
        bottomForceText.text = "Force: " + avgForceBottom;

        //use force and walldims to calculate average pressure
        avgPressureLeft = avgForceLeft / (boundsMax.x - boundsMin.x);
        avgPressureRight = avgForceRight / (boundsMax.x - boundsMin.x);
        avgPressureAbove = avgForceAbove / (boundsMax.y - boundsMin.y);
        avgPressureBelow = avgForceBelow / (boundsMax.y - boundsMin.y);
        avgPressureTop = avgForceTop / (boundsMax.z - boundsMin.z);
        avgPressureBottom = avgForceBottom / (boundsMax.z - boundsMin.z);
        //
        leftPressureText.text = "Pressure: " + avgPressureLeft;
        rightPressureText.text = "Pressure: " + avgPressureRight;
        abovePressureText.text = "Pressure: " + avgPressureAbove;
        belowPressureText.text = "Pressure: " + avgPressureBelow;
        topPressureText.text = "Pressure: " + avgPressureTop;
        bottomPressureText.text = "Pressure: " + avgPressureBottom;

        //calculate the average linear pressure on the 2D box
        averagePressure = (avgPressureLeft + avgPressureRight + avgPressureTop + avgPressureBottom + avgPressureAbove + avgPressureBelow) / 6;
        //
        pressureText.text = "Avg pressure: " + averagePressure;

        currentParticleDistSqr /= (float)numParticlesContributingToDistSqr;
        avgParticleDistSqr.Add(currentParticleDistSqr);
        avgParticleDist.text = "Avg particle dist sqr: " + currentParticleDistSqr;

        numParticlesContributingToDistSqr = 0;
        currentParticleDistSqr = 0;

        //some logging
        if (tLeftLogData < 0)
        {
            skyPressures.Add(avgPressureAbove);
            groundPressures.Add(avgPressureBelow);
            horizontalPressures.Add((avgPressureLeft + avgPressureRight + avgPressureTop + avgPressureBottom) / 4);
            avgPressures.Add(averagePressure);
            temps.Add(currentTemp);
        }
    }
}
