using UnityEngine;
public abstract class ITileNavOccupant : MonoBehaviour
{
    public Vector2Int GridCoordinates;
    public NavTile OccupiedTile;
    protected bool TryMove(Vector2Int direction, out TileStatus result, out NavTile tile)
    {
        TileNavEngine engine = TileNavEngine.Instance;
        Vector2Int newCoordinates = GridCoordinates + direction;
        if (engine.TryGetTile(newCoordinates, out result, out tile))
        {
            if(result==TileStatus.Available)
                OnMoveTile(tile, OccupiedTile);
            return true;
        }
        return false;
    }
    /// <summary>
    ///while moving, 2 tiles could be occupied by the same occupant at the same time. user to handle the tile change in this function
    /// </summary>
    protected abstract void OnMoveTile(NavTile newTile, NavTile previousTile);
}