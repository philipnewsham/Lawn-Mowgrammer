using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGarden : MonoBehaviour
{
    private int x;
    private int z;
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    public GameObject block;
    private List<int> possibleMoves = new List<int>();
    private List<GameObject> blocks = new List<GameObject>();
    private bool isBusy;

    void Start()
    {
        x = Random.Range(3, 11);
        z = Random.Range(3, 7);
        currentPosition = Vector3.zero;
        previousPosition = Vector3.zero;
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        StartCoroutine(CreatePath());
        yield return new WaitWhile(() => isBusy);
        Debug.Log("end");
    }

    IEnumerator CreatePath()
    {
        isBusy = true;
        while (currentPosition != new Vector3(x, 0.0f, z))
        {
            Debug.LogFormat("current position = {0}, end position = {1}", currentPosition, new Vector3(x, 0.0f, z));
            possibleMoves.Clear();
            if (currentPosition.z < z && currentPosition.z + 1 != previousPosition.z)
                possibleMoves.Add(0);
            if (currentPosition.x < x)
                possibleMoves.Add(1);
            if (currentPosition.z > 0 && currentPosition.z - 1 != previousPosition.z && currentPosition.x < x)
                possibleMoves.Add(2);

            int direction = possibleMoves[Random.Range(0, possibleMoves.Count)];
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
            yield return new WaitForSeconds(0.2f);
        }
        isBusy = false;
    }

    IEnumerator FillEmptySpaces()
    {
        //find empty spaces
        //check empty space for adjacent grass blocks
        yield return null;
    }

    public Material[] materials;
    void CreateBlock(Vector3 position, FloorType floorType)
    {
        GameObject blockClone = Instantiate(block, position, Quaternion.identity);
        blockClone.GetComponent<MeshRenderer>().material = materials[(int)floorType];
        blocks.Add(blockClone);
    }
}
