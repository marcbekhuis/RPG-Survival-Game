using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class PathGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private PathFinding pathFinding;
    [SerializeField] private LayerMask CollisionCheckMask;
    [SerializeField] private bool update = true;
    [SerializeField] private bool debugging = false;

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (!update) return;
        update = false;
        GeneratePath();
    }
#endif

    private void Awake()
    {
        GeneratePath();
    }

    public void GeneratePath()
    {
        tilemap.CompressBounds();
        BoundsInt boundsInt = tilemap.cellBounds;
        pathFinding.pathTiles = new PathFinding.PathTile[boundsInt.size.x, boundsInt.size.y];
        pathFinding.offset = (Vector2Int)boundsInt.position;
        Vector2 offset = new Vector2(0.5f, 0.5f);
        Vector2 size = new Vector2(0.9f, 0.9f);
        for (int y = 0; y < boundsInt.size.y; y++)
        {
            for (int x = 0; x < boundsInt.size.x; x++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                Vector2 position = (Vector2Int)boundsInt.position + offset + gridPosition;
                bool blocked = Physics2D.OverlapBox(position, size, 0, CollisionCheckMask);
                pathFinding.pathTiles[x, y] = new PathFinding.PathTile(position, gridPosition, blocked);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!debugging) return;
        if (!pathFinding) return;
        if (pathFinding.pathTiles == null) return;

        Gizmos.color = Color.red;
        //Vector2 offset = new Vector2(0.5f, 0.5f);
        Vector2 size = new Vector2(0.9f, 0.9f);
        foreach (var pathTile in pathFinding.pathTiles)
        {
            if(pathTile.blocked)
                Gizmos.DrawCube(pathTile.position, size);
        }
    }
#endif
}
