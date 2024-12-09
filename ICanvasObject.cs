using System;
using UnityEngine;
public abstract class ICanvasObject
    : MonoBehaviour
{
    public UIKey Key { get; }
    public virtual void OnOpen(Action ondone = null)
    {
        ondone?.Invoke();
    }
    public virtual void Close(Action ondone = null)
    {
        Destroy(gameObject);
        ondone?.Invoke();
    }
}