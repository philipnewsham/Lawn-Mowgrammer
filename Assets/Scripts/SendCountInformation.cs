using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateIdentifier;

public class SendCountInformation : MonoBehaviour
{
    private readonly State state = State.COUNT;
    public InputField inputFields;
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
            checkLetter = gameController.ReturnIntFromLetter(inputFields.text)
        };
    }
}
