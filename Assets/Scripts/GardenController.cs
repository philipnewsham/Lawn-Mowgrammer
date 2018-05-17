using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenType
{
    public FloorType floorType;
    public Vector3 position;
}

public enum FloorType
{
    MOWED,
    UNMOWED,
    BLOCKER
}

public class GardenController : MonoBehaviour
{
    private List<GardenType> gardenBlocks = new List<GardenType>();
    public Material[] materials;

	void Start ()
    {
		foreach(Transform child in transform)
        {
            GardenType gardenBlock = new GardenType();
            gardenBlock.position = child.position;
            Material mat = child.GetComponent<MeshRenderer>().sharedMaterials[0];
            if (mat == materials[0])
                gardenBlock.floorType = FloorType.MOWED;
            if (mat == materials[1])
                gardenBlock.floorType = FloorType.UNMOWED;
            if (mat == materials[2])
                gardenBlock.floorType = FloorType.BLOCKER;
            
            gardenBlocks.Add(gardenBlock);
        }
    }

    public bool ForwardBump(Vector3 targetPosition)
    {
        Vector3 position = new Vector3(targetPosition.x, -1.0f, targetPosition.z);
        foreach (GardenType block in gardenBlocks)
        {
            if (position == block.position)
            {
                Debug.Log(block.floorType.ToString());
                return block.floorType == FloorType.BLOCKER;
            }
        }
        return false;
    }
}
