using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SimController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Initialize()
    {
        isSimRunning = false;
        logCreated = false;

        boundsMax.x = curLength / 2;
        boundsMax.z = curLength / 2;
        boundsMin.x = -curLength / 2;
        boundsMin.z = -curLength / 2;

        //reset values
        RandomGaussian.SetSigma(0.2f * (boundsMax.x - boundsMin.x));
        simTime = 0;
        particleMass.text = "Particle mass: " + Particle.DefaultMass();
        temp.text = "Temperature: " + Temp();

        //clear old particles
        while (particles.Count > 0)
        {
            Particle curParticle = particles[0];
            particles.RemoveAt(0);
            Object.Destroy(curParticle.gameObject);
        }

        //create starting particles
        int particlesLeft = startingParticles;
        while (particlesLeft > 0)
        {
            particlesLeft--;
            CreateParticle();
            particleVelocity.text = "Particle velocity: " + particles[0].velocity;
        }

        //clear old walls
        while(walls.Count > 0)
        {
            GameObject curWall = walls[0];
            Object.Destroy(curWall);
            walls.RemoveAt(0);
        }

        //create starting box bounds
        GameObject newWallGameobject;
        float wallWidth = 0.01f;
        float wallHeight = 0.1f;

        //negative x direction
        newWallGameobject = GameObject.Instantiate(wallPrefab);
        newWallGameobject.transform.position = new Vector3(boundsMin.x, 0, 0);
        newWallGameobject.transform.localScale = new Vector3(wallWidth, wallHeight, boundsMax.z - boundsMin.z);
        walls.Add(newWallGameobject);

        //positive x direction
        newWallGameobject = GameObject.Instantiate(wallPrefab);
        newWallGameobject.transform.position = new Vector3(boundsMax.x, 0, 0);
        newWallGameobject.transform.localScale = new Vector3(wallWidth, wallHeight, boundsMax.z - boundsMin.z);
        walls.Add(newWallGameobject);

        //negative z direction
        newWallGameobject = GameObject.Instantiate(wallPrefab);
        newWallGameobject.transform.position = new Vector3(0, 0, boundsMin.z);
        newWallGameobject.transform.localScale = new Vector3(boundsMax.x - boundsMin.x, wallHeight, wallWidth);
        walls.Add(newWallGameobject);

        //positive z direction
        newWallGameobject = GameObject.Instantiate(wallPrefab);
        newWallGameobject.transform.position = new Vector3(0, 0, boundsMax.z);
        newWallGameobject.transform.localScale = new Vector3(boundsMax.x - boundsMin.x, wallHeight, wallWidth);
        walls.Add(newWallGameobject);
    }
}
