using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendButtonInformation : MonoBehaviour
{
    public StateIdentifier.State state;
    public InputField inputField;

    public void SendInformation()
    {
        int jumpTo = int.Parse(inputField.text);
        //FindObjectOfType<GameController>().AddGenericInstruction(state, jumpTo);
    }
}
