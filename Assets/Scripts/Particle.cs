using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public SimController myController;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public static float DefaultMass()
    {
        return Mathf.Pow(10f, -26f);
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
    float tLeftStep = 0f;
    void Update()
    {
        if (isRunning && myController != null && myController.isSimRunning)
        {
            //reduce the step update rate
            tLeftStep -= Time.deltaTime;
            if(tLeftStep <= 0)
            {
                tLeftStep = Time.deltaTime;
                //what is the change?
                float deltaTime = myController.SimDeltaTime();
                Vector3 posDelta = velocity * deltaTime;
                bool outOfBounds = false;
                transform.position += posDelta;
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
