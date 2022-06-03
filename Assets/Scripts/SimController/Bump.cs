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
    public void BumpAbove(Particle aParticle)
    {
        //Debug.Log("BumpLeft()");
        totalImpulseAbove += 2 * aParticle.velocity.y * aParticle.Mass();
        aboveImpulseText.text = "Impulse: " + totalImpulseAbove;
    }
    public void BumpBelow(Particle aParticle)
    {
        //Debug.Log("BumpRight()");
        totalImpulseBelow -= 2 * aParticle.velocity.y * aParticle.Mass();
        belowImpulseText.text = "Impulse: " + totalImpulseBelow;
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
}
