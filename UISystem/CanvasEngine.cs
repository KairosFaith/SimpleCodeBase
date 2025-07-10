using System;
using System.Collections.Generic;
using UnityEngine;
public enum UIType
{
    //Project Dependent!
}
[RequireComponent(typeof(Canvas))]
public class CanvasEngine : MonoBehaviour
{
    const string ResourcePath = "UI";
    Dictionary<UIType, CanvasObject> _prefabs;
    List<CanvasObject> _CurrentUIObjects = new List<CanvasObject>();
    public static CanvasEngine Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _prefabs = new Dictionary<UIType, CanvasObject>();
            CanvasObject[] prefabs = Resources.LoadAll<CanvasObject>(ResourcePath);
            foreach (CanvasObject prefab in prefabs)
                _prefabs.Add(prefab.Type, prefab);
        }
        else
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    public CanvasObject OpenUnlisted(UIType key, Action ondone = null)
    {
        if (_prefabs.TryGetValue(key, out CanvasObject prefab))
        {
            CanvasObject newObj = Instantiate(prefab, transform);
            newObj.OnOpen(ondone);
            return newObj;
        }
        else
            throw new Exception(key + "not found");
    }
    public CanvasObject Open(UIType key, Action ondone = null)
    {
        CanvasObject newObj = OpenUnlisted(key, ondone);
        _CurrentUIObjects.Add(newObj);
        return newObj;
    }
    public void Close(CanvasObject objectToRemove, Action ondone = null)
    {
        //if (_CurrentUIObjects.Contains(objectToRemove))
            _CurrentUIObjects.Remove(objectToRemove);
        objectToRemove.Close(ondone);
    }
    public void CloseAllListed(Action ondone = null)
    {
        CanvasObject[] objectsToClose = _CurrentUIObjects.ToArray();
        foreach(CanvasObject cg in objectsToClose)
            Close(cg, ondone);
        _CurrentUIObjects.Clear();
    }
}