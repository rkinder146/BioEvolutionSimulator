using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrganismManager : MonoBehaviour
{
    public List<Organism> organisms = new List<Organism>();
    private GenerationsManager generationManager;
    private List<GameObject> spawnedOrganisms = new List<GameObject>();

    public GameObject baseOrganism;
    public GameObject node;
    public GameObject muscle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generationManager = FindFirstObjectByType<GenerationsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (organisms.Count == 0)
        {
            for (int i = 0; i < generationManager.generationOrganisms.Count; i++)
            {
                organisms.Add(generationManager.generationOrganisms[i].organism);
            }
        }
    }

    public void SpawnOrganism(Organism o, Vector3 pos)
    {
        var baseGo = Instantiate(baseOrganism);
        //organism = o;
        // Spawn Objs
        GameObject cObj;
        GameObject[] oObjs = new GameObject[o.nodes.Length];
        GameObject[] mObjs = new GameObject[o.muscles.Length];
        for (int i = 0; i < o.nodes.Length; i++)
        {
            oObjs[i] = Instantiate(OrganismGenerator.instance.node, o.nodes[i].relPosition, Quaternion.identity, baseGo.transform);
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
        //otherObjs = oObjs;
        //centralObj = cObj;
        //muscleObjs = mObjs;
        baseGo.transform.position = pos;
    }
}
