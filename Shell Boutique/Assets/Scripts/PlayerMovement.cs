using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
  public static PlayerMovement Instance {get; private set;}
  [SerializeField] private float baseMoveSpeed = 5;
  private float moveSpeed;
  [SerializeField] private InputActionReference moveAction;
  private Rigidbody2D rb;
  private Vector2 moveInput;
  [SerializeField] private Animator animator;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    moveSpeed = baseMoveSpeed;

    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    } else
    {
      Instance = this;
    }
  }

  void OnEnable()
  {
    moveAction.action.Enable();
    moveAction.action.performed += OnMove;
    moveAction.action.canceled += OnMove;
    
  }
  void OnDisable()
  {
    moveAction.action.performed -= OnMove;
    moveAction.action.canceled -= OnMove;
    moveAction.action.Disable();
    
  }

  public void UpdateSpeed(float newSpeed)
  {
    moveSpeed = newSpeed;
  }
  public float GetInitialSpeed() => baseMoveSpeed;

  private void OnMove(InputAction.CallbackContext ctx)
  {
    if (ctx.canceled)
    {
      moveInput = Vector2.zero;
      animator.SetBool("isRunning", false);
      return;
    }
    
    moveInput = ctx.ReadValue<Vector2>();
    if (moveInput.x < 0)
      GetComponent<SpriteRenderer>().flipX = true;
    else 
      GetComponent<SpriteRenderer>().flipX = false;

    animator.SetBool("isRunning", true);
  }

  private void FixedUpdate()
  {
    rb.linearVelocity = moveInput * moveSpeed; 

  }
}