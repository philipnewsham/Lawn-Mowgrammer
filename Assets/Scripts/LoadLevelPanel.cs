using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelPanel : MonoBehaviour
{
    public InputField seedInput;
    public Button loadButton;

    private void Start()
    {
        loadButton.onClick.AddListener(() => SceneController.controller.LoadNewGarden(seedInput.text));
    }
}
