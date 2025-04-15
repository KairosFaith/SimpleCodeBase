using System;
using UnityEngine;
public abstract class ICanvasObject : MonoBehaviour
{
    public UIType Type { get;}
    public virtual void OnOpen(Action ondone = null)
    {
        ondone?.Invoke();//exclude?
    }
    public virtual void Close(Action ondone = null)
    {
        Destroy(gameObject);
        ondone?.Invoke();
    }
    public bool CheckRectContains(Vector2 screenPosition)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        rect.center = rectTransform.anchoredPosition + GetAnchorOffset(rectTransform);//Canvas must be parent
        return rect.Contains(screenPosition);
    }
    Vector2 GetAnchorOffset(RectTransform rectTransform)
    {
        return GetAnchorOffset(rectTransform.anchorMin, rectTransform.anchorMax);
    }
    Vector2 GetAnchorOffset(Vector2 anchorMin, Vector2 anchorMax)
    {
        return new Vector2((anchorMax.x - anchorMin.x) * Screen.width, (anchorMax.y - anchorMin.y) * Screen.height);
    }
}