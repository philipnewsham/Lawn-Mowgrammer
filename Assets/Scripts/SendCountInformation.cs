using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendCountInformation : MonoBehaviour
{
    private StateIdentifier.State state = StateIdentifier.State.COUNT;
    public InputField inputFields;
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void SendInformation()
    {
        Instruction instruction = new Instruction();
        instruction.state = state;
        instruction.checkLetter = gameController.ReturnIntFromLetter(inputFields.text);
        //gameController.AddSpecificInstruction(instruction);
    }
}
