using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{

    [Header("Rendering")]
    [SerializeField] Transform weaponHolder;
    SpriteRenderer spriteRenderer;
    Vector3 weaponPos;
    [SerializeField] float downWeaponX;
    [SerializeField] float upWeaponX;

    [Header("Fire")]
    [SerializeField] Projectile projectile;
    [SerializeField] Transform fireOrigin;
    InputMap inputMap;
    public float fireInterval;
    float lastFireTime;
    bool CanFire => Time.time>= lastFireTime + fireInterval;

    GameObject parentCharacter;
    Animator anim;
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        inputMap = new InputMap();
        inputMap.Enable();
        //inputMap.Gameplay.Fire.performed += OnFire;

        parentCharacter = GetComponentInParent<PlayerMovement>().gameObject;

        anim = GetComponentInChildren<Animator>();
        anim.speed = 1f / 6f;

        health = GetComponentInParent<Health>();
        health.onDie.AddListener(OnDie);

        RAMManager.Instance.onRAMModeEnabled += OnRAMModeEnabled;
    }

    private void OnRAMModeEnabled(bool isOn)
    {
        //freeze ability to shoot right after ram mode
        if(isOn)
            lastFireTime =Time.time;
    }

    private void OnDie()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (RAMManager.Instance.isInRAMMode)
            return;
        RotateWeapon();
        UpdateSprite();
        if (inputMap.Gameplay.Fire.IsPressed())
            Fire();
    }

    void RotateWeapon()
    {
        transform.rotation = Quaternion.Euler(0, 0, CharacterLookDirection.Instance.lookAngle);
    }

    void UpdateSprite()
    {
        //Look Vertical for offset and rendering order
        switch (CharacterLookDirection.Instance.LookDirectionVertical)
        {
            case Direction.Up:
                spriteRenderer.sortingOrder = -1;
                weaponPos.x = upWeaponX;
                break;
            case Direction.Down:
            default:
                spriteRenderer.sortingOrder = 1;
                weaponPos.x = downWeaponX;
                break;
        }

        weaponHolder.localPosition = weaponPos;

        //Look Horizontal for flip
        switch (CharacterLookDirection.Instance.LookDirectionHorizontal)
        {
            case Direction.Right:
            default:
                spriteRenderer.flipY = false;
                break;
            case Direction.Left:
                spriteRenderer.flipY = true;
                break;
        }
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        Fire();
    }

    void Fire()
    {
        if (!CanFire || health.isDead)
            return;

        if (RAMManager.Instance.isInRAMMode)
            return;

        Invoke("DelayedGenerateProjectile", 0.05f);
        lastFireTime = Time.time;
        anim.SetTrigger("Fire");

        SFXManager.PlaySound(GlobalSFX.Fire);
    }

    void DelayedGenerateProjectile()
    {
        Projectile newProj = Instantiate(projectile, null);
        newProj.transform.position = fireOrigin.position;
        newProj.Fire(parentCharacter, transform.right);
    }
}
