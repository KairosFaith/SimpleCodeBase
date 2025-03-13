using System.IO;
using System.Xml.Serialization;
using System;
using UnityEngine;
public static class SaveEngineXML
{
    static string GetPath(Type dataType)
    {
        string className = dataType.Name;
        return "/" + $"{className}.xml";
    }
    public static void SaveXML<T>(T dataToSave) where T : class
    {
        string path = GetPath(typeof(T));
        SaveXML(dataToSave, path);
    }
    public static void SaveXML<T>(T dataToSave, string streamPath)
    {
        Type dataType = typeof(T);
        string path = Application.streamingAssetsPath + streamPath;
        XmlSerializer serializer = new XmlSerializer(dataType);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, dataToSave);
        }
    }
    public static bool TryLoadXML<T>(out T dataToLoad) where T : class
    {
        string path = GetPath(typeof(T));
        return TryLoadXML<T>(path, out dataToLoad);
    }
    public static bool TryLoadXML<T>(string streamPath, out T dataToLoad) where T : class
    {
        string path = Application.streamingAssetsPath + streamPath;
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