using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitGroup : MonoBehaviour
{
    public int targetValue;
    int currentDigitValue;
    public ScriptableBool outputBool;
    DigitBlock[] digits;

    private void Start()
    {
        digits = GetComponentsInChildren<DigitBlock>();
    }

    public void OnDigitUpdated(DigitBlock digitBlock)
    {
        if (CheckCompleted())
        {
            outputBool.value = true;
            FreezeDigits(true);
        }
    }

    bool CheckCompleted()
    {
        currentDigitValue = 0;
        foreach (DigitBlock digit in digits)
        {
            if (digit.isOn)
                currentDigitValue |= 1 << digit.digit;
        }

        return currentDigitValue == targetValue;
    }

    void FreezeDigits(bool value)
    {
        foreach (DigitBlock digit in digits)
        {
            digit.freezeValue = value;
        }
    }

    public bool GetBit(int from, int bitID)
    {
         return (from & (1 << bitID - 1)) != 0;
    }
}
