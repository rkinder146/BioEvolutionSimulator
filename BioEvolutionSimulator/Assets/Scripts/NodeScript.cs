using UnityEngine;
using UnityEngine.UIElements;

public class NodeScript : MonoBehaviour
{
    public Organism organism;
    public Node node;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().color = OrganismGenerator.instance.nodeFrictionColors.Evaluate(Mathf.Clamp01((node.friction - OrganismGenerator.instance.nodeFrictionMin) / (OrganismGenerator.instance.nodeFrictionMax - OrganismGenerator.instance.nodeFrictionMin)));
        float newScale = OrganismGenerator.instance.nodeMassSizeCurve.Evaluate(Mathf.Clamp01((node.mass - OrganismGenerator.instance.nodeMassMin) / (OrganismGenerator.instance.nodeMassMax - OrganismGenerator.instance.nodeMassMin)));
        //transform.localScale = new Vector3(newScale, newScale, 1);
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.mass = organism.centralNode.mass;
        rb.sharedMaterial = new PhysicsMaterial2D() { friction = organism.centralNode.friction };
        if (transform.position.y < transform.localScale.y / 2)
        {
            //Debug.Log(transform.position.y + "     " + transform.localScale.y / 2 +"     "+ transform.position + Vector3.up * (transform.localScale.y / 2 - transform.position.y));
            transform.position += Vector3.up * (transform.localScale.y / 2 - transform.position.y);
        }
    }
}
