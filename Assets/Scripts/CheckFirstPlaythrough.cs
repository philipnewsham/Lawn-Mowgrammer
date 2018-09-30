using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFirstPlaythrough : MonoBehaviour
{
    public GameObject infoBox;
	void Start ()
    {
        if (!PlayerPrefs.HasKey("Played"))
        {
            PlayerPrefs.SetInt("Played", 1);
            infoBox.SetActive(true);
        }	
	}
}
