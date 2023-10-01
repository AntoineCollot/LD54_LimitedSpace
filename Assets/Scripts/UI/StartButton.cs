using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        RAMManager.Instance.ResetAll();
        SFXManager.PlaySound(GlobalSFX.ButtonClick);
        GetComponent<SceneLoader>().LoadNext();
    }
}
