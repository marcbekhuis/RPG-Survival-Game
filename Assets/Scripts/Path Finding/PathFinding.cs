using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding instance;
    public PathTile[,] pathTiles;
    public Vector2Int offset;

    [SerializeField] private bool debugging = false;

    private List<PathTile> openList = new List<PathTile>();
    private List<PathTile> closedList = new List<PathTile>();
    public Vector2 tileOffset = new Vector2(0.5f , 0.5f);

    private const int moveStraightCost = 10;
    private const int moveDiagonalCost = 14;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        instance = this;
    }

    public class PathTile
    {
        public Vector2 position;
        public Vector2Int gridPosition;
        public bool blocked;
        private CharacterMovement characterVar;

        public delegate void Change(CharacterMovement character);
        public event Change onChange = delegate { };

        public PathTile(Vector2 position, Vector2Int gridPosition, bool blocked)
        {
            this.position = position;
            this.gridPosition = gridPosition;
            this.blocked = blocked;
            Reset();
        }

        public int gCost;
        public int hCost;
        public int fCost;
        public PathTile cameFrom;

        public CharacterMovement character 
        { 
            get => characterVar;
            set
            {
                if(characterVar != value)
                    onChange(value);
                characterVar = value;
            }
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public void Reset()
        {
            gCost = int.MaxValue;
            CalculateFCost();
            cameFrom = null;
        }
    }

    public PathTile[] GetPath(Vector2 startPoint, Vector2 endPoint, CharacterMovement requesting)
    {
        PathTile startTile = GetTile(startPoint);
        PathTile endTile = GetTile(endPoint);

        return GetPath(startTile, endTile, requesting);
    }

    public PathTile[] GetPath(Vector2Int startPoint, Vector2Int endPoint, CharacterMovement requesting)
    {
        PathTile startTile = GetTile(startPoint);
        PathTile endTile = GetTile(endPoint);

        return GetPath(startTile, endTile, requesting);
    }

    public PathTile[] GetPath(PathTile startTile, PathTile endTile, CharacterMovement requesting)
    {
        foreach (var tile in closedList)
        {
            tile.Reset();
        }

        foreach (var tile in openList)
        {
            tile.Reset();
        }

        closedList.Clear();
        openList.Clear();

        if (startTile == null)
        {
            Debug.Log("Start position invalid");
            return null;
        }
        if (endTile == null)
        {
            Debug.Log("End position invalid");
            return null;
        }
        openList.Add(startTile);
        startTile.gCost = 0;
        startTile.hCost = CalculateDistanceCost(startTile.gridPosition, endTile.gridPosition);
        startTile.CalculateFCost();

        PathTile closestTile = startTile;
        while (openList.Count > 0)
        {
            PathTile currentTile = GetLowestFCostNode(openList);
            if (currentTile == endTile)
            {
                // End
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
                return CalculatePath(endTile).ToArray();
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach (var neighbourTile in GetNeighbour(currentTile))
            {
                if (closedList.Contains(neighbourTile)) continue;
                if (openList.Contains(neighbourTile)) continue;
                if (neighbourTile.blocked)
                {
                    closedList.Add(neighbourTile);
                    continue;
                }
                if (neighbourTile.character && neighbourTile.character != requesting)
                {
                    closedList.Add(neighbourTile);
                    continue;
                }

                int tentativeGcost = currentTile.gCost + CalculateDistanceCost(currentTile.gridPosition, neighbourTile.gridPosition);
                if (tentativeGcost < neighbourTile.gCost)
                {
                    neighbourTile.cameFrom = currentTile;
                    neighbourTile.gCost = tentativeGcost;
                    neighbourTile.hCost = CalculateDistanceCost(neighbourTile.gridPosition, endTile.gridPosition);
                    neighbourTile.CalculateFCost();

                    if (!openList.Contains(neighbourTile))
                        openList.Add(neighbourTile);
                        //openList.Insert(Random.Range(0, openList.Count),neighbourTile);
                }
            }

            if (currentTile.hCost < closestTile.hCost)
                closestTile = currentTile;
        }

        return CalculatePath(closestTile).ToArray();
    }

    public List<PathTile> GetNeighbour(PathTile currentTile)
    {
        List<PathTile> neighbours = new List<PathTile>();
        if(currentTile.gridPosition.x - 1 >= 0)
        {
            // Left
            neighbours.Add(GetTile(currentTile.gridPosition.x - 1, currentTile.gridPosition.y));
            // Left Down
            if(currentTile.gridPosition.y - 1 >= 0) neighbours.Add(GetTile(currentTile.gridPosition.x - 1, currentTile.gridPosition.y - 1));
            // Left Up
            if (currentTile.gridPosition.y + 1 < pathTiles.GetLength(1)) neighbours.Add(GetTile(currentTile.gridPosition.x - 1, currentTile.gridPosition.y + 1));
        }
        if (currentTile.gridPosition.x + 1 < pathTiles.GetLength(0))
        {
            // Right
            neighbours.Add(GetTile(currentTile.gridPosition.x + 1, currentTile.gridPosition.y));
            // Right Down
            if (currentTile.gridPosition.y - 1 >= 0) neighbours.Add(GetTile(currentTile.gridPosition.x + 1, currentTile.gridPosition.y - 1));
            // Right Up
            if (currentTile.gridPosition.y + 1 < pathTiles.GetLength(1)) neighbours.Add(GetTile(currentTile.gridPosition.x + 1, currentTile.gridPosition.y + 1));
        }
        // Down
        if (currentTile.gridPosition.y - 1 >= 0) neighbours.Add(GetTile(currentTile.gridPosition.x, currentTile.gridPosition.y - 1));
        // Up
        if (currentTile.gridPosition.y + 1 < pathTiles.GetLength(1)) neighbours.Add(GetTile(currentTile.gridPosition.x, currentTile.gridPosition.y + 1));

        return neighbours;
    }

    private List<PathTile> CalculatePath(PathTile endTile)
    {
        List<PathTile> path = new List<PathTile>();
        path.Add(endTile);
        PathTile currentTile = endTile;
        while (currentTile.cameFrom != null)
        {
            path.Add(currentTile.cameFrom);
            currentTile = currentTile.cameFrom;
        }
        path.Reverse();
        return path;
    }

    private PathTile GetLowestFCostNode(List<PathTile> pathTiles)
    {
        PathTile lowestFCostNode = pathTiles[0];
        for (int i = 1; i < pathTiles.Count; i++)
        {
            if (pathTiles[0].fCost < lowestFCostNode.fCost)
                lowestFCostNode = pathTiles[0];
        }
        return lowestFCostNode;
    }

    private PathTile GetTile(Vector2 worldPosition)
    {
        Vector2Int gridPosition = Vector2Int.RoundToInt(worldPosition - tileOffset) - offset;

        if (gridPosition.x < 0 || gridPosition.y < 0)
            return null;
        if (gridPosition.x >= pathTiles.GetLength(0) || gridPosition.y >= pathTiles.GetLength(1))
            return null;

        return pathTiles[gridPosition.x, gridPosition.y];
    }

    private PathTile GetTile(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0 || gridPosition.y < 0)
            return null;
        if (gridPosition.x >= pathTiles.GetLength(0) || gridPosition.y >= pathTiles.GetLength(1))
            return null;

        return pathTiles[gridPosition.x, gridPosition.y];
    }

    private PathTile GetTile(int x, int y)
    {
        if (x < 0 || y < 0)
            return null;
        if (x >= pathTiles.GetLength(0) || y >= pathTiles.GetLength(1))
            return null;

        return pathTiles[x, y];
    }

    private int CalculateDistanceCost(Vector2Int fromPos, Vector2Int toPos)
    {
        int xDistance = Mathf.Abs(fromPos.x - toPos.x);
        int yDistance = Mathf.Abs(fromPos.y - toPos.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return moveDiagonalCost * Mathf.Min(xDistance, yDistance) + moveStraightCost * remaining;
    }

    public Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        return Vector2Int.RoundToInt(worldPosition - tileOffset) - offset;
    }

    public Vector2 GridToWorldPosition(Vector2 gridPosition)
    {
        return gridPosition + tileOffset + offset;
    }

    public Vector2 ValidPosition(Vector2 worldPosition, CharacterMovement requesting, int range = 5)
    {
        Vector2Int gridPosition = WorldToGridPosition(worldPosition);
        PathTile closestTile = GetTile(gridPosition);
        if (closestTile != null)
        {
            if (!closestTile.character || closestTile.character == requesting)
                if (!closestTile.blocked)
                    return closestTile.position;
        }

        float closest = range * 2;
        for (int y = -range + 1; y < range; y++)
        {
            for (int x = -range + 1; x < range; x++)
            {
                PathTile tile = GetTile(gridPosition + new Vector2Int(x, y));
                if (tile == null)
                    continue;
                if (tile.blocked)
                    continue;
                if (tile.character && tile.character != requesting)
                    continue;

                float distance = Vector2.Distance(tile.gridPosition, gridPosition);
                if (distance < closest)
                {
                    closest = distance;
                    closestTile = tile;
                }
            }
        }

        if(closestTile == null)
        {
            Debug.LogError("No valid position found");
            return worldPosition;
        }
        return closestTile.position;
    }

    public PathTile SetTileCharacter(Vector2 worldPosition, CharacterMovement character)
    {
        PathTile tile = GetTile(worldPosition);
        tile.character = character;
        return tile;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!debugging)
            return;

        foreach (var tile in pathTiles)
        {
            if (tile.blocked)
                Gizmos.color = Color.red;
            else if (tile.character)
                Gizmos.color = Color.yellow;
            else
                Gizmos.color = Color.green;

            Gizmos.DrawCube(tile.position, Vector2.one);
        }
    }
#endif
}
