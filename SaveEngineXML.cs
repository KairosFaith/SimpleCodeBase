using System.IO;
using System.Xml.Serialization;
using System;
using UnityEngine;
public static class SaveEngineXML
{
    static string GetPath(Type dataType)
    {
        string className = dataType.Name;
        return Application.streamingAssetsPath + "/" + $"{className}.xml";
    }
    public static void SaveXML<T>(T dataToSave) where T : class
    {
        Type dataType = typeof(T);
        string path = GetPath(dataType);
        XmlSerializer serializer = new XmlSerializer(dataType);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, dataToSave);
        }
    }
    public static bool TryLoadXML<T>(out T dataToLoad) where T : class
    {
        Type dataType = typeof(T);
        string path = GetPath(dataType);
        dataToLoad = null;
        if (File.Exists(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                dataToLoad = serializer.Deserialize(stream) as T;
                return true;
            }
        }
        return false;
    }
}