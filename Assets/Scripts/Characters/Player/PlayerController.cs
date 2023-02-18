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
    public PlayerInputActions playerControls;
    public Transform mainCamera;

    private InputAction move;
    private InputAction fire;
    private InputAction interact;

    private bool attackBlocked;
    private GameObject target;
    private Vector2 moveDirection = Vector2.zero;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public bool isHoldingBeerKeg;
    public bool isHoldingGlassOfBeer;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        fire = playerControls.Player.Fire;
        fire.performed += Fire;
        interact = playerControls.Player.Interact;
        interact.performed += Interact;

        interact.Enable();
        fire.Enable();
        move.Enable();

        kegOfBeer.SetActive(false);
        glassOfBeer.SetActive(false);

        TavernEventsManager.DayStarts += DayStartsHandler;
        TavernEventsManager.NightStarts += NightStartsHandler;

        isHoldingBeerKeg = false;
        isHoldingGlassOfBeer = false;

}

private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        interact.Disable();

        TavernEventsManager.DayStarts -= DayStartsHandler;
        TavernEventsManager.NightStarts -= NightStartsHandler;
    }

    private void Start()
    {
        speed = GameConfigManager.PlayerSpeed;
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
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
        move.Disable();
        fire.Disable();
        interact.Disable();
    }

    private void DayStartsHandler()
    {
        interact.Enable();
        fire.Enable();
        move.Enable();
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
            if(target.TryGetComponent(out PlayerInterractible interractible))
            {
                interractible.PlayerInterraction();
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
