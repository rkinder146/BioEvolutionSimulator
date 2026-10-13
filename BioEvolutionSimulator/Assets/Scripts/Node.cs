using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2 relPosition; //position 
    public float friction;
    public float mass;
    public Node()
    {
        relPosition = new Vector2();
        friction = 0f;
        mass = 1f;
    }

    public Node(Node other)
    {
        relPosition = other.relPosition;
        friction = other.friction;
        mass = other.mass;
    }
}
