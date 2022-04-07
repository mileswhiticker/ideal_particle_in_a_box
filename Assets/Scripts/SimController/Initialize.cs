using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SimController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Initialize()
    {
        //time to traverse box bounds
        tau = Mathf.Min(boundsMax.x - boundsMin.x, boundsMax.y - boundsMin.y) / Mathf.Min(defaultVelocity.x, defaultVelocity.z);
        timeMax = tau * 20;
        timeStep = tau / 500;
        stepMax = Mathf.RoundToInt(timeMax / timeStep);

        RandomGaussian.SetSigma(0.2f * (boundsMax.x - boundsMin.x));

        //create starting particles
        int particlesLeft = startingParticles;
        while (particlesLeft > 0)
        {
            particlesLeft--;
            CreateParticle();
        }

        //create starting box bounds
        GameObject newWallGameobject;
        float wallWidth = 0.01f;
        float wallHeight = 0.1f;

        //negative x direction
        newWallGameobject = GameObject.Instantiate(wallPrefab);
        newWallGameobject.transform.position = new Vector3(boundsMin.x, 0, 0);
        newWallGameobject.transform.localScale = new Vector3(wallWidth, wallHeight, boundsMax.y - boundsMin.y);

        //positive x direction
        newWallGameobject = GameObject.Instantiate(wallPrefab);
        newWallGameobject.transform.position = new Vector3(boundsMax.x, 0, 0);
        newWallGameobject.transform.localScale = new Vector3(wallWidth, wallHeight, boundsMax.y - boundsMin.y);

        //negative z direction
        newWallGameobject = GameObject.Instantiate(wallPrefab);
        newWallGameobject.transform.position = new Vector3(0, 0, boundsMin.x);
        newWallGameobject.transform.localScale = new Vector3(boundsMax.y - boundsMin.y, wallHeight, wallWidth);

        //positive z direction
        newWallGameobject = GameObject.Instantiate(wallPrefab);
        newWallGameobject.transform.position = new Vector3(0, 0, boundsMax.x);
        newWallGameobject.transform.localScale = new Vector3(boundsMax.y - boundsMin.y, wallHeight, wallWidth);
    }
}
