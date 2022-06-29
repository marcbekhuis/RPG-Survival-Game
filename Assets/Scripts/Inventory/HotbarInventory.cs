using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarInventory : Inventory
{
    [SerializeField] private CharacterMovement characterMovement;

    private Vector2Int index = Vector2Int.zero;
    private bool sameUpdate = false;
    private Vector2Int oldIndex;

    public Item selectedItem { get => GetItem(index); }

    public delegate void SelectedItemChanged(Vector2Int index, Vector2Int oldIndex);
    public event SelectedItemChanged onSelectedItemChanged = delegate { };

    // Start is called before the first frame update
    public override void FullSetup()
    {
        base.FullSetup();
        onSelectedItemChanged(index, index);
        onItemSlotChange += SelectedItemMoved;
    }

    // Update is called once per frame
    void Update()
    {
        sameUpdate = false;
        if (GameManager.instance.playerInputBlocked) return;
        if (selectedItem)
            if (selectedItem.used) return;

        if (selectedItem)
            if(selectedItem.TryGetType<UsableItem>(out UsableItem usableItem))
                usableItem.UseItemAction(this.gameObject, characterMovement.lastDirection);

        for (int x = 0; x < 9; x++)
        {
            if (InputManager.instance.GetKeyDown(System.Enum.Parse<InputActions>("SelectItem" + (x + 1))) && x != index.x)
            {
                if (GetItem(x, 0))
                {
                    Vector2Int oldIndex = index;
                    index = Vector2Int.right * x;
                    onSelectedItemChanged(index, oldIndex);
                    if (selectedItem)
                        if (selectedItem.TryGetType<UsableItem>(out UsableItem usableItem))
                            usableItem.OnSelect(this.gameObject, characterMovement.lastDirection);
                }
            }
        }

        float turnValue = InputManager.instance.GetAxisRaw(AxisActions.MouseScrollWheel);
        if (turnValue > 0)
        {
            Vector2Int oldIndex = index;
            for (int i = 0; i < 9; i++)
            {
                index += Vector2Int.right;
                if (index.x > 8)
                    index = Vector2Int.zero;
                if (GetItem(index.x, 0))
                    break;
            }
            onSelectedItemChanged(index, oldIndex);
            if (selectedItem)
                if (selectedItem.TryGetType<UsableItem>(out UsableItem usableItem))
                    usableItem.OnSelect(this.gameObject, characterMovement.lastDirection);
        }
        else if (turnValue < 0)
        {
            Vector2Int oldIndex = index;
            for (int i = 0; i < 9; i++)
            {
                index += Vector2Int.left;
                if (index.x < 0)
                    index = new Vector2Int(8, 0);
                if (GetItem(index.x, 0))
                    break;
            }
            onSelectedItemChanged(index, oldIndex);
            onSelectedItemChanged(index, oldIndex);
            if (selectedItem)
                if (selectedItem.TryGetType<UsableItem>(out UsableItem usableItem))
                    usableItem.OnSelect(this.gameObject, characterMovement.lastDirection);
        }
    }

    private void SelectedItemMoved(Vector2Int index, Item item)
    {
        if (sameUpdate)
        {
            Vector2Int oldIndex = this.index;
            this.index = this.oldIndex;
            onSelectedItemChanged(this.index, oldIndex);
            if (selectedItem)
                if (selectedItem.TryGetType<UsableItem>(out UsableItem usableItem))
                    usableItem.OnSelect(this.gameObject, characterMovement.lastDirection);
        }
        else if(index == this.index)
        {
            oldIndex = index;
            if (selectedItem)
            {
                if (selectedItem.TryGetType<UsableItem>(out UsableItem usableItem))
                    usableItem.OnSelect(this.gameObject, characterMovement.lastDirection);
            }
            else
            {
                Vector2Int oldIndex = this.index;
                for (int i = 0; i < 9; i++)
                {
                    this.index += Vector2Int.right;
                    if (this.index.x > 8)
                        this.index = Vector2Int.zero;
                    if (GetItem(this.index.x, 0))
                        break;
                }
                onSelectedItemChanged(this.index, oldIndex);
                if (selectedItem)
                    if (selectedItem.TryGetType<UsableItem>(out UsableItem usableItem))
                        usableItem.OnSelect(this.gameObject, characterMovement.lastDirection);
            }
        }
        sameUpdate = true;
    }
}
