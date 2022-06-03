using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class SimController : MonoBehaviour
{
    public Particle CreateParticle(Vector3 aForcedPos)
    {
        Particle particle = CreateParticle();
        particle.transform.position = aForcedPos;
        return particle;
    }
    public Particle CreateParticle()
    {
        GameObject newPrefab = GameObject.Instantiate(particlePrefab);
        Particle newParticle = newPrefab.GetComponent<Particle>();
        particles.Add(newParticle);

        //tell them about us
        newParticle.myController = this;
        newParticle.Initialize();

        //random position (uniform)
        //newPrefab.transform.position = new Vector3(Random.Range(boundsMin.x * 0.9f, boundsMax.x * 0.9f), 0, Random.Range(boundsMin.z * 0.9f, boundsMax.z * 0.9f));
        //newPrefab.transform.position = new Vector3(0, 0, 0);

        Vector3 startingVelocity = new Vector3(RandomGaussian.Generate(), 0, RandomGaussian.Generate());
        if(DoSimulate3D())
        {
            startingVelocity.y = RandomGaussian.Generate();
            //Debug.Log("startingVelocity:" + startingVelocity);
        }
        newParticle.velocity = startingVelocity;
        //newParticle.velocity = new Vector3(0, 0, 0);
        newParticle.speed = newParticle.velocity.magnitude;
        newParticle.mass = Particle.DefaultMass();
        //Debug.Log("Speed: " + newParticle.speed);
        //Debug.Log("Velocity: " + newParticle.velocity);

        //activate it
        newParticle.ToggleRunning();
        return newParticle;
    }
}
