using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instruction
{
    public StateIdentifier.State state;
    public int jumpTo;
}

public class StateIdentifier
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
}

public class GameController : MonoBehaviour
{
    private List<Instruction> programme = new List<Instruction>();
    public GardenController gardenController;
    private bool busy = false;
    public Text textBox;
    private bool bumped = false;
	
    IEnumerator StartProgram(List<Instruction> stateProgramme)
    {
        for (int i = 0; i < stateProgramme.Count; i++)
        {
            switch (stateProgramme[i].state)
            {
                case StateIdentifier.State.FORWARD:
                    Vector3 position = transform.localPosition + transform.forward;
                    if (!gardenController.ForwardBump(position))
                    {
                        transform.localPosition = transform.localPosition += transform.forward;
                    }
                    else
                    {
                        bumped = true;
                    }
                    break;
                case StateIdentifier.State.MOW:
                    break;
                case StateIdentifier.State.JUMP:
                    Debug.LogFormat("jump to {0}",stateProgramme[i].jumpTo-1);
                    i = stateProgramme[i].jumpTo - 1;
                    break;
                case StateIdentifier.State.BUMP:
                    if(bumped)
                        i = stateProgramme[i].jumpTo - 1;
                    bumped = false;
                    break;
                case StateIdentifier.State.ROTATE:
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + 90.0f, transform.localEulerAngles.z);
                    break;
                case StateIdentifier.State.STOP:
                    i = stateProgramme.Count;
                    break;
            }
            yield return new WaitForSeconds(1.0f);
            yield return new WaitWhile(() => busy);
        }
        Debug.Log("programme ended");
    }

    public void AddForward()
    {
        AddGenericInstruction(StateIdentifier.State.FORWARD, 0);
    }

    public void AddRotate()
    {
        AddGenericInstruction(StateIdentifier.State.ROTATE, 0);
    }

    public void AddJump(int jumpTo)
    {
        AddGenericInstruction(StateIdentifier.State.JUMP, jumpTo);
    }

    public void AddStop()
    {
        AddGenericInstruction(StateIdentifier.State.STOP, 0);
    }

    public void AddGenericInstruction(StateIdentifier.State state, int jumpTo)
    {
        textBox.text = textBox.text + string.Format("\n{0} {1} ", programme.Count, state.ToString());
        if (state == StateIdentifier.State.JUMP || state == StateIdentifier.State.BUMP)
            textBox.text += string.Format("to {0}", jumpTo);

        Instruction instruction = new Instruction();
        instruction.state = state;
        instruction.jumpTo = jumpTo;
        programme.Add(instruction);
    }

    public void StartProgramme()
    {
        StartCoroutine(StartProgram(programme));
    }
}
