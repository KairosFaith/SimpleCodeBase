using System.Collections.Generic;
using UnityEngine;
public enum NavTileType
{
    Walkable,
    Boundary,
}
public enum TileStatus
{
    Available,
    Occupied,
    Collide,
    Invalid,
}
public class NavTile : MonoBehaviour
{
    public Vector2Int GridCoordinates;
    public ITileNavOccupant Occupant;
    public NavTileType TileType;
    public TileStatus Status
    {
        get
        {
            if (Occupant != null)
                return TileStatus.Occupied;
            else if (TileType == NavTileType.Boundary)
                return TileStatus.Collide;
            else
                return TileStatus.Available;
        }
    }
    void Awake()
    {
        EventEngine.SubscribeEvent(EventType.RegisterTile, RegisterTile);
    }
    private void OnDestroy()
    {
        EventEngine.UnsubscribeEvent(EventType.RegisterTile, RegisterTile);
        TileNavEngine engine = TileNavEngine.Instance;
        if (engine != null)
            engine.UnregisterTile(this);
    }
    void RegisterTile(object sender, object[] args)
    {
        TileNavEngine engine = (TileNavEngine)sender;
        engine.RegisterTile(this);
    }
    public List<NavTile> GetAdjacentTiles()
    {
        List<NavTile> adjacentTiles = new List<NavTile>();
        foreach (Vector2Int direction in TileNavEngine.GridDirections)
        {
            Vector2Int newCoordinates = GridCoordinates + direction;
            if (TileNavEngine.Instance.TryGetTile(newCoordinates, out _, out NavTile tile))
                adjacentTiles.Add(tile);
        }
        return adjacentTiles;
    }
}