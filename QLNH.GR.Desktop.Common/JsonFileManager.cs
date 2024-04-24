using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public static class JsonFileManager
{
    public static List<T> ReadListFromJson<T>(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(jsonData);
            }
            else
            {
                Console.WriteLine($"JSON file not found: {filePath}");
                return new List<T>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JSON file: {ex.Message}");
            return new List<T>();
        }
    }

    public static void WriteListToJson<T>(List<T> data, string filePath)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonData);
            Console.WriteLine($"Data successfully saved to JSON file: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to JSON file: {ex.Message}");
        }
    }

    public static T ReadFromJson<T>(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            else
            {
                Console.WriteLine($"JSON file not found: {filePath}");
                return default;
            }
        }
        catch (Exception ex)
        {
            throw ex;
           
        }
    }

    public static void WriteToJson<T>(T data, string filePath)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonData);
            Console.WriteLine($"Data successfully saved to JSON file: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to JSON file: {ex.Message}");
        }
    }
}
