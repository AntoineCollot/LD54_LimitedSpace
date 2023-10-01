using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ally : MonoBehaviour
{
    Animator anim;
    Health health;
    public GameObject speachBubble;
    public float speachAreaRadius = 3;
    public TextMeshProUGUI text;
    string baseText;

    static char[] randomSymbols = new char[] { '/', '.', ',', '^', '%', 'u', 'o', 'x', '!', '?', '|', '_', '-', '$', '(', '}', '[', 'é', '~',' ','>'};

    public bool alwaysTranslate = false;
    bool currentTranslate = false;
    public static bool translateToEnglish = false;
    bool ShouldTranslate => translateToEnglish || alwaysTranslate;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.speed = 1 / 6f;
        health = GetComponent<Health>();
        health.onDie.AddListener(OnDie);
        health.onHit.AddListener(OnHit);

        baseText = text.text;
        text.text = EncodeText(baseText);
        speachBubble.SetActive(false);
    }

    string EncodeText(string str)
    {
        string encoded = "";
        bool isProcessingNumbers = false;
        int symbolEncodedCount = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsNumber(str[i]))
            {
                if(!isProcessingNumbers)
                {
                    encoded += " <b><color=#88AAFFFF>";
                    isProcessingNumbers = true;
                }
                encoded += str[i];
            }
            else
            {
                if (isProcessingNumbers)
                {
                    encoded += "</color></b> ";
                    isProcessingNumbers = false;
                }

                symbolEncodedCount++;
                if(symbolEncodedCount%2 == 1)
                    encoded += randomSymbols[UnityEngine.Random.Range(0, randomSymbols.Length)];
            }
        }

        return encoded;
    }

    private void Update()
    {
       bool isPlayerClose = Vector2.Distance(PlayerState.Instance.CenterOfMass, transform.position) <= speachAreaRadius;
       speachBubble.SetActive(isPlayerClose&& !health.isDead);

        //on translation state changed
        if(currentTranslate != ShouldTranslate)
        {
            if (ShouldTranslate)
                text.text = baseText;
            else
                text.text = EncodeText(baseText);

            currentTranslate = ShouldTranslate;
        }
    }

    private void OnHit()
    {
        SFXManager.PlaySound(GlobalSFX.EnemyDamaged);
    }

    private void OnDie()
    {
        anim.SetBool("IsDead", true);
        ExplosionProvider.Instance.SpawnEnemyDeath(transform.position);
        SFXManager.PlaySound(GlobalSFX.EnemyKilled);
        GetComponentInChildren<SpriteRenderer>().sortingOrder = -3; 
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, speachAreaRadius);
    }
#endif
}
