using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    public void BumpLeft(Particle aParticle)
    {
        //Debug.Log("BumpLeft()");
        totalImpulseLeft -= 2 * aParticle.velocity.x * aParticle.Mass();
        leftText.text = "Impulse: " + totalImpulseLeft;
    }
    public void BumpRight(Particle aParticle)
    {
        //Debug.Log("BumpRight()");
        totalImpulseRight += 2 * aParticle.velocity.x * aParticle.Mass();
        rightText.text = "Impulse: " + totalImpulseRight;
    }
    public void BumpTop(Particle aParticle)
    {
        //Debug.Log("BumpTop()");
        totalImpulseTop += 2 * aParticle.velocity.z * aParticle.Mass();
        topText.text = "Impulse: " + totalImpulseTop;
    }
    public void BumpBottom(Particle aParticle)
    {
        //Debug.Log("BumpBottom()");
        totalImpulseBottom -= 2 * aParticle.velocity.z * aParticle.Mass();
        bottomText.text = "Impulse: " + totalImpulseBottom;
    }
    public float totalImpulseLeft = 0;
    public Text leftText;
    public float totalImpulseRight = 0;
    public Text rightText;
    public float totalImpulseTop = 0;
    public Text topText;
    public float totalImpulseBottom = 0;
    public Text bottomText;
}
