using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FloatArraySaverTester : MonoBehaviour
{
    void Start()
    {
        // Create a sample list of float arrays.
        List<float[]> floatArrayList = new List<float[]>
        {
            new float[] { 1.1f, 2.2f, 3.3f },
            new float[] { 4.4f, 5.5f, 6.6f },
            new float[] { 7.7f, 8.8f, 9.9f }
        };

        // Build the file path using Path.Combine for better compatibility.
        string filePath = Path.Combine(Application.dataPath, "fileName.txt");

        // Save the list to a file.
        FloatArraySaver.SaveFloatArrayListToFile(floatArrayList, filePath);

        // Load the list from the file.
        List<float[]> loadedList = FloatArraySaver.LoadFloatArrayListFromFile(filePath);

        if (loadedList != null)
        {
            foreach (float[] array in loadedList)
            {
                string arrayString = string.Join(", ", array);
                Debug.Log("Loaded Array: " + arrayString);
            }

            if (loadedList.Count > 0 && loadedList[0].Length > 0)
            {
                float firstElementOfFirstArray = loadedList[0][0];
                Debug.Log("First element of first array: " + firstElementOfFirstArray);
            }

            float totalSum = 0;
            foreach (float[] array in loadedList)
            {
                foreach (float value in array)
                {
                    totalSum += value;
                }
            }
            Debug.Log("Total sum of all floats: " + totalSum);
        }
    }
}
