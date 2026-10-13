
using System.Collections.Generic;
using UnityEngine.Timeline;

[System.Serializable]
public class Organism
{
    public Node centralNode;
    public Node[] nodes;
    public Muscle[] muscles;
    public List<OldOrganism> oldOrganisms = new List<OldOrganism>();

    public OldOrganism ToOldOrganism()
    {
        OldOrganism o = new OldOrganism();
        o.centralNode = centralNode;
        o.nodes = nodes;
        o.muscles = muscles;
        return o;
    }
}

[System.Serializable]
public class OldOrganism
{
    public Node centralNode;
    public Node[] nodes;
    public Muscle[] muscles;

    public Organism ToOrganism()
    {
        Organism o = new Organism();
        o.centralNode = centralNode;
        o.nodes = nodes;
        o.muscles = muscles;
        return o;
    }
}
