using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSetGarden : MonoBehaviour
{
    public List<Vector2> positions;
    private GenerateGarden generateGarden;
    private int x = 0;
    private int z = 0;

	void Start ()
    {
        generateGarden = FindObjectOfType<GenerateGarden>();
        generateGarden.CreateBlock(Vector3.zero, FloorType.MOWED);
        foreach (Vector2 pos in positions)
        {
            int posX = Mathf.FloorToInt(pos.x);
            int posZ = Mathf.FloorToInt(pos.y);
            Debug.Log(posZ);
            x = posX > x ? posX : x;
            z = posZ > z ? posZ : z;
            generateGarden.CreateBlock(new Vector3(pos.x, 0.0f, pos.y), FloorType.UNMOWED);
        }

        generateGarden.StartBoundaryWalls(x, z);
	}
}
