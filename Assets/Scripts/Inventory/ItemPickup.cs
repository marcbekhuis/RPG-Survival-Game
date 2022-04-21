using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private float range;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, range, itemLayer);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<DropItem>(out DropItem dropItem))
            {
                dropItem.PickupItem(inventory);
            }
        }
    }
}
