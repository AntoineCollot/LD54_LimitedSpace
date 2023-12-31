using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : Door
{
    public ScriptableBool openBool;
    public enum State { Closed, Opening, Open }
    public Direction direction;
    State state;

    Animator anim;
    public float openingTime = 1;
    public float openingDelay = 1.3f;

    protected override void Start()
    {
        base.Start();

        openBool.value = false;
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("Direction", (int)direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (openBool.value && state == State.Closed)
            TriggerOpening();
    }

    public void TriggerOpening()
    {
        if (state != State.Closed)
            return;

        state = State.Opening;
        StartCoroutine(OpenAnim());
    }

    IEnumerator OpenAnim()
    {
        yield return new WaitForSeconds(openingDelay);

        anim.SetBool("IsOpen", true);
        SFXManager.PlaySound(GlobalSFX.DoorOpen);

        yield return new WaitForSeconds(openingTime);

        state = State.Open;
        OpenDoor();
    }
}
