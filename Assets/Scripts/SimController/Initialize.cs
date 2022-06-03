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
        //RandomGaussian.SetSigma(0.2f * (boundsMax.x - boundsMin.x));
        //random starting velocity (gaussian and based off equipartition formula for thermal equilibrium)
        //RandomGaussian.SetSigma(Mathf.Sqrt(1.38f * Mathf.Pow(10f, -23f) * Temp() / Particle.DefaultMass()));
        RandomGaussian.SetSigma(Mathf.Sqrt(startingTemp / Particle.DefaultMass()));
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
        float squaredVelocitiesHorizontal = 0;
        int particlesLeft = startingParticles;

        //layout particles on a grid to avoid exploding from interaction forces
        float gridDims = Mathf.Sqrt(startingParticles);
        gridDims = Mathf.Ceil(gridDims);
        if (gridDims < 2)
        {
            gridDims = 2;
        }
        float cellWidth = curLength / gridDims;
        //Debug.Log("gridDims:" + gridDims + ", cellWidth:" + cellWidth);

        //RandomGaussian.SetSigma(0.01f);

        for (int i = 0; i < gridDims; i++)
        {
            for (int j = 0; j < gridDims; j++)
            {
                //create the next one
                Vector3 gridPos = new Vector3(i * cellWidth - curLength/2 + cellWidth/2, 0, j * cellWidth - curLength/2 + cellWidth/2);

                particlesLeft--;
                Particle curParticle = CreateParticle(gridPos);
                curParticle.gameObject.name = "Particle #" + (startingParticles - particlesLeft);
                squaredVelocitiesHorizontal += curParticle.velocity.x * curParticle.velocity.x;
                //Debug.Log(curParticle.velocity.x * curParticle.velocity.x);

                //none left
                if (particlesLeft <= 0)
                {
                    break;
                }
            }
            //none left
            if (particlesLeft <= 0)
            {
                break;
            }
        }

        //debug text for one of the particles
        particles[0].doDebug = true;
        particles[0].gameObject.GetComponent<Renderer>().material = RedMaterial;

        float rmsHorizontal = Mathf.Sqrt(squaredVelocitiesHorizontal) / (float)startingParticles;
        //Debug.Log("RMS: "+ rmsHorizontal);
        //particleVelocity.text = "RMS horiz velocity: " + rmsHorizontal;
        squaredMeanVelocityHorizontal.Add(rmsHorizontal);
        particleMass.text = "Particle mass: " + Particle.DefaultMass();

        //clear old walls
        while (walls.Count > 0)
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

        sidelength.text = "Side length: " + curLength;
        volume.text = "Volume length: " + curLength * curLength;
    }
}
