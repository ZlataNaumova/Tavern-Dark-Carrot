using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int attackDelay;
    [SerializeField] private int attackDamage;
    [SerializeField] private GameObject glassOfBeer;
    [SerializeField] private GameObject kegOfBeer;

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

    public bool isHoldingBeerKeg;
    public bool isHoldingGlassOfBeer;

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

        kegOfBeer.SetActive(false);
        glassOfBeer.SetActive(false);

        TavernEventsManager.DayStarted += DayStartsHandler;
        TavernEventsManager.NightStarted += NightStartsHandler;

        isHoldingBeerKeg = false;
        isHoldingGlassOfBeer = false;

}

private void OnDisable()
    {
        movement.Disable();
        fire.Disable();
        interaction.Disable();

        TavernEventsManager.DayStarted -= DayStartsHandler;
        TavernEventsManager.NightStarted -= NightStartsHandler;
    }

    private void Start()
    {
        speed = GameConfigManager.PlayerSpeed;
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
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
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

    public void GetBeerKeg()
    {
        isHoldingBeerKeg = true;
        kegOfBeer.SetActive(true);
    }

    public void ReleaseBeerKeg()
    {
        isHoldingBeerKeg = false;
        kegOfBeer.SetActive(false);
    }

    public void GetGlassOfBeer()
    {
        isHoldingGlassOfBeer = true;
        glassOfBeer.SetActive(true);
    }

    public void SellGlassOfBeer()
    {
        isHoldingGlassOfBeer = false;
        glassOfBeer.SetActive(false);
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        Debug.Log(target.name);
    }

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
