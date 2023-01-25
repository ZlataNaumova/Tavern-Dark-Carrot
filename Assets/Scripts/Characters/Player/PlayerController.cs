using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public PlayerInputActions playerControls;

    private InputAction move;
    private InputAction fire;
    private InputAction interact;


    [SerializeField] private float speed = 6f;
    [SerializeField] private int attackDelay;
    [SerializeField] private int attackDamage;

    private bool attackBlocked;
    private Transform target;
    private Vector2 moveDirection = Vector2.zero;



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

    }
    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        if(moveDirection.magnitude >= 0.1f)
        {
            controller.Move(new Vector3(moveDirection.x,0f,moveDirection.y) * speed * Time.deltaTime);
        }
    }

    public void Fire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (attackBlocked || target == null)
            {
                return;
            }
            attackBlocked = true;
            StartCoroutine(DelayAttack());
            
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Interact");
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBlocked = false;
    }
}
