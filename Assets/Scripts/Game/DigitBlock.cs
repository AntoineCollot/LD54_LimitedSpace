using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitBlock : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;
    SpriteRenderer spriteRenderer;
    DigitGroup group;

    public int digit;
    public bool isOn;
    public bool freezeValue;
    public bool freezeOnceTurnedOn;

    // Start is called before the first frame update
    void Start()
    {
        Health health = GetComponent<Health>();
        health.onHit.AddListener(OnHit);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        group = GetComponentInParent<DigitGroup>();

        UpdateSprites();
    }

    private void OnHit()
    {
        if (GameManager.Instance.gameIsOver || freezeValue)
            return;

        Switch();
    }
       
    public void Switch()
    {
        isOn = !isOn;
        UpdateSprites();

        if (freezeOnceTurnedOn && isOn)
            freezeValue = true;

        group.OnDigitUpdated(this);
    }

    void UpdateSprites()
    {
        if (isOn)
            spriteRenderer.sprite = onSprite;
        else
            spriteRenderer.sprite = offSprite;

    }
}
