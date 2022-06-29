#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectGridSnapEditor : MonoBehaviour
{
    [SerializeField] private bool xOffset = false;
    [SerializeField] private bool yOffset = false;

    private Vector3 oldPosition;

    // Update is called once per frame
    void Update()
    {
        if (oldPosition == this.transform.position) return;
        if(xOffset && yOffset)
            this.transform.position = TransformGrid.WorldPositionToGridOffset(this.transform.position);
        else if (xOffset)
            this.transform.position = TransformGrid.WorldPositionToGridXOffset(this.transform.position);
        else if (yOffset)
            this.transform.position = TransformGrid.WorldPositionToGridYOffset(this.transform.position);
        else
            this.transform.position = TransformGrid.WorldPositionToGrid(this.transform.position);
        oldPosition = this.transform.position;
    }
}
#endif