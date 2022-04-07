using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class SimController : MonoBehaviour
{
    public void CreateParticle()
    {
        GameObject newPrefab = GameObject.Instantiate(particlePrefab);
        Particle newParticle = newPrefab.GetComponent<Particle>();
        particles.Add(newParticle);

        //tell them about us
        newParticle.myController = this;
        newParticle.Initialize();

        //random position (uniform)
        //newPrefab.transform.position = new Vector3(Random.Range(boundsMin.x * 0.9f, boundsMax.x * 0.9f), 0, Random.Range(boundsMin.z * 0.9f, boundsMax.z * 0.9f));
        newPrefab.transform.position = new Vector3(0, 0, 0);

        //random starting velocity (gaussian and based off equipartition formula for thermal equilibrium)
        RandomGaussian.SetSigma(Mathf.Sqrt(1.38f * Mathf.Pow(10f, -23f) * Temp() / newParticle.Mass()));
        newParticle.velocity = new Vector3(RandomGaussian.Generate(),0, RandomGaussian.Generate());
        //newParticle.velocity = new Vector3(5, 0, 2);
        newParticle.speed = newParticle.velocity.magnitude;
        //Debug.Log("Speed: " + newParticle.speed);
        //Debug.Log("Velocity: " + newParticle.velocity);

        //activate it
        newParticle.ToggleRunning();
    }
}
