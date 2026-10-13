using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif
public class GenerationsManager : MonoBehaviour
{
    public bool autoMode;
    public int autoGenCap;
    public static int numResets;

    public List<OrganismScript> generationOrganisms = new List<OrganismScript>();
    public int generationNum = 1;
    public int generationSize;
    public float generationTimeLength;
    [HideInInspector]
    public float generationTime;
    public float[] generationDistances;
    public UIDataManager UIdataManager;

    [Range(0.05f, 1f)]
    public static float percentSurvive;

    [Range(0f, 1f)]
    public float manualPercentSurvive;

    public TMPro.TextMeshProUGUI buttonText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (manualPercentSurvive != 0f && !autoMode)
        {
            percentSurvive = manualPercentSurvive;
        }
        if (percentSurvive == 0f || (numResets == 0 && autoMode))
        {
            percentSurvive = 1f;
        }
        if (Mathf.RoundToInt(generationSize % (1 / percentSurvive)) != 0)
        {
            int addtonextmultiple = Mathf.RoundToInt((1 / percentSurvive) - generationSize % (1 / percentSurvive));
            Debug.Log("generation size changed from: " + generationSize + " to: " + (generationSize + Mathf.RoundToInt((1 / percentSurvive) - generationSize % (1 / percentSurvive))));
            generationSize += Mathf.RoundToInt((1 / percentSurvive) - generationSize % (1 / percentSurvive));
        }
        buttonText.text = "Save " + percentSurvive;
        StartInitialGeneration();
    }
    
    void StartInitialGeneration()
    {
        for (int i = 0; i < generationSize; i++)
        {
            OrganismGenerator.instance.GenerateOrganism();
        }
        foreach (var item in FindObjectsByType<OrganismScript>(FindObjectsSortMode.None))
        {
            generationOrganisms.Add(item);
        }
        StartCoroutine(startinasec());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Time.timeScale *= 2f;
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket)) 
        { 
            Time.timeScale /= 2f; 
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        if(generationTime > generationTimeLength)
        {
            if (gameObject.GetComponent<GenerationGridDisplay>().isActiveAndEnabled) { gameObject.GetComponent<GenerationGridDisplay>().DisplayOverlay(generationOrganisms); }
            generationNum++;
            generationDistances = new float[generationSize];
            generationTime = 0;
            for (int i = 0;i < generationOrganisms.Count; i++)
            {
                generationDistances[i] = generationOrganisms[i].CalculateDistance();
            }

            Array.Sort(generationDistances);
            generationOrganisms.Sort((a, b) => (int)(a.distance - b.distance));

            UIdataManager.AddGenerationData(generationNum, generationDistances);


            if (generationNum >= autoGenCap && autoMode)
            {
                UIdataManager.SaveData();
                StopGame();
                Debug.Log("resetit");
                switch (numResets)
                {
                    case 4:
                        StopGame();
                        break;
                    case 3:
                        percentSurvive = 0.10f;
                        break;
                    case 2:
                        percentSurvive = 0.25f;
                        break;
                    case 1:
                        percentSurvive = 0.5f;
                        break;
                    case 0:
                        percentSurvive = 0.75f;
                        break;
                }
                numResets++;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);

            }

            for (int i = 0; i < (generationSize * (1 - percentSurvive)); i++)
            {
                var gO = generationOrganisms[0].gameObject;
                generationOrganisms.RemoveAt(0);
                Destroy(gO);

                //Destroy(generationOrganisms[i].gameObject);
            }
            int nspawned = 0;
            // fix this regenerator
            Debug.Log("generationOrganisms.Count: " + generationOrganisms.Count);
            //int maxCount = generationSize;
            //for (int i = 0; i < generationOrganisms.Count; i++)
            //{
            //    for (int j = 0; j < (1f/percentSurvive); j++)
            //    {
            //        if (nspawned > maxCount) { break; }
            //        generationOrganisms.Add(OrganismGenerator.instance.GenerateOrganism(generationOrganisms[0].organism).GetComponent<OrganismScript>());
            //        generationOrganisms[generationOrganisms.Count - 1].organism.oldOrganisms.Add(generationOrganisms[0].organism.ToOldOrganism());
            //        nspawned++;
            //    }
            //    Destroy(generationOrganisms[0].gameObject);
            //    generationOrganisms.RemoveAt(0);
            //}
            int maxCount = generationSize-1;
            for (int i = generationOrganisms.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < (1f / percentSurvive); j++)
                {
                    if (nspawned > maxCount) { break; }
                    generationOrganisms.Add(OrganismGenerator.instance.GenerateOrganism(generationOrganisms[i].organism).GetComponent<OrganismScript>());
                    generationOrganisms[generationOrganisms.Count - 1].organism.oldOrganisms.Add(generationOrganisms[i].organism.ToOldOrganism());
                    nspawned++;
                }
                Destroy(generationOrganisms[i].gameObject);
                generationOrganisms.RemoveAt(i);
            }

            StartCoroutine(startinasec());
        }
        else
        {
            generationTime += Time.deltaTime;
        }
    }

    public IEnumerator startinasec()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        float ts = Time.timeScale;
        Time.timeScale = 0.5f;
        foreach (var org in generationOrganisms)
        {
            org.ActivateRB();
        }
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = ts;
    }
    void StopGame()
    {
#if UNITY_EDITOR
    EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
