using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Particle : MonoBehaviour
{
    protected void CalculateInteractions()
    {
        interactionAcceleration = new Vector3(0,0,0);
        //if (doDebug) Debug.Log("1 interactionAcceleration:" + interactionAcceleration);
        if (myController.DoInteractions())
        {
            bool logVelocities = false;
            List<float> sqrvelocities = null;
            if (myController.TLeftLogData() <= 0)
            {
                logVelocities = true;
                sqrvelocities = new List<float>();
                myController.squaredVelocities.Add(sqrvelocities);
                //Log.AddLine("" + myController.currentTemp + ",...");
                //Log.AddLine("" + myController.averagePressure + ",...");
            }

            //calculate net forces from all other particles
            float nearbyDistSqr = 0;
            float numContributing = 0;
            foreach (Particle otherParticle in myController.particles)
            {
                if(logVelocities)
                {
                    sqrvelocities.Add(otherParticle.velocity.sqrMagnitude);
                }
                if(myController.doAvgVelocityUpdate)
                {
                    myController.averageVelocitySqr += otherParticle.velocity.sqrMagnitude;
                }
                if (otherParticle == this)
                {
                    //dont interact with yourself, you'll go blind
                    continue;
                }

                //calculate the difference in position
                //a positive value is a repulsive force therefore calculate the posdelta from other -> this
                Vector3 posDelta = otherParticle.transform.position - this.transform.position;
                //if (doDebug) Debug.Log("posDelta:" + posDelta);
                //Debug.Log(this.name + " posDelta:" + posDelta);
                /*if (posDelta.x == 0 && posDelta.y == 0 && posDelta.z == 0)
                {
                    //no interaction effect if we are on top of each other
                    continue;
                }*/

                //calculate the force due to interaction
                //use coulomb potential 1/r^4 - 1/r^2
                float distSquared = posDelta.x * posDelta.x + posDelta.z * posDelta.z;
                if(myController.DoSimulate3D())
                {
                    distSquared += posDelta.y * posDelta.y;
                }

                //for optimisation: only calculate nearby particles
                if (distSquared > 1)
                {
                    //continue;
                }

                //if (doDebug) Debug.Log("distSquared:" + distSquared);
                float force = 0;
                if (distSquared != 0)
                {
                    force = 1 / (10 * distSquared * distSquared) - 1 / (10 * distSquared);
                }
                if(force > 1)
                {
                    force = 1;
                }

                //calculate the acceleration
                float accelMagnitude = force / Mass();

                Vector3 accel = posDelta.normalized * accelMagnitude * -1;

                interactionAcceleration += accel;

                //calculate the distance of the nearest particles
                if(distSquared <= 1.5)
                {
                    nearbyDistSqr += distSquared;
                    numContributing++;
                }
            }

            //what is our average distance?
            if(numContributing > 0)
            {
                nearbyDistSqr /= numContributing;
                myController.currentParticleDistSqr += nearbyDistSqr;
                myController.numParticlesContributingToDistSqr++;
            }

            //if (doDebug) Debug.Log("2 interactionAcceleration:" + interactionAcceleration);
            //Debug.DrawLine(transform.position, transform.position + interactionAcceleration, Color.green, interactionAcceleration.magnitude);

            //doing these here is a hack but it's an optimisation
            //should only run once per update loop due to the boolean check doAvgVelocityUpdate
            if (logVelocities)
            {
                float standardDeviation = 0;
                foreach (Particle otherParticle in myController.particles)
                {
                    standardDeviation += Mathf.Pow(otherParticle.velocity.sqrMagnitude - myController.averageVelocitySqr, 2);
                }
                standardDeviation /= myController.particles.Count - 1;
                standardDeviation = Mathf.Sqrt(standardDeviation);
                myController.sqrVelocityError.Add(standardDeviation);
            }

            //Debug.Log("3 interactionAcceleration:" + interactionAcceleration);

            //Debug.DrawLine(transform.position, transform.position + new Vector3(1, 0, 1), Color.green, 0.01f);
            //Debug.DrawLine(transform.position, transform.position + new Vector3(1, 0, 0), Color.red, 0.01f);
            //display acceleration direction
            Vector3 accelNormed = interactionAcceleration;
            if(interactionAcceleration.sqrMagnitude > 0)
            {
                accelNormed.Normalize();
                //Debug.DrawLine(transform.position, transform.position + transform.rotation.eulerAngles, Color.green, 0.01f);
            }

            //Debug.Log("4 interactionAcceleration:" + interactionAcceleration);
        }
    }
}
