using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;
    private float inputValue;
    private string enteredValue;
    public InputField input = null;
    public GameObject keypad;
    public GameObject root = null;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = this;
        enteredValue = string.Empty;
        inputValue = 0;
        keypad.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        input.GetComponentInChildren<Text>().text = enteredValue.ToString();
        input.text = "";
    }

    public void convertValue()
    {
        try
        {
            float.TryParse(enteredValue, out inputValue);
            if (input != null)
            {
                input.GetComponentInChildren<Text>().text = inputValue.ToString();
            }
            input.GetComponentInChildren<Text>().color = Color.green;
            keypad.SetActive(false);
            UserInputManager.manager.actvieAll();
        }
        catch
        {
            if (input != null)
            {
                input.GetComponentInChildren<Text>().text = "Error!";
            }
        }
    }

    public float getInputValue()
    {
        return inputValue;
    }

    public void openKeypad()
    {
        try
        {
            keypad.SetActive(true);
            input.GetComponentInChildren<Text>().color = Color.red;
            UserInputManager.manager.selectedField(root.name);
        }
        catch
        {
            Debug.Log("Can't find keypad");
        }
    }

    #region Input
    public void clickZero()
    {
        enteredValue += "0";
    }

    public void clickOne()
    {
        enteredValue += "1";
    }

    public void clickTwo()
    {
        enteredValue += "2";
    }

    public void clickThree()
    {
        enteredValue += "3";
    }

    public void clickFour()
    {
        enteredValue += "4";
    }

    public void clickFive()
    {
        enteredValue += "5";
    }

    public void clickSix()
    {
        enteredValue += "6";
    }

    public void clickSeven()
    {
        enteredValue += "7";
    }

    public void clickEight()
    {
        enteredValue += "8";
    }

    public void clickNine()
    {
        enteredValue += "9";
    }

    public void clickClear()
    {
        enteredValue = string.Empty;
    }

    public void clickDot()
    {
        enteredValue += ".";
    }
    #endregion
}
