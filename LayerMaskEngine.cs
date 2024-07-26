using UnityEngine;
public static class LayerMaskEngine
{
    public static bool MatchLayerMask(this GameObject gameObject, params string[] layerNames)
    {
        return MatchLayerMask(gameObject.layer, layerNames);
    }
    public static bool MatchLayerMask(int layer, params string[] layerNames)
    {
        LayerMask mask = LayerMask.GetMask(layerNames);
        return mask.ContainsLayer(layer);
    }
    public static bool ContainsLayer(this LayerMask layerMask, GameObject gameObject)
    {
        return ContainsLayer(layerMask, gameObject.layer);
    }
    public static bool ContainsLayer(this LayerMask layerMask, int layer)
    {
        int bitLayer = 1 << layer;
        int match = bitLayer & layerMask;
        return match > 0;
    }
}