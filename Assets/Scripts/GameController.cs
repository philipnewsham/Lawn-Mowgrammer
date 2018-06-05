using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Instruction
{
    public enum Operator
    {
        EQUALS,     //==
        LESS_THAN,  //<
        MORE_THAN,  //>
        EQUAL_LESS, //<=
        EQUAL_MORE  //>=
    }

    public StateIdentifier.State state;
    public int jumpTo;
    public int checkLetter;
    public Operator checkOperator;
    public string checkCount;
}


public class StateIdentifier
{
    public enum State
    {
        FORWARD,
        ROTATE,
        JUMP,
        BUMP,
        COUNT,
        CHECK,
        STOP
    }
}

[System.Serializable]
public class GameController : MonoBehaviour
{
    private List<Instruction> program = new List<Instruction>();
    private GardenController gardenController;
    private bool busy = false;
    private bool bumped = false;
    private int[] values = new int[26];
    private string[] letters = new string[26] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    public GameObject textInstruction;
    public RectTransform textParent;
    private List<Text> textInstructions = new List<Text>();

    private Transform lawnMower;

    private bool dragInstruction;
    private Instruction currentInstruction;

    public bool GetDrag()
    {
        return dragInstruction;
    }

    public void SetDrag(bool drag)
    {
        dragInstruction = drag;
    }

    public void SetCurrentInstruction(Instruction instruction)
    {
        currentInstruction = instruction;
    }

    public Instruction GetCurrentInstruction()
    {
        return currentInstruction;
    }

    void Start()
    {
        lawnMower = GameObject.FindGameObjectWithTag("Player").transform;
        gardenController = FindObjectOfType<GardenController>();
        program = SaveController.Load();
        if (program.Count > 0)
        {
            UpdateProgramString();
            UpdateInstructionList();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ClearProgram();
    }

    void ClearProgram()
    {
        ClearShownInstructions();
        textInstructions.Clear();
        program.Clear();
        SaveController.Save(program);
    }

    void ClearShownInstructions()
    {
        foreach (Text instructions in textInstructions)
            Destroy(instructions.gameObject);

        foreach (Transform child in panelParent)
            Destroy(child.gameObject);
    }

    void UpdateProgramString()
    {
        foreach (Instruction instruction in program)
            AddStringInstruction(instruction);
    }
    public GameObject[] instructionButton;

    void UpdateInstructionList()
    {
        foreach (Instruction instruction in program)
        {
            GameObject instructionClone = Instantiate(instructionButton[(int)instruction.state], panelParent);
            instructionClone.GetComponent<InstructionInformation>().instruction = instruction;
        }
    }

    public void StartProgramme()
    {
        foreach (Text text in textInstructions)
            Destroy(text.gameObject);
        textInstructions.Clear();
        program.Clear();

        foreach (Transform child in panelParent)
        {
            program.Add(child.GetComponent<InstructionInformation>().instruction);
            AddStringInstruction(child.GetComponent<InstructionInformation>().instruction);
        }

        SaveController.Save(program);
        StartCoroutine(StartProgram(program));
    }

    IEnumerator StartProgram(List<Instruction> stateProgramme)
    {
        SaveController.Save(program);
        for (int i = 0; i < stateProgramme.Count; i++)
        {
            for (int j = 0; j < textInstructions.Count; j++)
                textInstructions[j].color = (j == i) ? Color.red : Color.black;

            switch (stateProgramme[i].state)
            {
                case StateIdentifier.State.FORWARD:
                    Vector3 position = lawnMower.localPosition + lawnMower.forward;
                    if (!gardenController.ForwardBump(position))
                    {
                        lawnMower.localPosition = lawnMower.localPosition += lawnMower.forward;
                        gardenController.MowLawn(position);
                    }
                    else
                        bumped = true;
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
                    lawnMower.localEulerAngles = new Vector3(lawnMower.localEulerAngles.x, lawnMower.localEulerAngles.y + 90.0f, lawnMower.localEulerAngles.z);
                    break;
                case StateIdentifier.State.COUNT:
                    values[stateProgramme[i].checkLetter]++;
                    Debug.Log(values[stateProgramme[i].checkLetter]);
                    break;
                case StateIdentifier.State.CHECK:
                    Debug.LogFormat("if {0} {1} {2} jump {3}", values[stateProgramme[i].checkLetter], operatorStrings[(int)stateProgramme[i].checkOperator], ReturnValue(stateProgramme[i].checkCount), stateProgramme[i].jumpTo);
                    if(CheckCondition(stateProgramme[i].checkOperator, ReturnValue(stateProgramme[i].checkCount), values[stateProgramme[i].checkLetter]))
                        i = stateProgramme[i].jumpTo - 1;
                    break;
                case StateIdentifier.State.STOP:
                    i = stateProgramme.Count;
                    break;
            }
            yield return new WaitForSeconds(1.0f);
            yield return new WaitWhile(() => busy);
        }
        Debug.Log("programme ended");
        EndProgram();
    }

    public GameObject endScreen;

    void EndProgram()
    {
        if(gardenController.ProgramComplete())
            Instantiate(endScreen, transform);
    }

    int ReturnValue(string value)
    {
        Debug.LogFormat("checking");
        for (int i = 0; i < letters.Length; i++)
        {
            if (value == letters[i])
                return values[i];
        }

        return int.Parse(value);
    }

    public void AddGenericInstruction(StateIdentifier.State state, int jumpTo)
    {
        Instruction instruction = new Instruction();
        instruction.state = state;
        instruction.jumpTo = jumpTo;
        AddSpecificInstruction(instruction);
    }

    public void AddSpecificInstruction(Instruction instruction)
    {
        AddStringInstruction(instruction);
        program.Add(instruction);
    }

    public Transform panelParent;
    
    public int ReturnIntFromLetter(string letter)
    {
        for (int i = 0; i < letters.Length; i++)
        {
            if (letter == letters[i])
                return i;
        }
        Debug.LogWarningFormat("{0} is not a possible type! Returning 0");
        return 0;
    }

    private List<string> operatorStrings = new List<string>()
    {
        "==",
        "<",
        ">",
        "<=",
        ">="
    };
    
    void AddStringInstruction(Instruction instruction)
    {
        GameObject textClone = Instantiate(textInstruction, textParent);
        Text textBox = textClone.GetComponent<Text>();
        textBox.text = textBox.text + string.Format("{0} {1} ", program.Count, instruction.state.ToString());
        if (instruction.state == StateIdentifier.State.JUMP || instruction.state == StateIdentifier.State.BUMP)
            textBox.text += string.Format("to {0}", instruction.jumpTo);
        if (instruction.state == StateIdentifier.State.COUNT)
            textBox.text += string.Format("{0}++", letters[instruction.checkLetter]);
        if (instruction.state == StateIdentifier.State.CHECK)
            textBox.text += string.Format("[{0} {1} {2}] to {3}", letters[instruction.checkLetter], operatorStrings[(int)instruction.checkOperator], instruction.checkCount, instruction.jumpTo);
        textInstructions.Add(textClone.GetComponent<Text>());
    }

    bool CheckCondition(Instruction.Operator operatorType, int checkValue, int currentValue)
    {
        switch (operatorType)
        {
            case Instruction.Operator.EQUALS:
                return currentValue == checkValue;
            case Instruction.Operator.EQUAL_LESS:
                return currentValue <= checkValue;
            case Instruction.Operator.EQUAL_MORE:
                return currentValue >= checkValue;
            case Instruction.Operator.LESS_THAN:
                return currentValue < checkValue;
            case Instruction.Operator.MORE_THAN:
                return currentValue > checkValue;
            default:
                return false;
        }
    }
}
