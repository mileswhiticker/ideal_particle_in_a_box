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
        interactionAcceleration = new Vector3();
        if (myController.DoInteractions())
        {
            bool logVelocities = false;
            List<float> velocities = null;
            if (myController.tLeftLogData <= 0)
            {
                logVelocities = true;
                myController.tLeftLogData = myController.logDataInterval;
                velocities = new List<float>();
                myController.squaredVelocities.Add(velocities);
                //Log.AddLine("" + myController.currentTemp + ",...");
                Log.AddLine("" + myController.averagePressure + ",...");
            }

            //calculate net forces from all other particles
            foreach (Particle otherParticle in myController.particles)
            {
                if (myController.doAvgVelocityUpdate)
                {
                    myController.averageVelocitySqr += otherParticle.velocity.sqrMagnitude;
                }
                if(logVelocities)
                {
                    velocities.Add(otherParticle.velocity.sqrMagnitude);
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

                //for optimisation: only calculate nearby particles
                if(distSquared > 1)
                {
                    //continue;
                }

                //if (doDebug) Debug.Log("distSquared:" + distSquared);
                float force = 1 / (10 * distSquared * distSquared) - 1 / (10 * distSquared);
                if(force > 1)
                {
                    force = 1;
                }
                //Debug.Log(this.name + " force:" + force);
                //if (doDebug) Debug.Log("force:" + force);

                //calculate the acceleration
                float accelMagnitude = force / Mass();
                //if (doDebug) Debug.Log("accelMagnitude:" + accelMagnitude);

                //work out the acceleration vector

                //tan(theta) = opp/adj
                //:. theta = atan(opp/adj)
                //float theta = Mathf.Atan(posDelta.x / posDelta.z);
                float angle = (float)Math.Atan2(posDelta.x, posDelta.z);
                floatText.text = "" + angle;
                //if (doDebug) Debug.Log("angle:" + angle);
                //Debug.Log(this.name + " theta:" + angle);

                //cos(theta) = adj/hyp
                //:. adj = hyp*cos(theta)
                //:. opp = hyp*sin(theta)
                Vector3 accel = new Vector3(-accelMagnitude * Mathf.Sin(angle), 0, -accelMagnitude * Mathf.Cos(angle));
                //if (doDebug) Debug.Log("accel:" + accel);
                interactionAcceleration += accel;
            }

            if (myController.doAvgVelocityUpdate)
            {
                //Debug.Log("calc avg vel sqr");
                myController.averageVelocitySqr /= myController.particles.Count;
                myController.doAvgVelocityUpdate = false;
            }

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

            //Debug.DrawLine(transform.position, transform.position + new Vector3(1, 0, 1), Color.green, 0.01f);
            //Debug.DrawLine(transform.position, transform.position + new Vector3(1, 0, 0), Color.red, 0.01f);
            //display acceleration direction
            /*
            Vector3 accelNormed = interactionAcceleration;
            if(interactionAcceleration.sqrMagnitude > 0)
            {
                accelNormed.Normalize();
                Debug.DrawLine(transform.position, transform.position + accelNormed * 0.2f, Color.green, 0.01f);
            }
            */
        }
    }
}
