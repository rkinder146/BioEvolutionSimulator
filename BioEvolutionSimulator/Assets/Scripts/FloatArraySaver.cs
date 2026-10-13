using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FloatArraySaver
{
    public static void SaveFloatArrayListToFile(List<float[]> floatArrayList, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write the number of arrays.
                writer.WriteLine(floatArrayList.Count);
                int n = 1;
                foreach (float[] floatArray in floatArrayList)
                {
                    // Write the length of each array.
                    writer.WriteLine(n);
                    // Write the array elements as a comma-separated line.
                    string line = string.Join(",", floatArray);
                    writer.WriteLine(line);
                    n++;
                }
            }
            Debug.Log("Float array list saved to: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving float array list: " + ex.Message);
        }
    }

    public static List<float[]> LoadFloatArrayListFromFile(string filePath)
    {
        List<float[]> floatArrayList = new List<float[]>();
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                int arrayCount = int.Parse(reader.ReadLine());
                for (int i = 0; i < arrayCount; i++)
                {
                    int arrayLength = int.Parse(reader.ReadLine());
                    string line = reader.ReadLine();
                    string[] floatStrings = line.Split(',');
                    if (floatStrings.Length != arrayLength)
                    {
                        Debug.LogError($"Error: Array length mismatch. Expected {arrayLength}, got {floatStrings.Length}");
                        return null;
                    }
                    float[] floatArray = new float[arrayLength];
                    for (int j = 0; j < arrayLength; j++)
                    {
                        floatArray[j] = float.Parse(floatStrings[j]);
                    }
                    floatArrayList.Add(floatArray);
                }
            }
            Debug.Log("Float array list loaded from: " + filePath);
            return floatArrayList;
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("File not found: " + filePath);
            return null;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading float array list: " + ex.Message);
            return null;
        }
    }
}
