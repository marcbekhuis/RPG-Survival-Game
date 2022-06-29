using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class CraftingList : Singleton<CraftingList>
{
    [SerializeField] private bool update = false;

    [HideInInspector] public List<CraftingItem> craftingItems = new List<CraftingItem>();

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            craftingItems.Clear();
            foreach (var guid in AssetDatabase.FindAssets("t:" + typeof(CraftingItem).Name))
            {
                craftingItems.Add(AssetDatabase.LoadAssetAtPath<CraftingItem>(AssetDatabase.GUIDToAssetPath(guid)));
            }
            update = false;
        }
    }
#endif
}
