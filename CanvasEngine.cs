using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Canvas))]//Place this script on the canvas object
public class CanvasEngine : MonoBehaviour
{
    const string ResourcePath = "ui";
    public static CanvasEngine Instance { get; private set; }
    public List<CanvasGroup> _CurrentUIObjects = new List<CanvasGroup>();
    static Dictionary<UIKey, CanvasGroup> _prefabs;
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
        _prefabs = new Dictionary<UIKey, CanvasGroup>();
        CanvasGroup[] prefabs = Resources.LoadAll<CanvasGroup>(ResourcePath);
        foreach (CanvasGroup prefab in prefabs)
            if (Enum.TryParse(prefab.name, out UIKey key))
                _prefabs.Add(key, prefab);
            else
                throw new Exception($"{prefab.name} does not match any {nameof(UIKey)}");
    }
    public CanvasGroup Open(UIKey key, float fadeTime, Action ondone = null)
    {
        if (_prefabs.TryGetValue(key, out CanvasGroup prefab))
        {
            CanvasGroup newObj = Instantiate(prefab, transform);
            StartCoroutine(Fade(newObj, true, fadeTime, ondone));
            return newObj;
        }
        else
            throw new Exception(key + "not found");
    }
    public CanvasGroup OpenInList(UIKey key, float fadeTime, Action ondone = null)
    {
        CanvasGroup newObj = Open(key, fadeTime, ondone);
        _CurrentUIObjects.Add(newObj);
        return newObj;
    }
    void Close(CanvasGroup objectToRemove, float fadeTime, Action ondone = null)
    {
        StartCoroutine(Fade(objectToRemove, false, fadeTime, ondone));
    }
    public void Close(CanvasGroup objectToRemove, float fadeTime)
    {
        if (_CurrentUIObjects.Contains(objectToRemove))
            _CurrentUIObjects.Remove(objectToRemove);
        Close(objectToRemove, fadeTime, () => Destroy(objectToRemove));
    }
    public void CloseAllListed(float fadeTime)
    {
        CanvasGroup[] objectsToClose = _CurrentUIObjects.ToArray();
        foreach(CanvasGroup cg in objectsToClose)
            Close(cg, fadeTime, () => Destroy(cg));
        _CurrentUIObjects.Clear();
    }
    public void DestroyAll()//hope you wont need to use this but here it is
    {
        CloseAllListed(0f);
        bool stillHasChildren = transform.childCount > 0;
        while (stillHasChildren)
        {
            Transform child = transform.GetChild(0);
            Destroy(child);
        }
    }
    public static IEnumerator Fade(CanvasGroup cg, bool fadeIn, float fadeTime,  Action ondone)
    {
        float t = 0;
        float targetValue;
        if (fadeIn)
        {
            targetValue = 1.0f;
            cg.alpha = 0;
        }
        else
        {
            targetValue = 0.0f;
            t = 1 - cg.alpha;
        }
        for (float alpha = cg.alpha; t < 1.0f; t += Time.unscaledDeltaTime / fadeTime)
        {
            cg.alpha = Mathf.Lerp(alpha, targetValue, t);
            yield return new WaitForEndOfFrame();
        }
        ondone?.Invoke();
    }
}
public enum UIKey
{

}