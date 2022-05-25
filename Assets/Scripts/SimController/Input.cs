using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class SimController : MonoBehaviour
{
    private float tLeftKeyDown = 0.0f;
    private void PollInput()
    {
        if(tLeftKeyDown > 0)
        {
            tLeftKeyDown -= Time.deltaTime;
            return;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            Particle.stepDelay += Time.deltaTime;
            tLeftKeyDown = 0.2f;
            Debug.LogWarning("Particle.stepDelay increased to " + Particle.stepDelay);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            Particle.stepDelay -= Time.deltaTime;
            if(Particle.stepDelay < 0)
            {
                Particle.stepDelay = 0;
            }
            Debug.LogWarning("Particle.stepDelay lowered to " + Particle.stepDelay);
            tLeftKeyDown = 0.2f;
        }
    }
}
