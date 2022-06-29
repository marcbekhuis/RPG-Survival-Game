using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWandering : MonoBehaviour
{
    [SerializeField] private int range = 5;
    [SerializeField] private int maxPathLength = 5;
    [SerializeField] private int acceptanceRange = 2;
    [SerializeField] private float minCooldown = 5;
    [SerializeField] private float maxCooldown = 30;
    [SerializeField] protected AIMovement movement;

    [SerializeField] private bool debugging = false;

    private bool point = false;
    private float timer = 0;
    [HideInInspector] public Vector2 targetPosition;

    public Vector2 GetWanderingTarget()
    {
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        direction.Normalize();

        return PathFinding.instance.ValidPosition((Vector2)this.transform.position + direction * range, movement, acceptanceRange);
    }

    protected void SetupWandering()
    {
        targetPosition = this.transform.position;
        timer = GameManager.instance.time;
        point = false;
    }

    protected void Wandering()
    {
        if (Vector2.Distance(targetPosition, this.transform.position) < 0.1f && !point)
        {
            timer = GameManager.instance.time + Random.Range(minCooldown, maxCooldown);
            point = true;
        }

        if (GameManager.instance.time > timer && point)
        {
            int pathLength = int.MaxValue;
            do
            {
                Vector2 target = GetWanderingTarget();
                if (target == null)
                    continue;
                movement.SetTarget(target, out pathLength);
            } while (pathLength > maxPathLength);
            point = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!debugging)
            return;

        Gizmos.color = Color.black;
        Gizmos.DrawCube(targetPosition, Vector3.one);
    }
#endif
}
