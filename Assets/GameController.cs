using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum State
    {
        FORWARD,
        MOW,
        JUMP,
        BUMP,
        ROTATE,
        STOP
    }

    public List<State> programme;
    private bool busy = false;

	void Start ()
    {
        StartCoroutine(StartProgram());
	}
	
    IEnumerator StartProgram()
    {
        for (int i = 0; i < programme.Count; i++)
        {
            switch (programme[i])
            {
                case State.FORWARD:
                    transform.localPosition += transform.forward;
                    Debug.Log("Forward!");
                    break;
                case State.MOW:
                    Debug.Log("Mow!");
                    break;
                case State.JUMP:
                    break;
                case State.BUMP:
                    break;
                case State.ROTATE:
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 90.0f, transform.localEulerAngles.z);
                    Debug.Log("Rotate!");
                    break;
                case State.STOP:
                    break;
            }
            yield return new WaitForSeconds(1.0f);
            yield return new WaitWhile(() => busy);
        }
    }
}
