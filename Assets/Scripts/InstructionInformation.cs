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

    public void SetJumpTo()
    {
        instruction.jumpTo = int.Parse(jumpToInputField.text);
    }

    public void SetCheckLetter()
    {
        instruction.checkLetter = int.Parse(checkLetterInputField.text);
    }

    public void SetCheckOperator()
    {
        instruction.checkOperator = (Instruction.Operator)checkOperaterDropdown.value;
    }

    public void SetCheckCount()
    {
        instruction.checkCount = int.Parse(checkCountInputField.text);
    }
}
