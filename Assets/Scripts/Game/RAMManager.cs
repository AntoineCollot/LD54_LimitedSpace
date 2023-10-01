using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RAMManager : MonoBehaviour
{
    public delegate void OnRAMGroupEventHandler(RAMType type, RAMState state);
    public event OnRAMGroupEventHandler onRAMStateChanged;
    public delegate void OnRAMCountChangedEventHandler(int newRAMCount);
    public event OnRAMCountChangedEventHandler onRAMCountChanged;
    public delegate void OnRAMModeEventHandler(bool isOn);
    public event OnRAMModeEventHandler onRAMModeEnabled;

    int availableRAMCount = 0;
    public ScriptableRAMData ramData;
    InputMap inputMap;

    //RAM Mode
    public bool isInRAMMode { get; private set; }
    RAMGroup hoveredGroup;

    public static RAMManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        inputMap = new InputMap();
        inputMap.Enable();
        inputMap.Gameplay.UseRAM.performed += OnUseRAM;

        Time.timeScale = 1;

        Init();
    }

    private void Update()
    {
        if (isInRAMMode)
        {
            ProcessRAMMode(inputMap.Gameplay.Fire.WasPressedThisFrame());
            Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
        }

    }

    void Init()
    {
        foreach (RAMGroup group in ramData.ramGroups)
        {
            group.UpdateMaterials();
        }
    }

    void ProcessRAMMode(bool useRAM)
    {
        //Detect item hovered by cursor
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        //Try items first
        LayerMask mask = 1 << LayerMask.NameToLayer("RAM");
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray,1000, mask);
        if (hit.collider == null)
        {
            //Get Map if nothing else
            mask = 1 << LayerMask.NameToLayer("Level");
            hit = Physics2D.GetRayIntersection(ray, 1000, mask);
        }

        //if something is hovered
        if(hit.collider !=null)
        {
           Debug.Log("hover " + hit.collider.name);
            //Find the group the cursor is hovering
            RAMInteractable interactable = hit.collider.GetComponentInParent<RAMInteractable>();
            if (interactable == null)
            {
                SetState(hoveredGroup, RAMState.Locked);
                hoveredGroup = null;
                return;
            }

            RAMGroup group = ramData.GetRAMGroup(interactable.RAMType);
           Debug.Log("INteractable " + interactable.RAMType);

            //Process click
            if(useRAM)
            {
                SetState(group, RAMState.Unlocked);
                ExitRAMMode();
                return;
            }
            //Process hover
            if (hoveredGroup == group)
                return;

            //Reset the state of previous hovered group (can't change unlocked state anyway)
            SetState(hoveredGroup,RAMState.Locked);
            hoveredGroup = group;
            //Hover the new group
            SetState(hoveredGroup,RAMState.Hovered);
        }
        else
        {
            SetState(hoveredGroup, RAMState.Locked);
            hoveredGroup = null;
        }

        if (useRAM)
            ExitRAMMode();
    }

    public RAMState GetState(RAMType type)
    {
        return ramData.GetRAMGroup(type).state;
    }

    private void OnUseRAM(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<UIRAMCount>().button.gameObject);
        EnterRAMMode();
    }

    public bool SetState(RAMType type, RAMState state, bool allowDelocking = false)
    {
        RAMGroup group = ramData.GetRAMGroup(type);

        return SetState(group, state, allowDelocking);
    }

    public bool SetState(RAMGroup group, RAMState state, bool allowDelocking = false)
    {
        if (group == null)
            return false;
        if (!group.SetState(state, allowDelocking))
            return false;

        onRAMStateChanged.Invoke(group.type, state);
        return true;
    }

    public void FindRAM()
    {
        availableRAMCount++;
        onRAMCountChanged.Invoke(availableRAMCount);
    }

    public void EnterRAMMode()
    {
        if (availableRAMCount <= 0 || isInRAMMode)
            return;
        isInRAMMode = true;
        Time.timeScale = 0;
        onRAMModeEnabled.Invoke(true);
        return;
    }

    public void ExitRAMMode()
    {
        if (!isInRAMMode)
            return;
        isInRAMMode = false;
        Time.timeScale = 1;
        onRAMModeEnabled.Invoke(false);
        ResetAllHoveredStates();
    }

    public void UseRAM(RAMType type)
    {
        if (availableRAMCount <= 0)
            return;

        if (SetState(type, RAMState.Unlocked))
        {
            availableRAMCount--;
            onRAMCountChanged.Invoke(availableRAMCount);
        }
    }

    void ResetAllHoveredStates()
    {
        foreach (RAMGroup group in ramData.ramGroups)
        {
            //Set all states as locked, as by default unlocked state doesn't allow to be changed
            SetState(group, RAMState.Locked);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("TriggerEventForAll")]
    public void DebugTriggerEventForAll()
    {
        foreach (RAMGroup group in ramData.ramGroups)
        {
            onRAMStateChanged.Invoke(group.type, group.state);
        }
    }
#endif
}

[System.Serializable]
public class RAMGroup
{
    public RAMType type;
    public RAMState state;
    public List<Material> materials;

    public bool SetState(RAMState newState, bool allowDelocking = false)
    {
        //Don't do anything to unlocked state
        if (!allowDelocking && state == RAMState.Unlocked)
            return false;

        if (newState == state)
            return false;

        state = newState;
        UpdateMaterials();

        Debug.Log($"Setting group {type} to {newState}");
        return true;
    }

    public void UpdateMaterials()
    {
        if (materials != null)
        {
            foreach (Material material in materials)
            {
                material.SetInteger("_RAMState", (int)state);
            }
        }
    }
}

public enum RAMType
{
    Tilemap,
    Character,
    Enemy0,
    Enemy1,
    Enemy2,
    NPC,
    DigitBlock,
    FX,
}

public enum RAMState { Locked, Hovered, Unlocked }