using UnityEngine;

public class InteractionsManager : MonoBehaviour
{
    OrganismGenerator organismGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            OrganismScript org = hit.collider.transform.parent.GetComponent<OrganismScript>();
            for (int i = 0; i < org.organism.oldOrganisms.Count; i++)
            {
                organismGenerator.SpawnOrganismAtPosition(Vector2.zero, org.organism.oldOrganisms[i].ToOrganism());
            }
        }
    }
}
