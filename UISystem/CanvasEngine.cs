using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Canvas))]
public class CanvasEngine : MonoBehaviour
{
    const string ResourcePath = "UI";
    Dictionary<UIType, ICanvasObject> _prefabs;
    public List<ICanvasObject> _CurrentUIObjects = new List<ICanvasObject>();
    public static CanvasEngine Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPrefabBank();
        }
        else
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    void LoadPrefabBank()
    {
        _prefabs = new Dictionary<UIType, ICanvasObject>();
        ICanvasObject[] prefabs = Resources.LoadAll<ICanvasObject>(ResourcePath);
        foreach (ICanvasObject prefab in prefabs)
                _prefabs.Add(prefab.Type, prefab);
    }
    public ICanvasObject OpenUnlisted(UIType key, Action ondone = null)
    {
        if (_prefabs.TryGetValue(key, out ICanvasObject prefab))
        {
            ICanvasObject newObj = Instantiate(prefab, transform);
            newObj.OnOpen(ondone);
            return newObj;
        }
        else
            throw new Exception(key + "not found");
    }
    public ICanvasObject Open(UIType key, Action ondone = null)
    {
        ICanvasObject newObj = OpenUnlisted(key, ondone);
        _CurrentUIObjects.Add(newObj);
        return newObj;
    }
    public void Close(ICanvasObject objectToRemove, Action ondone = null)
    {
        if (_CurrentUIObjects.Contains(objectToRemove))
            _CurrentUIObjects.Remove(objectToRemove);
        objectToRemove.Close(ondone);
    }
    public void CloseAllListed(Action ondone = null)
    {
        ICanvasObject[] objectsToClose = _CurrentUIObjects.ToArray();
        foreach(ICanvasObject cg in objectsToClose)
            Close(cg, ondone);
        _CurrentUIObjects.Clear();
    }
    public void DestroyAll()//hope you wont need to use this but here it is
    {
        CloseAllListed();
        bool stillHasChildren = transform.childCount > 0;
        while (stillHasChildren)
        {
            Transform child = transform.GetChild(0);
            Destroy(child);
        }
    }
}
public enum UIType
{
    //Project Dependent!
}