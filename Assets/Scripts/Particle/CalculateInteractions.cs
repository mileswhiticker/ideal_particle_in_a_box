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
            if (myController.tLeftLogData <= 0)
            {
                logVelocities = true;
                myController.tLeftLogData = myController.logDataInterval;
                sqrvelocities = new List<float>();
                myController.squaredVelocities.Add(sqrvelocities);
                //Log.AddLine("" + myController.currentTemp + ",...");
                //Log.AddLine("" + myController.averagePressure + ",...");
            }

            //calculate net forces from all other particles
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
                //Debug.Log(this.name + " force:" + force);
                //if (doDebug) Debug.Log("force:" + force);

                //calculate the acceleration
                float accelMagnitude = force / Mass();

                //work out the acceleration vector
                //tan(theta) = opp/adj
                //:. theta = atan(opp/adj)
                //float theta = Mathf.Atan(posDelta.x / posDelta.z);
                //use Atan2 to automatically convert between different quadrants
                //float angle = (float)Math.Atan2(posDelta.x, posDelta.z);
                //floatText.text = "" + angle;

                //cos(theta) = adj/hyp
                //:. adj = hyp*cos(theta)
                //:. opp = hyp*sin(theta)
                //Vector3 accel = new Vector3(-accelMagnitude * Mathf.Sin(angle), 0, -accelMagnitude * Mathf.Cos(angle));
                Vector3 accel = posDelta.normalized * accelMagnitude * -1;
                //if (doDebug) Debug.Log("accelMagnitude:" + accelMagnitude + " Mathf.Cos(angle):" + Mathf.Cos(angle) + " Mathf.Sin(angle):" + Mathf.Sin(angle));
                //if (doDebug) Debug.Log(-accelMagnitude * Mathf.Sin(angle));

                //have unity handle the spatial transformations for us
                //this.transform.LookAt(otherParticle.transform.position);
                //a positive force is away from the other particle
                //Vector3 accel = transform.forward * accelMagnitude * -1;
                //Debug.DrawLine(transform.position, transform.position + transform.rotation.eulerAngles, Color.green, 0.01f);

                //if (myController.DoSimulate3D())
                {
                    //treat the y dimension as a single separate axis
                    //not sure if this will work
                    //distSquared = posDelta.y * posDelta.y;
                    //force = 1 / (10 * distSquared * distSquared) - 1 / (10 * distSquared);
                    //accel.y = force / Mass();
                    //Debug.Log("accel.y:" + accel.y);
                }

                //if (doDebug) Debug.Log("accelMagnitude:" + accelMagnitude + " force:" + force + " distSquared:" + distSquared + " posDelta:" + posDelta + " accel2:" + accel);

                interactionAcceleration += accel;
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
