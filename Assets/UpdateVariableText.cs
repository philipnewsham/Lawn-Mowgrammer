using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct VariableInfo
{
    public string name;
    public int value;
}

public class UpdateVariableText : MonoBehaviour
{
    public Text variableText;

    public void UpdateText(List<VariableInfo> variables)
    {
        string message = "";

        foreach (VariableInfo item in variables)
        {
            string variableInfo = string.Format("{0} = {1}", item.name, item.value);
            message += variableInfo;
            message += ", ";
        }
        message = message.TrimEnd(',', ' ');
        variableText.text = message;
    }
}
