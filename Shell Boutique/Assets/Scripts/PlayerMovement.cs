using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private float baseMoveSpeed = 5;
  private float moveSpeed;
  [SerializeField] private InputActionReference moveAction;
  private Rigidbody2D rb;
  private Vector2 moveInput;

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    moveSpeed = baseMoveSpeed;
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

  private void OnMove(InputAction.CallbackContext ctx)
  {
    moveInput = ctx.ReadValue<Vector2>();
  }

  private void FixedUpdate()
  {
    rb.linearVelocity = moveInput * moveSpeed; 
  }
}