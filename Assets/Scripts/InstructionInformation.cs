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
    }

    public void SetJumpTo()
    {
        instruction.jumpTo = int.Parse(jumpToInputField.text);
    }

    public void SetCheckLetter()
    {
        instruction.checkLetter = FindObjectOfType<GameController>().ReturnIntFromLetter(checkLetterInputField.text);
    }

    public void SetCheckOperator()
    {
        instruction.checkOperator = (Instruction.Operator)checkOperaterDropdown.value;
    }

    public void SetCheckCount()
    {
        instruction.checkCount = checkCountInputField.text;
    }
}
