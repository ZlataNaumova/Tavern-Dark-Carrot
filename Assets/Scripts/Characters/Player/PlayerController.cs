using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private int attackDelay;
    [SerializeField] private int attackDamage;
    [SerializeField] private GameObject glassOfBeer;
    [SerializeField] private GameObject kegOfBeer;

    public CharacterController controller;
    public PlayerInputActions playerControls;
    public Transform camera;

    private InputAction move;
    private InputAction fire;
    private InputAction interact;

    private bool attackBlocked;
    private GameObject target;
    private Vector2 moveDirection = Vector2.zero;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public bool isHoldingBeerKeg = false;
    public bool isHoldingGlassOfBeer = false;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;

        kegOfBeer.SetActive(false);
        glassOfBeer.SetActive(false);
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        interact.Disable();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveDirection.x, 0f, moveDirection.y).normalized;
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
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
