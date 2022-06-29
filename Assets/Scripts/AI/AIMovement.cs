using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : CharacterMovement
{
    [SerializeField] private AIWandering wandering;
    [SerializeField] private bool debugging = false;

    private Vector2Int targetPosition;
    private Vector2Int aiPosition;

    private List<PathFinding.PathTile> path = new List<PathFinding.PathTile>();

    public void SetTarget(Vector2 target, out int pathLength)
    {
        targetPosition = PathFinding.instance.WorldToGridPosition(target);
        aiPosition = PathFinding.instance.WorldToGridPosition(this.transform.position);

        foreach (var tile in path)
        {
            tile.onChange -= PathBroken;
        }
        path.Clear();
        path.AddRange(PathFinding.instance.GetPath(aiPosition, targetPosition, this));
        foreach (var tile in path)
        {
            tile.onChange += PathBroken;
        }

        pathLength = path.Count;
        wandering.targetPosition = path[path.Count - 1].position;
    }

    public void UpdatePath()
    {
        if (path.Count < 1)
        {
            return;
        }

        if (Vector2.Distance(this.transform.position, path[0].position) > 1.5f)
        {
            aiPosition = PathFinding.instance.WorldToGridPosition(this.transform.position);

            foreach (var tile in path)
            {
                tile.onChange -= PathBroken;
            }
            path.Clear();
            path.AddRange(PathFinding.instance.GetPath(aiPosition, targetPosition, this));
            foreach (var tile in path)
            {
                tile.onChange += PathBroken;
            }

            wandering.targetPosition = path[path.Count - 1].position;
        }
    }

    private void PathBroken(CharacterMovement character)
    {
        if (character != this)
        {
            Debug.LogError("Path Broken");
            aiPosition = PathFinding.instance.WorldToGridPosition(this.transform.position);

            foreach (var tile in path)
            {
                tile.onChange -= PathBroken;
            }
            path.Clear();
            path.AddRange(PathFinding.instance.GetPath(aiPosition, targetPosition, this));
            foreach (var tile in path)
            {
                tile.onChange += PathBroken;
            }

            wandering.targetPosition = path[path.Count - 1].position;
        }
    }

    private void Update()
    {
        if (GameManager.instance.paused)
        {
            Move(Vector2.zero);
            return;
        }

        if(path.Count > 0)
        {
            if (Vector2.Distance(path[0].position, (Vector2)this.transform.position) > 2 * Time.deltaTime)
            {
                Vector2 direction = path[0].position - (Vector2)this.transform.position;
                Move(direction);
            }
            else
                Move(Vector2.zero);

            if (Vector2.Distance(path[0].position, (Vector2)this.transform.position) < 0.05f)
            {
                path[0].onChange -= PathBroken;
                path.RemoveAt(0);
            }

            UpdatePath();
        }
        else
            Move(Vector2.zero);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!debugging)
            return;
        if (path.Count < 1)
            return;

        Gizmos.color = Color.cyan;
        foreach (var tile in path)
        {
            Gizmos.DrawCube(tile.position, Vector3.one * 0.5f);
        }
    }
#endif
}
