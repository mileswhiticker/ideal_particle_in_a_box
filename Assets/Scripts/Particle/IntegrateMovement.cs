using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Particle : MonoBehaviour
{
    public Vector3 interactionAcceleration = new Vector3(0,0,0);
    public Vector3 extraVelocity = new Vector3(0,0,0);
    protected void IntegrateMovement()
    {
        float deltaTime = myController.SimDeltaTime();

        //*** velocity verlet integration method ***//

        //gravity
        interactionAcceleration.y -= 1 / Mass();

        //cosmic rays
        interactionAcceleration += extraVelocity;
        extraVelocity = new Vector3(0, 0, 0);       //reset this for next tick

        //first 
        //x(t + dt) = x(t) + v(t)dt + 0.5 a(t) (dt)^2
        //Debug.Log("interactionAcceleration:" + interactionAcceleration);
        float deltaX = velocity.x * deltaTime + interactionAcceleration.x * deltaTime * deltaTime;
        float deltaY = velocity.y * deltaTime + interactionAcceleration.y * deltaTime * deltaTime;
        float deltaZ = velocity.z * deltaTime + interactionAcceleration.z * deltaTime * deltaTime;
        //if (doDebug) Debug.Log("deltaX,deltaZ:" + deltaZ + "," + deltaX);
        //if (doDebug) Debug.Log("velocity:" + velocity);

        transform.position = new Vector3(
            transform.position.x + deltaX,
            transform.position.y + deltaY,
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
            midStepVelocity.y + 0.5f * interactionAcceleration.y * deltaTime,
            midStepVelocity.z + 0.5f * interactionAcceleration.z * deltaTime);

        //if (doDebug) Debug.Log("velocity:" + velocity);

        BoundaryIntersections();

        myController.netSquaredVelocity += velocity.sqrMagnitude;
        myController.numContributingSqrVelocity++;

        //a heat dissipation effect to gradually cool the simulation
        //doing x and z separately is crude but fast to calculate
        velocity *= 0.995f;
    }
}