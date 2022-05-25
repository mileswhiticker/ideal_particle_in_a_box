using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Particle : MonoBehaviour
{
    public static float DefaultMass()
    {
        return 1;//return Mathf.Pow(10f, -24f);
    }
    private float mass = 1;
    public float Mass()
    {
        return mass;
    }
    public void ToggleRunning()
    {
        isRunning = !isRunning;
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
