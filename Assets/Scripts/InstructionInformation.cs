using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionInformation : MonoBehaviour
{
    public Instruction instruction;
    public InputField jumpToInputField;
    public InputField checkLetterInputField;
    public Dropdown checkOperaterDropdown;
    public InputField checkCountInputField;
    private GameController gameController;
    public GameObject lightIcon;
    public GameObject closeButton;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Start()
    {
        if (jumpToInputField != null)
            SetJumpTo();
        if (checkLetterInputField != null)
            SetCheckLetter();
        if (checkOperaterDropdown != null)
            SetCheckOperator();
        if (checkCountInputField != null)
            SetCheckCount();

        GameObject light = Instantiate(lightIcon, transform);
        light.GetComponent<RectTransform>().anchoredPosition = new Vector2(-30.0f, 0.0f);
        GameObject close = Instantiate(closeButton, transform);
        close.GetComponent<RectTransform>().anchoredPosition = new Vector2(30.0f, 0.0f);
        close.GetComponent<Button>().onClick.AddListener(() => { gameController.RemoveInstructionFromProgram(instruction); Destroy(gameObject); });
    }

    public void SetJumpTo()
    {
        instruction.jumpTo = int.Parse(jumpToInputField.text);
    }

    public void SetCheckLetter()
    {
        instruction.checkLetter = gameController.ReturnIntFromLetter(checkLetterInputField.text);
    }

    public void SetCheckOperator()
    {
        instruction.checkOperator = (Instruction.Operator)checkOperaterDropdown.value;
    }

    public void SetCheckCount()
    {
        instruction.checkCount = checkCountInputField.text;
    }

    public void SetInstruction(Instruction instruction)
    {
        this.instruction = instruction;
        switch (instruction.state)
        {
            case StateIdentifier.State.BUMP:
                jumpToInputField.text = instruction.jumpTo.ToString();
                break;
            case StateIdentifier.State.JUMP:
                jumpToInputField.text = instruction.jumpTo.ToString();
                break;
            case StateIdentifier.State.COUNT:
                checkLetterInputField.text = gameController.ReturnLetterFromInt(instruction.checkLetter);
                break;
            case StateIdentifier.State.CHECK:
                jumpToInputField.text = instruction.jumpTo.ToString();
                checkOperaterDropdown.value = (int)instruction.checkOperator;
                checkCountInputField.text = instruction.checkCount;
                checkLetterInputField.text = gameController.ReturnLetterFromInt(instruction.checkLetter);
                break;
        }
    }
}
