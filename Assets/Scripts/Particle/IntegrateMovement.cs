using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Particle : MonoBehaviour
{
    protected Vector3 interactionAcceleration = new Vector3();
    protected void IntegrateMovement()
    {
        float deltaTime = myController.SimDeltaTime();

        //transform.position += velocity * deltaTime;

        //*** velocity verlet integration method ***//

        //first 
        //x(t + dt) = x(t) + v(t)dt + 0.5 a(t) (dt)^2
        float deltaX = velocity.x * deltaTime + interactionAcceleration.x * deltaTime * deltaTime;
        float deltaZ = velocity.z * deltaTime + interactionAcceleration.z * deltaTime * deltaTime;
        //if (doDebug) Debug.Log("deltaX,deltaZ:" + deltaZ + "," + deltaX);
        //if (doDebug) Debug.Log("velocity:" + velocity);

        transform.position = new Vector3(
            transform.position.x + deltaX,
            transform.position.y,
            transform.position.z + deltaZ);

        //second
        //v(t + 0.5dt) = v(t) + 0.5a(t)dt
        Vector3 midStepVelocity = velocity + 0.5f * interactionAcceleration * deltaTime;

        //third
        //calculate the new acceleration
        CalculateInteractions();

        //fourth
        //v(t + dt) = v(t + 0.5dt) + 0.5a(t + dt)dt
        velocity.Set(midStepVelocity.x + 0.5f * interactionAcceleration.x * deltaTime,
            midStepVelocity.y,
            midStepVelocity.z + 0.5f * interactionAcceleration.z * deltaTime);

        //if (doDebug) Debug.Log("velocity:" + velocity);

        //BoundaryIntersections();

        //a heat dissipation effect to gradually cool the simulation
        //doing x and z separately is crude but fast to calculate
        velocity *= 0.99f;
    }
}
