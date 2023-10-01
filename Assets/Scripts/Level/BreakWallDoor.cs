using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWallDoor : Door
{
    public Sprite openWallSprite;

    protected override void Start()
    {
        base.Start();

        GetComponent<Health>().onDie.AddListener(OnDie);
    }

    private void OnDie()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = openWallSprite;
        GetComponentInChildren<ParticleSystem>().Play();
        Invoke("OpenDoor",1.5f);
        SFXManager.PlaySound(GlobalSFX.WallOpen);
    }
}
