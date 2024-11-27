using System;
using UnityEngine;
public abstract class ICanvasObject
    : MonoBehaviour
{
    public UIKey Key { get; }
    public abstract void OnOpen(Action ondone = null);
    public virtual void Close(Action ondone = null)
    {
        Destroy(gameObject);
        ondone?.Invoke();
    }
}