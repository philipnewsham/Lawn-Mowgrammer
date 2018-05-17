using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendCheckInformation : MonoBehaviour
{
    private StateIdentifier.State state = StateIdentifier.State.CHECK;
    public InputField[] inputFields;
    public Dropdown operatorDropdown;
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void SendInformation()
    {
        Instruction instruction = new Instruction();
        instruction.state = state;
        instruction.checkLetter = gameController.ReturnIntFromLetter(inputFields[0].text);
        instruction.jumpTo = int.Parse(inputFields[1].text);
        instruction.checkOperator = (Instruction.Operator)operatorDropdown.value;
        instruction.checkCount = int.Parse(inputFields[3].text);
        gameController.AddSpecificInstruction(instruction);
    }
}
