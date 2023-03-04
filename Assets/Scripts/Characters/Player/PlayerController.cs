using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int attackDelay;
    [SerializeField] private int attackDamage;
    [SerializeField] private Sprite redBeerGlassSprite;
    [SerializeField] private Sprite greenBeerGlassSprite;
    [SerializeField] private Sprite greenBeerBarrelSprite;
    [SerializeField] private Sprite redBeerBarrelSprite;
    [SerializeField] private Image currentItem;

    public CharacterController controller;
    public PlayerInputActions playerInputActions;
    public Transform mainCamera;

    private InputAction movement;
    private InputAction fire;
    private InputAction interaction;

    private bool attackBlocked;
    private GameObject target;
    private Vector2 moveDirection = Vector2.zero;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private int currentBeerType;

    public bool isHoldingBeerKeg;
    public bool isHoldingGlassOfBeer;

    public int CurrentBeerType => currentBeerType;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Move;
        fire = playerInputActions.Player.Fire;
        fire.performed += Fire;
        interaction = playerInputActions.Player.Interact;
        interaction.performed += Interact;

        interaction.Enable();
        fire.Enable();
        movement.Enable();
      
        currentItem.enabled = false;

        TavernEventsManager.OnDayStarted += DayStartsHandler;
        TavernEventsManager.OnNightStarted += NightStartsHandler;

        isHoldingBeerKeg = false;
        isHoldingGlassOfBeer = false;
    }

private void OnDisable()
    {
        movement.Disable();
        fire.Disable();
        interaction.Disable();

        TavernEventsManager.OnDayStarted -= DayStartsHandler;
        TavernEventsManager.OnNightStarted -= NightStartsHandler;
    }

    void Update()
    {
        moveDirection = movement.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveDirection.x, 0f, moveDirection.y).normalized;
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * GameConfigManager.PlayerSpeed * Time.deltaTime);
            //controller.Move(direction * GameConfigManager.PlayerSpeed * Time.deltaTime);
        }
    }

    private void NightStartsHandler()
    {
        movement.Disable();
        fire.Disable();
        interaction.Disable();
    }

    private void DayStartsHandler()
    {
        interaction.Enable();
        fire.Enable();
        movement.Enable();
    }

    public void TakeBeerKeg(int beerType)
    {
        currentItem.enabled = true;
        currentItem.sprite = beerType == 1 ? greenBeerBarrelSprite : redBeerBarrelSprite;
        isHoldingBeerKeg = true;
        //kegOfBeer.SetActive(true);
        currentBeerType = beerType;
    }

    public void ReleaseBeerKeg()
    {
        isHoldingBeerKeg = false;
        //kegOfBeer.SetActive(false);
        currentItem.enabled = false;
    }

    public void TakeGlassOfBeer(int beerType)
    {
        currentItem.enabled = true;
        currentItem.sprite = beerType == 1 ? greenBeerGlassSprite : redBeerGlassSprite;
        isHoldingGlassOfBeer = true;

    }

    public void SellGlassOfBeer()
    {
        isHoldingGlassOfBeer = false;
        currentItem.enabled = false;
    }

    public void SetTarget(GameObject newTarget) => target = newTarget;
  
    public void Fire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (attackBlocked || target == null)
            {
                return;
            }
            Debug.Log("Attack");
            attackBlocked = true;
            StartCoroutine(DelayAttack());
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && target != null)
        {
            if(target.TryGetComponent(out PlayerInteractable interactable))
            {
                interactable.PlayerInteraction();
            }
        }
        else
        {
            Debug.Log("No Target to interract");
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBlocked = false;
    }
}
