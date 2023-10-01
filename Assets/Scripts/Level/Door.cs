using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Collider2D finishLevelTrigger;

    protected virtual void Start()
    {
        finishLevelTrigger.enabled = false;
    }

    public void OpenDoor()
    {
        finishLevelTrigger.enabled = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GetComponent<SceneLoader>().LoadNext();
    }
}
