using Unity.VisualScripting;
using UnityEngine;

public class OrganismGenerator : MonoBehaviour
{
    public int defaultNodes;
    [Space]
    [Range(0f, 1f)]
    public float chanceForNewNode;
    [Range(0f, 1f)]
    public float chanceForLessNode;
    [Space]
    [Space]
    public float nodeFrictionMax;
    public float nodeFrictionMin;
    public float nodeFrictionMutationMax;
    public Gradient nodeFrictionColors;
    [Space]
    public float nodeMassMax;
    public float nodeMassMin;
    public float nodeMassMutationMax;
    public AnimationCurve nodeMassSizeCurve;
    [Space]
    public Vector2 nodeOffset;//  -x -> x, 0 -> y   idk why i did this
    public Vector2 nodeOffsetMutationMax;
    [Space]
    [Space]
    public float muscleStrenghtMax;
    public float muscleStrenghtMin;
    public float muscleStrengthMutationMax;
    [Space]
    public float muscleFullLengthMax;
    public float muscleFullLengthMin;
    public float muscleFullLengthMutationMax;
    [Space]
    public float musclelengthContractedMax;
    public float muscleLengthContractedMin;
    public float muscleLengthContractedMutationMax;
    [Space]
    public AnimationCurve muscleLineDistanceWidthCurve;
    [Space]
    public float muscleCycleLengthMax;
    public float muscleCycleLengthMin;
    public float muscleCycleLengthMutationMax;
    [Space]
    public float muscleCycleContractedPercentMax;
    public float muscleCycleContractedPercentMin;
    public float muscleCycleContractedPercentMutationMax;
    [Space]
    public float muscleCycleOffsetMax;
    public float muscleCycleOffsetMin;
    public float muscleCycleOffsetMutationMax;
    [Space]
    [Space]
    public GameObject baseOrganism;
    public GameObject node;
    public GameObject muscle;
    [Space]
    public GenerationsManager generationManager;
    public static OrganismGenerator instance;


    public int spawned;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("this");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("destroy");
        }
    }

    private void Start()
    {

    }

    public GameObject GenerateOrganism()
    {
        Organism o = new Organism();
        // Calc # of nodes
        int numOfNodes = defaultNodes;
        if (Random.Range(0f, 1f) > chanceForNewNode) { numOfNodes++; }
        if (Random.Range(0f, 1f) > chanceForLessNode) { numOfNodes--; }
        o.nodes = new Node[numOfNodes];
        // Calc # of muscles
        o.muscles = new Muscle[numOfNodes * (numOfNodes - 1) / 2];
        // Generate Other Nodes
        Node centralNode = new Node();
        centralNode.friction = Random.Range(nodeFrictionMin, nodeFrictionMax);
        centralNode.mass = Random.Range(nodeMassMin, nodeMassMax);
        centralNode.relPosition = Vector2.zero;
        o.centralNode = centralNode;
        o.nodes[0] = centralNode;
        // Generate Other Nodes
        for (int i = 1; i < numOfNodes; i++)
        {
            Node n = new Node();
            n.friction = Random.Range(nodeFrictionMin, nodeFrictionMax);
            n.mass = Random.Range(nodeMassMin, nodeMassMax);
            n.relPosition.x = Random.Range(-nodeOffset.x, nodeOffset.x);
            n.relPosition.y = Random.Range(0, nodeOffset.y);
            o.nodes[i] = n;
        }
        // Generate Muscles
        int k = 0; // muscleindex idk theres prob a better way
        for (int i = 0; i < numOfNodes; i++)
        {
            for (int j = i + 1; j < numOfNodes; j++)
            {
                Muscle m = new Muscle()
                {
                    node1 = i,
                    node2 = j,
                    lengthFull = Random.Range(muscleFullLengthMin, muscleFullLengthMax),
                    lengthContracted = Random.Range(muscleLengthContractedMin, musclelengthContractedMax),
                    cycleLength = Random.Range(muscleCycleLengthMin, muscleCycleLengthMax),
                    cyclecontractedPercent = Random.Range(muscleLengthContractedMin, muscleCycleContractedPercentMax),
                    cycleOffset = Random.Range(muscleCycleOffsetMin, muscleCycleOffsetMax),
                    strength = Random.Range(muscleStrenghtMin, muscleStrenghtMax)
                };
                o.muscles[k] = m;
                k++;
            }
        }
        // Spawn
        spawned++; //TESTTTTTTTTTTTTTTT
        var go = Instantiate(baseOrganism);
        go.GetComponent<OrganismScript>().SpawnOrganism(o);
        go.name += spawned;
        // Return
        return go;
    }

    /*
    public GameObject GenerateOrganism(Organism baseO)
    {
        //Organism org = new Organism();
        //org = baseO;
        //int numOfNodes = org.nodes.Length;
        //if (Random.Range(0f, 1f) > chanceForNewNode) { numOfNodes++; }
        //if (Random.Range(0f, 1f) > chanceForLessNode && numOfNodes > 2) { numOfNodes--; }
        //org.nodes = new Node[numOfNodes];
        //for (int i = 0; i < numOfNodes; i++)
        //{
        //    org.nodes[i] = baseO.nodes[i];
        //}
        Organism o = new Organism();
        o = baseO.CloneViaSerialization();
        int numOfNodes = baseO.nodes.Length; // Use baseO's nodes length
        if (Random.Range(0f, 1f) > chanceForNewNode) { numOfNodes++; }
        if (Random.Range(0f, 1f) > chanceForLessNode && numOfNodes > 2) { numOfNodes--; }
        o.nodes = new Node[numOfNodes];
        for (int i = 0; i < Mathf.Min(numOfNodes, baseO.nodes.Length); i++)
        {
            o.nodes[i] = new Node(baseO.nodes[i]);
        }
        if (numOfNodes > baseO.nodes.Length)
        {
            Node n = new Node();
            n.friction = Random.Range(nodeFrictionMin, nodeFrictionMax);
            n.mass = Random.Range(nodeMassMin, nodeMassMax);
            n.relPosition.x = Random.Range(-nodeOffset.x, nodeOffset.x);
            n.relPosition.y = Random.Range(0, nodeOffset.y);
            o.nodes[numOfNodes - 1] = n;

            //fix muscles
            o.muscles = new Muscle[numOfNodes * (numOfNodes - 1) / 2];
            for (int i = 0; i < Mathf.Min(numOfNodes, baseO.muscles.Length); i++)
            {
                o.muscles[i] = baseO.muscles[i];
            }
            for (int j = 0; j < numOfNodes-1; j++)
            {
                Muscle m = new Muscle()
                {
                    node1 = numOfNodes - 1,
                    node2 = j,
                    lengthFull = Random.Range(muscleFullLengthMin, muscleFullLengthMax),
                    lengthContracted = Random.Range(muscleLengthContractedMin, musclelengthContractedMax),
                    cycleLength = Random.Range(muscleCycleLengthMin, muscleCycleLengthMax),
                    cyclecontractedPercent = Random.Range(muscleLengthContractedMin, muscleCycleContractedPercentMax),
                    cycleOffset = Random.Range(muscleCycleOffsetMin, muscleCycleOffsetMax),
                    strength = Random.Range(muscleStrenghtMin, muscleStrenghtMax)
                };
                o.muscles[baseO.muscles.Length+j] = m;
            }
        }


        //shift values
        for (int i = 0; i < o.nodes.Length; i++)
        {
            o.nodes[i].friction += Random.Range(-nodeFrictionMutationMax, nodeFrictionMutationMax);
            o.nodes[i].mass += Random.Range(-nodeMassMutationMax, nodeMassMutationMax);
            o.nodes[i].relPosition += new Vector2(Random.Range(-nodeOffsetMutationMax.x, nodeOffsetMutationMax.x), Random.Range(-nodeOffsetMutationMax.y, nodeOffsetMutationMax.y));
        }
        for (int i = 0; i < o.muscles.Length; i++)
        {
            o.muscles[i].lengthContracted += Random.Range(-muscleLengthContractedMutationMax, muscleLengthContractedMutationMax);
            o.muscles[i].lengthFull += Random.Range(-muscleFullLengthMutationMax, muscleFullLengthMutationMax);
            o.muscles[i].cycleLength += Random.Range(-muscleCycleLengthMutationMax, muscleCycleLengthMutationMax);
            o.muscles[i].cyclecontractedPercent += Random.Range(-muscleCycleContractedPercentMutationMax, muscleCycleContractedPercentMutationMax);
            o.muscles[i].cycleOffset += Random.Range(-muscleCycleOffsetMutationMax, muscleCycleOffsetMutationMax);
            o.muscles[i].strength += Random.Range(-muscleStrengthMutationMax, muscleStrengthMutationMax);
        }
        o.centralNode = o.nodes[0];
        spawned++; //TESTTTTTTTTTTTTTTT
        var go = Instantiate(baseOrganism);
        go.GetComponent<OrganismScript>().SpawnOrganism(o);
        go.name += spawned; 
        return go;
    }
    */
    public GameObject GenerateOrganism(Organism baseO)
    {
        // Clone the base organism
        Organism o = baseO.CloneViaSerialization();
        int originalNodeCount = baseO.nodes.Length;
        int numOfNodes = originalNodeCount;

        // Adjust the number of nodes (add or remove one)
        if (RandomTriangular(0f, 1f) > chanceForNewNode && numOfNodes < 5)
        {
            numOfNodes++;
            //Debug.Log("hit " + generationManager.generationNum);
        }
        if (RandomTriangular(0f, 1f) > chanceForLessNode && numOfNodes > 3)
        {
            numOfNodes--;
            //Debug.Log("loss " + generationManager.generationNum);
        }

        //if(numOfNodes == originalNodeCount) { Debug.Log("nada " + generationManager.generationNum); }

        // Rebuild the nodes array
        Node[] newNodes = new Node[numOfNodes];
        int nodesToCopy = Mathf.Min(numOfNodes, originalNodeCount);
        // Copy over nodes that exist in baseO
        for (int i = 0; i < nodesToCopy; i++)
        {
            newNodes[i] = new Node(baseO.nodes[i]);
        }
        // If we have added new nodes, generate them
        for (int i = nodesToCopy; i < numOfNodes; i++)
        {
            Node n = new Node();
            n.friction = RandomTriangular(nodeFrictionMin, nodeFrictionMax);
            n.mass = RandomTriangular(nodeMassMin, nodeMassMax);
            n.relPosition = new Vector2(
                RandomTriangular(-nodeOffset.x, nodeOffset.x),
                RandomTriangular(0, nodeOffset.y)
            );
            newNodes[i] = n;
        }
        o.nodes = newNodes;

        // Rebuild the muscles array with the correct number of muscles
        int newMuscleCount = numOfNodes * (numOfNodes - 1) / 2;
        Muscle[] newMuscles = new Muscle[newMuscleCount];
        int k = 0;
        for (int i = 0; i < numOfNodes; i++)
        {
            for (int j = i + 1; j < numOfNodes; j++)
            {
                // If both nodes existed in the original, copy the muscle from baseO
                if (i < originalNodeCount && j < originalNodeCount)
                {
                    // Calculate the corresponding index in baseO.muscles using the same ordering as when they were generated
                    int originalIndex = GetMuscleIndex(i, j, originalNodeCount);
                    if (originalIndex < baseO.muscles.Length)
                    {
                        // Clone the muscle (assuming a copy constructor exists)
                        newMuscles[k] = baseO.muscles[originalIndex];
                    }
                    else
                    {
                        // Fall back to generating a new random muscle if something is off
                        newMuscles[k] = GenerateRandomMuscle(i, j);
                    }
                }
                else
                {
                    // For any new connection (involving a new node), generate a new random muscle.
                    newMuscles[k] = GenerateRandomMuscle(i, j);
                }
                k++;
            }
        }
        o.muscles = newMuscles;

        // Apply mutations to nodes
        for (int i = 0; i < o.nodes.Length; i++)
        {
            o.nodes[i].friction += RandomTriangular(-nodeFrictionMutationMax, nodeFrictionMutationMax);
            o.nodes[i].mass += RandomTriangular(-nodeMassMutationMax, nodeMassMutationMax);
            o.nodes[i].relPosition += new Vector2(
                RandomTriangular(-nodeOffsetMutationMax.x, nodeOffsetMutationMax.x),
                RandomTriangular(-nodeOffsetMutationMax.y, nodeOffsetMutationMax.y)
            );
        }
        // Apply mutations to muscles
        for (int i = 0; i < o.muscles.Length; i++)
        {
            o.muscles[i].lengthContracted += RandomTriangular(-muscleLengthContractedMutationMax, muscleLengthContractedMutationMax);
            o.muscles[i].lengthFull += RandomTriangular(-muscleFullLengthMutationMax, muscleFullLengthMutationMax);
            o.muscles[i].cycleLength += RandomTriangular(-muscleCycleLengthMutationMax, muscleCycleLengthMutationMax);
            o.muscles[i].cyclecontractedPercent += RandomTriangular(-muscleCycleContractedPercentMutationMax, muscleCycleContractedPercentMutationMax);
            o.muscles[i].cycleOffset += RandomTriangular(-muscleCycleOffsetMutationMax, muscleCycleOffsetMutationMax);
            o.muscles[i].strength += RandomTriangular(-muscleStrengthMutationMax, muscleStrengthMutationMax);
        }

        o.centralNode = o.nodes[0];
        o = ClampOrgValues(o);
        spawned++;
        var go = Instantiate(baseOrganism);
        go.GetComponent<OrganismScript>().SpawnOrganism(o);
        go.name += spawned;
        return go;
    }

    // Helper: Given a pair (i, j) (with i < j) and the original node count, calculate the index 
    // of the corresponding muscle in the array (which was built in order by nested loops).
    private int GetMuscleIndex(int i, int j, int nodeCount)
    {
        // This formula sums up the number of muscles for all nodes before i,
        // then adds the offset for j.
        return i * nodeCount - (i * (i + 1)) / 2 + (j - i - 1);
    }

    // Helper: Generate a new random muscle for the connection between nodes i and j.
    private Muscle GenerateRandomMuscle(int i, int j)
    {
        return new Muscle()
        {
            node1 = i,
            node2 = j,
            lengthFull = Random.Range(muscleFullLengthMin, muscleFullLengthMax),
            lengthContracted = Random.Range(muscleLengthContractedMin, musclelengthContractedMax),
            cycleLength = Random.Range(muscleCycleLengthMin, muscleCycleLengthMax),
            // (Check if the lower bound for cyclecontractedPercent should be muscleCycleContractedPercentMin instead!)
            cyclecontractedPercent = Random.Range(muscleLengthContractedMin, muscleCycleContractedPercentMax),
            cycleOffset = Random.Range(muscleCycleOffsetMin, muscleCycleOffsetMax),
            strength = Random.Range(muscleStrenghtMin, muscleStrenghtMax)
        };
    }

    public Organism ClampOrgValues(Organism org)
    {
        Organism o = org;

        for (int i = 0; i < o.nodes.Length; i++)
        {
            o.nodes[i].friction = Mathf.Clamp(o.nodes[i].friction, nodeFrictionMin, nodeFrictionMax);
            o.nodes[i].mass = Mathf.Clamp(o.nodes[i].mass, nodeMassMin, nodeMassMax);
            o.nodes[i].relPosition = new Vector2(Mathf.Clamp(o.nodes[i].relPosition.x, -nodeOffset.x, nodeOffset.x), Mathf.Clamp(o.nodes[i].relPosition.y, -nodeOffset.y, nodeOffset.y));
        }
        o.centralNode = o.nodes[0];
        for (int i = 0; i < o.muscles.Length; i++)
        {
            o.muscles[i].lengthFull = Mathf.Clamp(o.muscles[i].lengthFull, muscleFullLengthMin, muscleFullLengthMax);
            o.muscles[i].strength = Mathf.Clamp(o.muscles[i].strength, muscleStrenghtMin, muscleStrenghtMax);
            o.muscles[i].cyclecontractedPercent = Mathf.Clamp(o.muscles[i].cyclecontractedPercent, muscleCycleContractedPercentMin, muscleCycleContractedPercentMax);
            o.muscles[i].cycleLength = Mathf.Clamp(o.muscles[i].cycleLength, muscleCycleLengthMin, muscleCycleLengthMax);
            o.muscles[i].cycleOffset = Mathf.Clamp(o.muscles[i].cycleOffset, muscleCycleOffsetMin, muscleCycleOffsetMax);
            o.muscles[i].lengthContracted = Mathf.Clamp(o.muscles[i].lengthContracted, muscleLengthContractedMin, musclelengthContractedMax);
        }

        return o;
    }

    public GameObject SpawnOrganismAtPosition(Vector3 pos, Organism o)
    {
        var go = Instantiate(baseOrganism, pos, Quaternion.identity);
        go.GetComponent<OrganismScript>().SpawnOrganism(o);
        go.name += spawned;
        return go;
    }

    public static float RandomTriangular(float min, float max)
    {
        // Generate a value with a triangular distribution centered around 0.5.
        // Random.value returns a uniform float in [0, 1].
        float r = Random.value - Random.value;  // r is in the range [-1, 1] with a peak at 0.
        r = (r + 1) / 2; // Transform r to be in [0, 1] with a peak at 0.5.
        return Mathf.Lerp(min, max, r);
    }

}
