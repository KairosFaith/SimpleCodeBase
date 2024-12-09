using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Canvas))]//Place this script on the canvas object
public class CanvasEngine : MonoBehaviour
{
    const string ResourcePath = "ui";
    public static CanvasEngine Instance { get; private set; }
    public List<ICanvasObject> _CurrentUIObjects = new List<ICanvasObject>();
    static Dictionary<UIKey, ICanvasObject> _prefabs;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning(Instance.gameObject.name + nameof(CanvasEngine) + " Already exists");
            Destroy(this);
        }
        if(_prefabs==null)
            LoadPrefabBank();
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    void LoadPrefabBank()
    {
        _prefabs = new Dictionary<UIKey, ICanvasObject>();
        ICanvasObject[] prefabs = Resources.LoadAll<ICanvasObject>(ResourcePath);
        foreach (ICanvasObject prefab in prefabs)
            if (Enum.TryParse(prefab.name, out UIKey key))
                _prefabs.Add(key, prefab);
            else
                throw new Exception($"{prefab.name} does not match any {nameof(UIKey)}");
    }
    public ICanvasObject OpenUnlisted(UIKey key, Action ondone = null)
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
    public ICanvasObject Open(UIKey key, Action ondone = null)
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
public enum UIKey
{
    StartMenu,
    Instructions,
    Timer,
}