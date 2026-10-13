using UnityEngine;

public class MuscleScript : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Muscle muscle;
    public Organism organism;
    public Rigidbody2D rb1;
    public Rigidbody2D rb2;
    public SpringJoint2D spring;

    public bool contracted;

    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spring = rb1.gameObject.AddComponent<SpringJoint2D>();
        spring.autoConfigureDistance = false;
        spring.connectedBody = rb2;
        spring.dampingRatio = 0.3f;
    }
    private void Update()
    {
        lineRenderer.SetPosition(0, rb1.transform.position);
        lineRenderer.SetPosition(1, rb2.transform.position);
        //float distance = Vector2.Distance(rb1.transform.position, rb2.transform.position);
        //lineRenderer.startWidth = OrganismGenerator.instance.muscleLineDistanceWidthCurve.Evaluate(Mathf.Clamp01(Mathf.Abs(distance - spring.distance) / 2));
        //lineRenderer.endWidth = lineRenderer.startWidth;
        if (rb1.bodyType == RigidbodyType2D.Dynamic)
        {
            timer += Time.deltaTime;
            if (timer > muscle.cycleLength)
            {
                timer = 0;
            }
            if ((muscle.cycleOffset < timer && timer < muscle.cycleLength * muscle.cyclecontractedPercent + muscle.cycleOffset) || (muscle.cycleOffset - muscle.cycleLength < timer && timer < muscle.cycleLength * muscle.cyclecontractedPercent + muscle.cycleOffset - muscle.cycleLength))
            { // if in range of designated range or one below  in desmos: \ x-\operatorname{floor}\left(x\right),   0\left(a\right)+b<y<\left[0,1\right]\left(a\right)+b,   0\left(a\right)+b-1<y<\left[0,1\right]\left(a\right)+b-1
                contracted = true;
                spring.distance = muscle.lengthContracted;
            }
            else
            {
                contracted = false;
                spring.distance = muscle.lengthFull;
            }
        }
    }
}
