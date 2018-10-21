using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StateIdentifier;

public class SendButtonInformation : MonoBehaviour
{
    public State state;
    public InputField inputField;

    public void SendInformation()
    {
        int jumpTo = int.Parse(inputField.text);
        //FindObjectOfType<GameController>().AddGenericInstruction(state, jumpTo);
    }
}
