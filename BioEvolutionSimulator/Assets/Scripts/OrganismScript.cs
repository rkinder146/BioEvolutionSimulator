using TMPro;
using UnityEngine;

public class OrganismScript : MonoBehaviour
{
    public Organism organism;
    public GameObject[] otherObjs;
    public GameObject centralObj;
    public GameObject[] muscleObjs;
    public float distance = 0f;

    public TextMeshPro textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        textMeshPro.rectTransform.position = centralObj.transform.position;
        CalculateDistance();
        if (distance.ToString().Length > 4)
        {
            textMeshPro.text = distance.ToString().Substring(0, 3);
        }
    }

    public void SpawnOrganism(Organism o)
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        organism = o;
        // Spawn Objs
        GameObject cObj;
        GameObject[] oObjs = new GameObject[o.nodes.Length];
        GameObject[] mObjs = new GameObject[o.muscles.Length];
        for (int i = 0; i < o.nodes.Length; i++)
        {
            oObjs[i] = Instantiate(OrganismGenerator.instance.node, o.nodes[i].relPosition, Quaternion.identity, transform);
            NodeScript ns = oObjs[i].GetComponent<NodeScript>();
            ns.node = o.nodes[i];
            ns.organism = o;
        }
        cObj = oObjs[0];
        for (int i = 0; i < mObjs.Length; i++)
        {
            mObjs[i] = Instantiate(OrganismGenerator.instance.muscle, o.nodes[o.muscles[i].node1].relPosition, Quaternion.identity, oObjs[o.muscles[i].node1].transform);
            MuscleScript ms = mObjs[i].GetComponent<MuscleScript>();
            ms.rb1 = oObjs[o.muscles[i].node1].GetComponent<Rigidbody2D>();
            ms.rb2 = oObjs[o.muscles[i].node2].GetComponent<Rigidbody2D>();
            ms.muscle = o.muscles[i];
            ms.organism = o;
        }
        otherObjs = oObjs;
        centralObj = cObj;
        muscleObjs = mObjs;
    }

    [ContextMenu("CalculateDistance")]
    public float CalculateDistance()
    {
        float minDist = 1000f;
        float avgDist = 0f;
        foreach (var o in otherObjs)
        {
            avgDist += o.transform.position.x;
            if (minDist > o.transform.position.x)
            {
                minDist = o.transform.position.x;
            }
        }
        avgDist /= otherObjs.Length;
        //distance = (minDist+avgDist)/2;
        distance = avgDist;
        return distance;
    }

    public void ActivateRB()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
