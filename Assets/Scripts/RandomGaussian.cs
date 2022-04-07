using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * random gaussian algorithm sourced from VoxusSoftware https://github.com/VoxusSoftware/unity-random/blob/master/Assets/Voxus/Random/RandomGaussian.cs
 */

public static class RandomGaussian
{
    private static float seed = 1.0f;
    private static float sigma = 1.0f;
    private static float mu = 0.0f;
    public static void SetSeed(float aSeed)
    {
        seed = aSeed;
    }
    public static void SetSigma(float aSigma)
    {
        sigma = aSigma;
    }
    public static void SetMu(float aMu)
    {
        mu = aMu;
    }
    public static float Generate()
    {
        float x1, x2, w, y1, y2;

        do
        {
            x1 = Random.Range(-1f, 1f);
            x2 = Random.Range(-1f, 1f);
            w = x1 * x1 + x2 * x2;
        } while (w >= 1f);

        w = Mathf.Sqrt((-2f * Mathf.Log(w)) / w);
        y1 = x1 * w;
        y2 = x2 * w;

        return (y1 * sigma) + mu;
    }
}