using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DragInstruction : MonoBehaviour
{
    private Button button;
    public GameObject instruction;
    private RectTransform rectTransformer;
    private RectTransform rectClone;
    private bool onDrag;
    private Vector3 localPos;
    private GameController gameController;

    public StateIdentifier.State state;

	void Start ()
    {
        rectTransformer = GetComponent<RectTransform>();
        gameController = FindObjectOfType<GameController>();
	}
	
	public void SpawnInstruction ()
    {
        gameController.SetDrag(true);
        Instruction thisInstruction = new Instruction();
        thisInstruction.state = state;
        gameController.SetCurrentInstruction(thisInstruction);
        GameObject instructionClone = Instantiate(instruction, transform.root);
        instructionClone.GetComponentInChildren<Text>().text = gameController.GetCurrentInstruction().state.ToString();
        rectClone = instructionClone.GetComponent<RectTransform>();
        rectClone.anchoredPosition = Vector2.zero;
        localPos = rectClone.localPosition;
        onDrag = true;
	}

    void Update()
    {
        if (!onDrag)
            return;

        rectClone.localPosition = Input.mousePosition - new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            onDrag = false;
            Destroy(rectClone.gameObject);
            rectClone = null;
            gameController.SetDrag(false);
        }
    }
}
