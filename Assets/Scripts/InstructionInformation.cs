﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionInformation : MonoBehaviour
{
    public Instruction instruction;
    public InputField jumpToInputField;
    public InputField checkLetterInputField;
    public Dropdown checkOperaterDropdown;
    public InputField checkCountInputField;
    private GameController gameController;
    public GameObject lightIcon;
    public GameObject closeButton;
    private Image instructionLight;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Start()
    {
        if (instruction.state == StateIdentifier.State.JUMP)
            CreateJumpArrow();
        
        if (jumpToInputField != null)
            SetJumpTo();
        if (checkLetterInputField != null)
            SetCheckLetter();
        if (checkOperaterDropdown != null)
            SetCheckOperator();
        if (checkCountInputField != null)
            SetCheckCount();

        GameObject light = Instantiate(lightIcon, transform);
        light.GetComponent<RectTransform>().anchoredPosition = new Vector2(-30.0f, 0.0f);
        instructionLight = light.GetComponent<Image>();
        GameObject close = Instantiate(closeButton, transform);
        close.GetComponent<RectTransform>().anchoredPosition = new Vector2(30.0f, 0.0f);
        close.GetComponent<Button>().onClick.AddListener(() => { gameController.RemoveInstructionFromProgram(instruction); Destroy(gameObject); });
    }

    public GameObject arrow;
    private RectTransform jumpArrow;

    void CreateJumpArrow()
    {
        jumpArrow = Instantiate(arrow, transform).GetComponent<RectTransform>();

        ArrowInfo info = ArrowController.controller.ReturnInfo();
        jumpArrow.anchoredPosition = new Vector2(GetComponent<RectTransform>().sizeDelta.x + 30.0f + info.distance, -8.0f);
        jumpArrow.sizeDelta = new Vector2(8.0f, 16.0f);
        jumpArrow.GetComponent<Image>().color = info.color;
    }

    void UpdateArrowLength()
    {
        //scale = -1 when target is above
        int target = instruction.jumpTo;
        int current = gameController.ReturnOrderNoFromInstruction(instruction);
        bool upsideDown = target < current;
        jumpArrow.localScale = new Vector2(1, upsideDown ? -1 : 1);
        jumpArrow.anchoredPosition = new Vector2(jumpArrow.anchoredPosition.x, upsideDown ? -24.0f : -8.0f);
        jumpArrow.sizeDelta = new Vector2(jumpArrow.sizeDelta.x, 15.0f + (Mathf.Abs(current - target) * 30.0f));
    }

    public void SetJumpTo()
    {
        instruction.jumpTo = int.Parse(jumpToInputField.text);
        UpdateArrowLength();
    }

    public void SetCheckLetter()
    {
        instruction.checkLetter = gameController.ReturnIntFromLetter(checkLetterInputField.text);
    }

    public void SetCheckOperator()
    {
        instruction.checkOperator = (Instruction.Operator)checkOperaterDropdown.value;
    }

    public void SetCheckCount()
    {
        instruction.checkCount = checkCountInputField.text;
    }

    public void SetInstruction(Instruction instruction)
    {
        this.instruction = instruction;
        switch (instruction.state)
        {
            case StateIdentifier.State.BUMP:
                jumpToInputField.text = instruction.jumpTo.ToString();
                break;
            case StateIdentifier.State.JUMP:
                jumpToInputField.text = instruction.jumpTo.ToString();
                break;
            case StateIdentifier.State.COUNT:
                checkLetterInputField.text = gameController.ReturnLetterFromInt(instruction.checkLetter);
                break;
            case StateIdentifier.State.CHECK:
                jumpToInputField.text = instruction.jumpTo.ToString();
                checkOperaterDropdown.value = (int)instruction.checkOperator;
                checkCountInputField.text = instruction.checkCount;
                checkLetterInputField.text = gameController.ReturnLetterFromInt(instruction.checkLetter);
                break;
        }
    }

    private bool isLit = false;
    public void LightColor()
    {
        isLit = !isLit;
        Debug.Log(isLit);
        instructionLight.color = isLit ? Color.yellow : Color.white;
    }
}
