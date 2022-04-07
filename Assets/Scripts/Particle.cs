using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public SimController myController;
    public Vector3 velocity = new Vector3(0, 0, 0);
    private float mass = 1;
    public float Mass()
    {
        return mass;
    }
    public float speed = 0;
    bool isRunning = false;
    public void Initialize()
    {
        mass = Mathf.Pow(10f, -21f);
    }
    public void ToggleRunning()
    {
        isRunning = !isRunning;
    }

    void Update()
    {
        if (isRunning && myController != null && myController.isSimRunning)
        {
            //what is the change?
            float modifiers = Time.deltaTime * myController.TimeRate();
            Vector3 posDelta = velocity * modifiers;
            bool doContinue = false;
            transform.position += posDelta;
            do
            {
                if (OutOfBoundsLeft())
                {
                    doContinue = true;
                    float extra = myController.BoundsMin().x - transform.position.x;
                    transform.position = new Vector3(transform.position.x + extra, transform.position.y, transform.position.z);
                    myController.BumpLeft(this);
                    velocity.x = -velocity.x;
                }
                if (OutOfBoundsRight())
                {
                    doContinue = true;
                    float extra = transform.position.x - myController.BoundsMax().x;
                    transform.position = new Vector3(transform.position.x - extra, transform.position.y, transform.position.z);
                    myController.BumpRight(this);
                    velocity.x = -velocity.x;
                }
                if (OutOfBoundsTop())
                {
                    doContinue = true;
                    float extra = transform.position.z - myController.BoundsMax().z;
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - extra);
                    myController.BumpTop(this);
                    velocity.z = -velocity.z;
                }
                if (OutOfBoundsBottom())
                {
                    doContinue = true;
                    float extra = myController.BoundsMin().z - transform.position.z;
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + extra);
                    myController.BumpBottom(this);
                    velocity.z = -velocity.z;
                }
            }
            while (false);
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
