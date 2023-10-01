using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleTypewritting : MonoBehaviour
{
    public float startDelay = 1;
    public float timePerChar = 0.1f;
    public bool erase = true;
    public float displayDuraction = 2;

    private void Start()
    {
        StartCoroutine(Typewritting());
    }

    IEnumerator Typewritting()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.maxVisibleCharacters = 0;

        yield return new WaitForSeconds(startDelay);

        for (int i = 1; i <= text.text.Length; i++)
        {
            text.maxVisibleCharacters = i;

            yield return new WaitForSeconds(timePerChar);
        }

        if(!erase)
            yield break;

        yield return new WaitForSeconds(displayDuraction);

        for (int i = text.text.Length; i>=0; i--)
        {
            text.maxVisibleCharacters = i;
            yield return new WaitForSeconds(timePerChar / 3);
        }
    }
}
