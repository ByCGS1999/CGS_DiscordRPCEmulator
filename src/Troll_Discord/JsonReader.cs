using System;
using Newtonsoft.Json;

public class JsonReader
{
    public static string ReadJsonConfig(string filePath, string valuetoFind)
    {
        dynamic jsonRead = JsonConvert.DeserializeObject(File.ReadAllText(filePath));

        return jsonRead[valuetoFind];
    }

    public static dynamic ReadJsonDynamic(string filePath)
    {
        return JsonConvert.DeserializeObject(File.ReadAllText(filePath));
    }
}