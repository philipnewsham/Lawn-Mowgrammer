using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateGarden : MonoBehaviour
{
    private int x;
    private int y = 0;
    private int z;
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    public GameObject block;
    private List<int> possibleMoves = new List<int>();
    private List<GardenType> blocks = new List<GardenType>();
    private bool isBusy;
    private bool gardenFilled = false;

    private float waitTime = 0.0f;

    public int seed;
    private System.Random rng;

    void Start()
    {
        if (seed == 0)
            seed = UnityEngine.Random.Range(0, int.MaxValue);

        rng = new System.Random(seed);

        x = rng.Next(3, 11);
        z = rng.Next(3, 7);
        FindObjectOfType<CameraController>().SetCamera(x, z);
        currentPosition = Vector3.zero;
        previousPosition = Vector3.zero;
        CreateBlock(Vector3.zero, FloorType.MOWED);
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        StartCoroutine(CreatePath());
        yield return new WaitWhile(() => isBusy);
        while (!gardenFilled)
        {
            StartCoroutine(FillEmptySpaces());
            yield return new WaitWhile(() => isBusy);
        }
        StartCoroutine(FillRemainingBlocker());
        yield return new WaitWhile(() => isBusy);
        StartCoroutine(CreateBoundaryWalls(x, z));
        yield return new WaitWhile(() => isBusy);

        SetGardenBlocks();
    }

    public void SetGardenBlocks()
    {
        FindObjectOfType<GardenController>().SetGardenBlocks(blocks);
    }

    IEnumerator CreatePath()
    {
        isBusy = true;
        while (currentPosition != new Vector3(x, y, z))
        {
            //Debug.LogFormat("current position = {0}, end position = {1}", currentPosition, new Vector3(x, y, z));
            possibleMoves.Clear();
            if (currentPosition.z < z && currentPosition.z + 1 != previousPosition.z)
                possibleMoves.Add(0);
            if (currentPosition.x < x)
                possibleMoves.Add(1);
            if (currentPosition.z > 0 && currentPosition.z - 1 != previousPosition.z && currentPosition.x < x)
                possibleMoves.Add(2);

            int direction = possibleMoves[rng.Next(0, possibleMoves.Count)];
            previousPosition = currentPosition;
            switch (direction)
            {
                case 0:
                    currentPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 1);
                    break;
                case 1:
                    currentPosition = new Vector3(currentPosition.x + 1, currentPosition.y, currentPosition.z);
                    break;
                case 2:
                    currentPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - 1);
                    break;
            }
            CreateBlock(currentPosition,FloorType.UNMOWED);
            yield return new WaitForSeconds(waitTime);
        }
        isBusy = false;
    }

    List<Vector3> blankPositions = new List<Vector3>();
    IEnumerator FillEmptySpaces()
    {
        isBusy = true;
        //find empty spaces
        FindEmptySpaces();
        if (blankPositions.Count > 0)
        {
            List<Vector3> nextLayerBlocks = new List<Vector3>(); //these are the blocks not next to the path
                                                                 //check if block is adjacent to any grass
            for (int i = 0; i < blankPositions.Count; i++)
            {
                if (!AdjacentToGrass(blankPositions[i]))
                    nextLayerBlocks.Add(blankPositions[i]);
            }

            if (nextLayerBlocks.Count < blankPositions.Count)
            {
                //remove blocks not next to anything
                foreach (Vector3 pos in nextLayerBlocks)
                    blankPositions.Remove(pos);

                foreach (Vector3 space in blankPositions)
                {
                    List<FloorType> floorTypes = new List<FloorType>() { FloorType.BLOCKER, FloorType.UNMOWED, FloorType.UNMOWED };
                    CreateBlock(space, floorTypes[rng.Next(0, floorTypes.Count)]);
                    yield return new WaitForSeconds(waitTime);
                }

                blankPositions.Clear();
            }
            else
            {
                gardenFilled = true;
                isBusy = false;
            }
        }
        else
        {
            gardenFilled = true;
        }

        isBusy = false;
    }

    void FindEmptySpaces()
    {
        blankPositions.Clear();
        for (int i = 0; i < x + 1; i++)
        {
            for (int j = 0; j < z + 1; j++)
            {
                if (!SpaceFilled(new Vector3(i, y, j)))
                    blankPositions.Add(new Vector3(i, y, j));
            }
        }
    }

    IEnumerator FillRemainingBlocker()
    {
        isBusy = true;
        FindEmptySpaces();
        
        if (blankPositions.Count > 0)
        {
            foreach (Vector3 position in blankPositions)
            {
                CreateBlock(position, FloorType.BLOCKER);
                yield return new WaitForSeconds(waitTime);
            }
        }
        isBusy = false;
    }

    public void StartBoundaryWalls(int maxX, int maxZ)
    {
        StartCoroutine(CreateBoundaryWalls(maxX, maxZ));
    }

    public IEnumerator CreateBoundaryWalls(int maxX, int maxZ)
    {
        isBusy = true;
        for (int i = 0; i < maxZ + 1; i++)
        {
            CreateBlock(new Vector3(-1, y, i), FloorType.BLOCKER);
            yield return new WaitForSeconds(waitTime);
        }

        for (int i = 0; i < maxX + 1; i++)
        {
            CreateBlock(new Vector3(i, y, maxZ + 1), FloorType.BLOCKER);
            yield return new WaitForSeconds(waitTime);
        }

        for (int i = 0; i < maxX + 1; i++)
        {
            CreateBlock(new Vector3(i, y, -1), FloorType.BLOCKER);
            yield return new WaitForSeconds(waitTime);
        }

        for (int i = 0; i < maxZ + 1; i++)
        {
            CreateBlock(new Vector3(maxX + 1, y, i), FloorType.BLOCKER);
            yield return new WaitForSeconds(waitTime);
        }
        isBusy = false;
    }

    bool AdjacentToGrass(Vector3 position)
    {
        GardenType gardenType = new GardenType();
        List<FloorType> checkFloorTypes = new List<FloorType>() { FloorType.MOWED, FloorType.UNMOWED };

        if (position.x > 0)
        {
            gardenType = ReturnGardenTypeFromPosition(position + Vector3.left);
            if (gardenType != null)
            {
                if (CheckIfHasFloorTypes(gardenType, checkFloorTypes))
                    return true;
            }
        }

        if(position.x < x)
        {
            gardenType = ReturnGardenTypeFromPosition(position - Vector3.left);
            if (gardenType != null)
            {
                if (CheckIfHasFloorTypes(gardenType, checkFloorTypes))
                    return true;
            }
        }

        if (position.z > 0)
        {
            gardenType = ReturnGardenTypeFromPosition(position - Vector3.forward);
            if (gardenType != null)
            {
                if (CheckIfHasFloorTypes(gardenType, checkFloorTypes))
                    return true;
            }
        }

        if (position.z < z)
        {
            gardenType = ReturnGardenTypeFromPosition(position + Vector3.forward);
            if (gardenType != null)
            {
                if (CheckIfHasFloorTypes(gardenType, checkFloorTypes))
                    return true;
            }
        }
        return false;
    }

    bool CheckIfHasFloorTypes(GardenType gardenType, List<FloorType> floorTypes)
    {
        foreach(FloorType floor in floorTypes)
        {
            if (gardenType.floorType == floor)
                return true;
        }
        return false;
    }

    FloorType ReturnFloorTypeFromPosition(Vector3 position)
    {
        foreach (GardenType block in blocks)
        {
            if (block.position == position)
                return block.floorType;
        }
        return FloorType.BLANK;
    }

    GardenType ReturnGardenTypeFromPosition(Vector3 position)
    {
        foreach (GardenType block in blocks)
        {
            if (block.position == position)
                return block;
        }
        return null;
    }

    bool SpaceFilled(Vector3 position)
    {
        foreach(GardenType block in blocks)
        {
            if (block.position == position)
                return true;
        }
        return false;
    }

    public Material[] materials;
    public void CreateBlock(Vector3 position, FloorType floorType)
    {
        GameObject blockClone = Instantiate(block, position, Quaternion.identity);
        blockClone.transform.SetParent(this.transform);
        GardenType gardenType = new GardenType();
        gardenType.position = position;
        gardenType.floorType = floorType;
        gardenType.meshRenderer = blockClone.GetComponent<MeshRenderer>();
        blockClone.GetComponent<MeshRenderer>().material = materials[(int)floorType];
        blocks.Add(gardenType);
    }
}