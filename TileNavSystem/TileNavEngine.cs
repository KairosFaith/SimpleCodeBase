using System.Collections.Generic;
using UnityEngine;
public class TileNavEngine : MonoBehaviour
{
    public static readonly Vector2Int[] GridDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    public static TileNavEngine Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    Dictionary<Vector2Int, NavTile> _NavTiles = new Dictionary<Vector2Int, NavTile>();
    void Start()
    {
        EventEngine.BroadcastEvent(EventType.RegisterTile, this);
    }
    public void RegisterTile(NavTile tile)
    {
        _NavTiles.Add(tile.GridCoordinates, tile);
    }
    public void UnregisterTile(NavTile tile)
    {
        _NavTiles.Remove(tile.GridCoordinates);
    }
    public bool TryGetTile(Vector2Int coordinates, out TileStatus result, out NavTile tile)
    {
        if (_NavTiles.TryGetValue(coordinates, out tile))
        {
            result = tile.Status;
            return true;
        }
        else
        {
            result = TileStatus.Invalid;
            tile = null;
            return false;
        }
    }
}