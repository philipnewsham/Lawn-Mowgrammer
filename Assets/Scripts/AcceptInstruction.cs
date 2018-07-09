using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcceptInstruction : MonoBehaviour
{
    public GameObject instruction;
    private GameObject currentInstruction;
    bool hasInstruction;
    private GameController gameController;

    public GameObject[] instructionPrefabs;
    private SizeToChildren sizeToChildren;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        sizeToChildren = GetComponent<SizeToChildren>();
    }

    public void OnHover()
    {
        if (FindObjectOfType<GameController>().GetDrag())
        {
            hasInstruction = true;
            currentInstruction = Instantiate(instruction, transform);
            currentInstruction.GetComponent<InstructionInformation>().instruction = gameController.GetCurrentInstruction();
            currentInstruction.GetComponentInChildren<Text>().text = gameController.GetCurrentInstruction().state.ToString();
            currentInstruction.GetComponent<Button>().interactable = false;
        }
    }

    public void ExitHover()
    {
        hasInstruction = false;
        if (FindObjectOfType<GameController>().GetDrag())
        {
            Destroy(currentInstruction);
            currentInstruction = null;
        }
        else
        {
            if (currentInstruction != null)
                MouseUp();
        }
    }

    void Update()
    {
        if (!hasInstruction)
            return;

        if (Input.GetKeyUp(KeyCode.Mouse0))
            MouseUp();
    }

    public void MouseUp()
    {
        if (gameController.GetCurrentInstruction() != null)
        {
            GameObject clone = Instantiate(instructionPrefabs[(int)gameController.GetCurrentInstruction().state], transform);
            clone.GetComponent<InstructionInformation>().instruction = gameController.GetCurrentInstruction();
            Destroy(currentInstruction);
            currentInstruction = null;
            hasInstruction = false;
            gameController.SetCurrentInstruction(null);
            sizeToChildren.UpdateHeight();
        }
    }
}
