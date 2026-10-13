using UnityEngine;

public class PhysicsSimulation : MonoBehaviour
{
    void Start()
    {
        // Set the 2D physics simulation mode to manual.
        Physics2D.simulationMode = SimulationMode2D.Script;
    }

    void Update()
    {
        // Calculate the total simulation time for this frame.
        float simulationTime = Time.timeScale * Time.deltaTime;

        // Process the simulation in fixed steps.
        while (simulationTime > 0f)
        {
            // Determine the step duration.
            float step = Mathf.Min(Time.fixedDeltaTime, simulationTime);

            // Advance the physics simulation by the step.
            Physics2D.Simulate(step);

            // Subtract the processed time.
            simulationTime -= step;
        }
    }
}
