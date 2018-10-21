using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateIdentifier;

public class SendCheckInformation : MonoBehaviour
{
    private State state = State.CHECK;
    public InputField[] inputFields;
    public Dropdown operatorDropdown;
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void SendInformation()
    {
        Instruction instruction = new Instruction
        {
            state = state,
            checkLetter = gameController.ReturnIntFromLetter(inputFields[0].text),
            jumpTo = int.Parse(inputFields[1].text),
            checkOperator = (Instruction.Operator)operatorDropdown.value,
            checkCount = inputFields[3].text
        };
        //gameController.AddSpecificInstruction(instruction);
    }
}
