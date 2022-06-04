using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public partial class SimController : MonoBehaviour
{
    float tLeftRay = 1.0f;
    float rayInterval = 0f;
    public List<GameObject> oldObjects = new List<GameObject>();
    public void UpdateCosmicRays()
    {
        for(int index = 0; index < oldObjects.Count; index++)
        {
            GameObject go = oldObjects[index];
            Object.Destroy(go);
        }
        oldObjects = new List<GameObject>();

        //1 sec cooldown
        tLeftRay -= Time.deltaTime;
        if (tLeftRay > 0)
        {
            return;
        }
        tLeftRay = rayInterval;

        for(int numRays = 0; numRays < 6; numRays++)
        {
            EmitCosmicRay();
        }
    }
    public void EmitCosmicRay()
    {
        GameObject ray = new GameObject();
        oldObjects.Add(ray);
        LineRenderer line = ray.AddComponent<LineRenderer>();
        Vector3 startPos = new Vector3(Random.Range(-curLength/2, curLength/2), curLength, Random.Range(-curLength/2, curLength/2));
        //Vector3 targetPos = new Vector3(Random.Range(-curLength/2, curLength/2), -curLength / 2, Random.Range(-curLength/2, curLength/2));
        Vector3 targetPos = new Vector3(startPos.x, -curLength / 2, startPos.z);
        numRaysThisTick++;

        int layermask = 1 << 6;
        RaycastHit hitinfo = new RaycastHit();
        if(Physics.Raycast(startPos, targetPos, out hitinfo, 1000, layermask))
        {
            //work out incident angle
            //

            //add velocity in the direction of the ray
            //todo: compton scattering
            Particle hitParticle =  hitinfo.collider.gameObject.GetComponent<Particle>();
            hitParticle.extraVelocity += (targetPos - startPos) * 100;

            //where was the hit?
            targetPos = new Vector3(hitinfo.point.x, hitinfo.point.y, hitinfo.point.z);

            //create a visual effect
            GameObject explosion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            explosion.transform.localScale = new Vector3(1, 1, 1);
            explosion.transform.position = targetPos;
            explosion.GetComponent<Renderer>().material = RedMaterial;
            oldObjects.Add(explosion);

            //increment the counter
            numRayCollissionsThisTick++;
        }

        line.SetPosition(0, startPos);
        line.SetPosition(1, targetPos);
        //line.startColor = Color.yellow;
        line.material = YellowMaterial;
        line.startWidth = 0.01f;
    }
}

/*
public class Keep_dist : MonoBehaviour
{
    public GameObject eye;
    public GameObject Pikachu;
    public Vector3 a;
    public Vector3 b;
    public Transform obj1_s;
    public Transform obj1_e;
    public LineRenderer line1;
    public Transform obj2_s;
    public Transform obj2_e;
    public LineRenderer line2;
    public Transform obj3_s;
    public Transform obj3_e;
    public LineRenderer line3; // Start is called before the first frame update
    void Start()
    {
        line1 = eye.GetComponent();
        line2 = eye.GetComponent();
        line3 = eye.GetComponent();
    }
}
*/