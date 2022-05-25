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

        bool outOfBounds = false;
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

        //a heat dissipation effect to gradually cool the simulation
        //doing x and z separately is crude but fast to calculate
        //velocity.x *= 0.995f;
        //velocity.z *= 0.995f;
        do
        {
            outOfBounds = false;
            if (myController.BoundsMin().x > transform.position.x)
            {
                //out of bounds left
                outOfBounds = true;
                float extra = myController.BoundsMin().x - transform.position.x;
                transform.position = new Vector3(myController.BoundsMin().x + extra, transform.position.y, transform.position.z);
                myController.BumpLeft(this);
                velocity.x = -velocity.x;
            }
            if (myController.BoundsMax().x < transform.position.x)
            {
                //out of bounds right
                outOfBounds = true;
                float extra = transform.position.x - myController.BoundsMax().x;
                transform.position = new Vector3(myController.BoundsMax().x - extra, transform.position.y, transform.position.z);
                myController.BumpRight(this);
                velocity.x = -velocity.x;
            }
            if (myController.BoundsMax().z < transform.position.z)
            {
                //out of bounds top
                outOfBounds = true;
                float extra = transform.position.z - myController.BoundsMax().z;
                transform.position = new Vector3(transform.position.x, transform.position.y, myController.BoundsMax().z - extra);
                myController.BumpTop(this);
                velocity.z = -velocity.z;
            }
            if (myController.BoundsMin().z > transform.position.z)
            {
                //out of bounds bottom
                outOfBounds = true;
                float extra = myController.BoundsMin().z - transform.position.z;
                transform.position = new Vector3(transform.position.x, transform.position.y, myController.BoundsMin().z + extra);
                myController.BumpBottom(this);
                velocity.z = -velocity.z;
            }
        }
        while (outOfBounds);
    }
}
