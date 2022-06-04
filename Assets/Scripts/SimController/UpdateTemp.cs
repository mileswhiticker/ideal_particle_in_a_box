using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    private void UpdateTemp()
    {
        float volume = (boundsMax.x - boundsMin.x) * (boundsMax.z - boundsMin.z);
        //float gasConstant = 8.314472f;
        //float avoNumber = 6 * Mathf.Pow(10, 23);
        //float molarMass = volume / particles.Count;
        //float boltzmannConstant = 1.38f * Mathf.Pow(10, -23);
        float modifiedBoltzmann = 1.38f*Mathf.Pow(10,4);

        //ideal gas
        //PV = nRT
        //T = PV / nR
        //currentTemp = (averagePressure * volum)) / (particles.Count * gasConstant);

        //van der waals gas
        //float attractiveConstant = 0.001f;
        //float repulsiveConstant = -124f;//avoNumber * particles.Count * 16 * Mathf.PI * Particle.Radius() * Particle.Radius() * Particle.Radius() / 3;
        //currentTemp = ((averagePressure + (attractiveConstant * particles.Count * particles.Count) / (volume * volume)) * (volume - particles.Count * repulsiveConstant)) / (particles.Count * gasConstant);

        //equipartition formula
        //U = (3/2)NkT
        //T = (2/3)U/Nk
        float netKinetic = 0.5f * Particle.DefaultMass() * averageVelocitySqr;
        //Debug.Log(averageVelocitySqr);
        currentTemp = (2 * netKinetic) / (3 * modifiedBoltzmann);

        temp.text = "Temperature: " + currentTemp;
        particleVelocity.text = "Average squared velocity: " + averageVelocitySqr;
        doAvgVelocityUpdate = true;
        averageVelocitySqr = 0;
    }
}
