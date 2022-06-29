using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : Singleton<BuildingSystem>
{
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;
    [SerializeField] private float range = 4;

    [HideInInspector] public Transform player;

    private GameObject buildingPreview;
    private SpriteRenderer previewRenderer;
    private Vector2 collisionOffset;
    private Vector2 collisionSize;
    private bool building = false;

    // Update is called once per frame
    void Update()
    {
        if (building)
        {
            Vector2 position = PlayerCamera.camera.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(player.position, position) < range && !Physics2D.OverlapBox(position + collisionOffset, collisionSize, 0))
                previewRenderer.color = validColor;
            else
                previewRenderer.color = invalidColor;

            buildingPreview.transform.position = position;
        }
    }

    public void ActivateBuilding(GameObject buildingPreview, Vector2 collisionOffset, Vector2 collisionSize)
    {
        this.collisionOffset = collisionOffset;
        this.collisionSize = collisionSize;
        building = true;

        Vector2 position = PlayerCamera.camera.ScreenToWorldPoint(Input.mousePosition);
        this.buildingPreview = Instantiate(buildingPreview, position, new Quaternion(0,0,0,0));
        this.previewRenderer = this.buildingPreview.GetComponent<SpriteRenderer>();
    }

    public void PlaceBuilding(GameObject buildingPrefab, BuildingItem buildingItem)
    {
        Vector2 position = PlayerCamera.camera.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(player.position, position) < range)
        {
            if (!Physics2D.OverlapBox(position + collisionOffset, collisionSize, 0))
            {
                Instantiate(buildingPrefab, position, new Quaternion(0, 0, 0, 0));
                buildingItem.amount--;
            }
        }

        Destroy(buildingPreview);
        building = false;
    }

    public void DisableBuilding()
    {
        building = false;
        Destroy(buildingPreview);
    }
}
