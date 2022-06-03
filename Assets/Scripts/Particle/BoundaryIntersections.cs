using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Particle : MonoBehaviour
{
    protected void BoundaryIntersections()
    {
        bool outOfBounds = false;
        do
        {
            outOfBounds = false;

            //x axis
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

            //y axis
            if (myController.BoundsMin().y > transform.position.y)
            {
                //out of bounds below
                outOfBounds = true;
                float extra = myController.BoundsMin().y - transform.position.y;
                transform.position = new Vector3(transform.position.x, myController.BoundsMin().y + extra, transform.position.z);
                myController.BumpBelow(this);
                velocity.y = -velocity.y;
            }
            if (myController.BoundsMax().y < transform.position.y)
            {
                //out of bounds above
                outOfBounds = true;
                float extra = transform.position.y - myController.BoundsMax().y;
                transform.position = new Vector3(transform.position.x, myController.BoundsMax().y - extra, transform.position.z);
                myController.BumpAbove(this);
                velocity.y = -velocity.y;
            }

            //z axis
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
