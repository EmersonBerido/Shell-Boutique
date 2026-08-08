using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour {
  [SerializeField] private Canvas popupUI;
  [SerializeField] private InputActionReference interactAction;
  private bool isInside = false;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) return;

    interactAction.action.Enable();
    interactAction.action.performed += OnPerformed;
    popupUI.enabled = true;
    isInside = true;
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) return;
    
    interactAction.action.performed -= OnPerformed;
    interactAction.action.Disable();
    popupUI.enabled = false;
    isInside = false;
  }

  public abstract void OnInteract();

  void OnPerformed(InputAction.CallbackContext ctx)
  {
    if (isInside)
      OnInteract();
  }
}