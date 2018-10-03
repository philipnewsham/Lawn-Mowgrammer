using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController controller;
    public string seedName;

    private void Awake()
    {
        if(controller == null)
        {
            controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNewGarden(string seedName)
    {
        this.seedName = seedName;
        SceneManager.LoadScene("Test");
    }
}
