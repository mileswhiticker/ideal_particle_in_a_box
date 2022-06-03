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
        boundsMax.y = curLength / 2;
        boundsMax.z = curLength / 2;
        boundsMin.x = -curLength / 2;
        boundsMin.y = -curLength / 2;
        boundsMin.z = -curLength / 2;
        ground.transform.position = new Vector3(0, boundsMin.y, 0);

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
        float gridDims;
        if(DoSimulate3D())
        { 
            gridDims = Mathf.Pow(startingParticles, 1f / 3f);
        }
        else
        {
            gridDims = Mathf.Pow(startingParticles, 1f / 2f);
        }

        if(gridDims != Mathf.Round(gridDims))
        {
            gridDims = Mathf.Round(gridDims);
        }

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
                for (int k = 0; k < gridDims; k++)
                {
                    //none left
                    if (particlesLeft <= 0)
                    {
                        float numNeeded = gridDims * gridDims;
                        if (DoSimulate3D())
                        {
                            numNeeded *= gridDims;
                        }
                        Debug.LogWarning("Insufficient particles to neatly distribute over grid with cell width " + gridDims + ", have " + startingParticles + "/" + numNeeded);
                        particlesLeft--;
                        break;
                    }

                    //create the next one
                    Vector3 gridPos = new Vector3(i * cellWidth - curLength / 2 + cellWidth / 2, k, j * cellWidth - curLength / 2 + cellWidth / 2);

                    particlesLeft--;
                    Particle curParticle = CreateParticle(gridPos);
                    curParticle.gameObject.name = "Particle #" + (startingParticles - particlesLeft);
                    squaredVelocitiesHorizontal += curParticle.velocity.x * curParticle.velocity.x;
                    //Debug.Log(curParticle.velocity.x * curParticle.velocity.x);

                    //only do the bottom layer if we are not doing 3D
                    if (!DoSimulate3D())
                    {
                        break;
                    }
                }
                if (particlesLeft < 0)
                {
                    break;
                }
            }
            //none left
            if (particlesLeft < 0)
            {
                break;
            }
        }

        //debug text for one of the particles
        particles[0].doDebug = true;
        particles[0].gameObject.GetComponent<Renderer>().material = GreenMaterial;

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

        sidelength.text = "Side dimensions: " + curLength;
        volume.text = "Volume: " + curLength * curLength * curLength;
    }
}
