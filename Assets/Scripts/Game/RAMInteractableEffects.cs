using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAMInteractableEffects : RAMInteractable
{
    ParticleSystem particles;

    protected override void Start()
    {
        base.Start();

        particles = GetComponentInParent<ParticleSystem>();
    }

    private void Update()
    {
        if(RAMManager.Instance.isInRAMMode)
            col2D.enabled = particles.particleCount > 0;
    }
}
