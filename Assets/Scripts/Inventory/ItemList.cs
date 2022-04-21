using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ItemList : Singleton<ItemList>
{
    [SerializeField] private bool update = true;
    public List<Item> items = new List<Item>();

    public Item Getitem(string name)
    {
        foreach (var item in items)
        {
            if (item.name == name) return item;
        }

        return default;
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (!update) return;
        update = false;

        items.Clear();
        foreach (var guid in AssetDatabase.FindAssets("t:" + typeof(Item).Name))
        {
            items.Add(AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid)));
        }
    }
#endif

}
