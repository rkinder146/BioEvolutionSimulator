using UnityEngine;
using System.Collections.Generic;
using XCharts.Runtime;
using TMPro;
using System.IO;
using UnityEditor;
using System;

public class UIDataManager : MonoBehaviour
{
    public TextMeshPro UITimer;
    public ScatterChart UIScatterChart;
    public GenerationsManager generationManager;

    public List<float[]> generationsDistanceData = new List<float[]>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIScatterChart.ClearData(); //CLEAR DATA

    }

    // Update is called once per frame
    void Update()
    {
        string s = (generationManager.generationTimeLength - generationManager.generationTime).ToString();
        s += "   ";
        s= s.Substring(0, s.IndexOf('.') + 3);
        UITimer.text = s + "\n " + Time.timeScale;
    }

    public void AddGenerationData(int genNum, float[] distances)
    {
        generationsDistanceData.Add(distances);
            //UIScatterChart.series[0].AddData(genNum, distances[i]);// ADDDING DATA   

        int n = 10;
        for (int i = 0; i < (n); i++)
        {
            UIScatterChart.series[0].AddData(genNum, distances[i * generationManager.generationSize / n]);
        }
        UIScatterChart.series[0].AddData(genNum, distances[generationManager.generationSize-1]);
        UIScatterChart.RefreshChart();
    }
    [ContextMenu("SaveData")]
    public void SaveData()
    {

        string filePath = Path.Combine(Application.dataPath, ((GenerationsManager.percentSurvive * 100f).ToString() + "PercentData.txt"));
        FloatArraySaver.SaveFloatArrayListToFile(generationsDistanceData, filePath);
        List<float[]> summaryData = new List<float[]>();
        foreach (var item in generationsDistanceData)
        {
            int n = 10;
            float[] j = new float[n + 1];
            for (int i = 0; i < (n); i++)
            {
                j[i] =item[i * generationManager.generationSize / n];
            }
            j[n] = item[generationManager.generationSize - 1];
            summaryData.Add(j);
        }
        filePath = Path.Combine(Application.dataPath, ((GenerationsManager.percentSurvive * 100f).ToString() + "PercentDataSummary.txt"));
        FloatArraySaver.SaveFloatArrayListToFile(summaryData, filePath);
    }
}
