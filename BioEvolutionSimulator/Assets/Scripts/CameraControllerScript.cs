using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
    public float deltaSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals) & Camera.main.orthographicSize > deltaSize)
        {
            Camera.main.orthographicSize -= deltaSize;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Camera.main.orthographicSize += deltaSize;
        }
    }
}
