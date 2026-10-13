using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationGridDisplay : MonoBehaviour
{
    [Header("Grid Settings")]
    // Top–left corner of the grid overlay (adjust as needed)
    public Vector3 gridOrigin = new Vector3(-5, 5, 0);
    // Spacing between grid cells
    public Vector2 cellSpacing = new Vector2(2, 2);

    [Header("Animation Settings")]
    // Duration for the overlay appearance and disappearance animations
    public float animationDuration = 0.5f;
    // How long the overlay remains visible before fading out
    public float overlayDuration = 5f;

    [Header("Optional Overlay Container")]
    // (Optional) Parent transform for the overlay organisms so that they are rendered on top
    public Transform overlayParent;

    /// <summary>
    /// Creates and displays overlay copies of organisms based on their stored Organism data.
    /// These copies are arranged in a grid and appear on top of your running simulation.
    /// </summary>
    /// <param name="organisms">List of OrganismScript representing the current generation.</param>
    public void DisplayOverlay(List<OrganismScript> organisms)
    {
        StartCoroutine(DisplayOverlayCoroutine(organisms));
    }

    IEnumerator DisplayOverlayCoroutine(List<OrganismScript> organisms)
    {
        int count = organisms.Count;
        // Calculate a roughly square grid layout.
        int columns = Mathf.CeilToInt(Mathf.Sqrt(count));
        int rows = Mathf.CeilToInt((float)count / columns);

        // Precompute grid cell target positions.
        Vector3[] targetPositions = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            int col = i % columns;
            int row = i / columns;
            targetPositions[i] = gridOrigin + new Vector3(col * cellSpacing.x, -row * cellSpacing.y, 0);
        }

        // List to track overlay copies.
        List<GameObject> overlayOrganisms = new List<GameObject>();

        // For each organism, create an overlay copy.
        for (int i = 0; i < count; i++)
        {
            // Retrieve the organism's stored data.
            Organism orgData = organisms[i].organism;

            // Instantiate a new organism using your base prefab.
            GameObject newOrgGO = Instantiate(OrganismGenerator.instance.baseOrganism);
            // Optionally parent the overlay copy under a designated container.
            if (overlayParent != null)
            {
                newOrgGO.transform.SetParent(overlayParent, true);
            }

            OrganismScript newOrgScript = newOrgGO.GetComponent<OrganismScript>();
            newOrgScript.SpawnOrganism(orgData);

            // Wait one frame to ensure all child objects are created.
            yield return null;

            // Calculate the offset needed to align the organism's central node to the grid cell.
            Vector3 centralPos = newOrgScript.centralObj.transform.position;
            Vector3 offset = targetPositions[i] - centralPos;
            foreach (Transform child in newOrgGO.transform)
            {
                child.position += offset;
            }

            // Animate the organism scaling from zero to full size.
            newOrgGO.transform.localScale = Vector3.zero;
            float timer = 0f;
            while (timer < animationDuration)
            {
                timer += Time.deltaTime;
                newOrgGO.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / animationDuration);
                //yield return null;
            }
            newOrgGO.transform.localScale = Vector3.one;

            overlayOrganisms.Add(newOrgGO);
        }

        // The overlay organisms remain visible for a set duration while the simulation runs in the background.
        yield return new WaitForSeconds(overlayDuration);

        // Fade out and remove the overlay copies.
        foreach (GameObject overlayOrg in overlayOrganisms)
        {
            if (overlayOrg != null)
            {
                float timer = 0f;
                Vector3 originalScale = overlayOrg.transform.localScale;
                while (timer < animationDuration)
                {
                    timer += Time.deltaTime;
                    overlayOrg.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, timer / animationDuration);
                    yield return null;
                }
                Destroy(overlayOrg);
            }
        }
    }
}
