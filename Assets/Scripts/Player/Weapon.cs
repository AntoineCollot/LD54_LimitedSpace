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

    IAnimable animable;
    GameObject parentCharacter;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animable = GetComponentInParent<IAnimable>();

        inputMap = new InputMap();
        inputMap.Enable();
        inputMap.Gameplay.Fire.performed += OnFire;

        parentCharacter = GetComponentInParent<PlayerMovement>().gameObject;

        anim = GetComponentInChildren<Animator>();
        anim.speed = 1f / 6f;
    }

    // Update is called once per frame
    void Update()
    {
        RotateWeapon();
        UpdateSprite();
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
        Invoke("DelayedGenerateProjectile", 0.05f);

        anim.SetTrigger("Fire");
    }

    void DelayedGenerateProjectile()
    {
        Projectile newProj = Instantiate(projectile, null);
        newProj.transform.position = fireOrigin.position;
        newProj.Fire(parentCharacter, transform.right);
    }
}
