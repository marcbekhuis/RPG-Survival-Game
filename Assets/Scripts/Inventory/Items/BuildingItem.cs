using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingItem", menuName = "Items/Building Item")]
public class BuildingItem : UsableItem
{
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private GameObject buildingPreview;
    [SerializeField] private Vector2 collisionOffset;
    [SerializeField] private Vector2 collisionSize;

    private bool placing = false;

    public BuildingItem(SaveFile saveFile, BuildingItem item) : base(saveFile, item)
    {
        this.buildingPrefab = item.buildingPrefab;
        this.buildingPreview = item.buildingPreview;
        this.collisionOffset = item.collisionOffset;
        this.collisionSize = item.collisionSize;
    }

    public BuildingItem(BuildingItem item, int amount, Inventory inventory) : base(item, amount, inventory)
    {
        this.buildingPrefab = item.buildingPrefab;
        this.buildingPreview = item.buildingPreview;
        this.collisionOffset = item.collisionOffset;
        this.collisionSize = item.collisionSize;
    }

    public override Item Create(int amount, Inventory inventory)
    {
        BuildingItem item = new BuildingItem(this, amount, inventory);
        item.name = this.name;
        return item;
    }

    public override Item Create(SaveFile saveFile)
    {
        BuildingItem item = new BuildingItem(saveFile, this);
        item.name = this.name;
        return item;
    }

    public override void UseItem(GameObject user, Vector2 direction)
    {
        placing = !placing;
        if (placing)
            BuildingSystem.instance.ActivateBuilding(buildingPreview, collisionOffset, collisionSize);
        else
        {
            BuildingSystem.instance.PlaceBuilding(buildingPrefab, this);
        }
    }
}
