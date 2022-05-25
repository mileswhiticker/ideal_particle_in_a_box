using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public SimController myController;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public static float DefaultMass()
    {
        return 1;//return Mathf.Pow(10f, -24f);
    }
    private float mass = 1;
    public float Mass()
    {
        return mass;
    }
    public float speed = 0;
    bool isRunning = false;
    public void Initialize()
    {
        mass = DefaultMass();
    }
    public void ToggleRunning()
    {
        isRunning = !isRunning;
    }
    float tLeftStep = 0.5f;
    public bool doDebug = false;
    void Update()
    {
        if (isRunning && myController != null && myController.isSimRunning)
        {
            //reduce the step update rate
            tLeftStep -= Time.deltaTime;
            if(tLeftStep <= 0)
            {
                tLeftStep = 0;// 0.5f;
                float deltaTime = myController.SimDeltaTime();

                //are we doing particle interactions?
                if(myController.DoInteractions())
                {
                    //calculate net forces from all other particles
                    Vector3 interactionEffect = new Vector3(0,0,0);
                    foreach(Particle otherParticle in myController.particles)
                    {
                        if(otherParticle == this)
                        {
                            //dont interact with yourself, you'll go blind
                            continue;
                        }

                        //calculate the difference in position
                        //a positive value is a repulsive force therefore calculate the posdelta from other -> this
                        Vector3 posDelta = otherParticle.transform.position - this.transform.position;
                        if (doDebug) Debug.Log("posDelta:" + posDelta);
                        if (posDelta.x == 0 && posDelta.y == 0 && posDelta.z == 0)
                        {
                            //no interaction effect if we are on top of each other
                            continue;
                        }

                        //calculate the force due to interaction
                        //use potential 1/r^4 - 1/r^2
                        float distSquared = posDelta.x * posDelta.x + posDelta.z * posDelta.z;
                        if (doDebug) Debug.Log("distSquared:" + distSquared);
                        float force = 1 / (distSquared * distSquared) - 1 / (distSquared);
                        if (doDebug) Debug.Log("force:" + force);

                        //calculate the acceleration
                        float accelMagnitude = deltaTime * force / Mass();
                        if (doDebug) Debug.Log("accelMagnitude:" + accelMagnitude);

                        //work out the acceleration vector

                        //tan(theta) = opp/adj
                        //:. theta = atan(opp/adj)
                        float theta = Mathf.Atan(posDelta.x / posDelta.z);
                        if (doDebug) Debug.Log("theta:" + theta);

                        //cos(theta) = adj/hyp
                        //:. adj = hyp*cos(theta)
                        //:. opp = hyp*sin(theta)
                        Vector3 accel = new Vector3(accelMagnitude * Mathf.Cos(theta), 0, accelMagnitude * Mathf.Sin(theta));
                        if (doDebug) Debug.Log("accel:" + accel);
                        interactionEffect += accel;
                    }

                    //some interactions will cancel each other out
                    //if(doDebug) Debug.Log("interactionEffect:" + interactionEffect);
                    velocity += interactionEffect;
                }

                bool outOfBounds = false;
                transform.position += velocity * deltaTime;

                //a heat dissipation effect to gradually cool the simulation
                //velocity *= 0.995f;
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
    }
    public float GetMomentum()
    {
        return speed * mass;
    }
    bool OutOfBoundsLeft()
    {
        return transform.position.x < myController.BoundsMin().x;
    }
    bool OutOfBoundsRight()
    {
        return transform.position.x > myController.BoundsMax().x;
    }
    bool OutOfBoundsTop()
    {
        return transform.position.z > myController.BoundsMax().z;
    }
    bool OutOfBoundsBottom()
    {
        return transform.position.z < myController.BoundsMin().z;
    }
}
