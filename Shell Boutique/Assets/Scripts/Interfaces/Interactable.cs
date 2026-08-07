using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour {
  [SerializeField] private Canvas popupUI;
  [SerializeField] private InputActionReference interactAction;
  private bool isEnabled = false;
  private bool isInside = false;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) return;

    // create "Interact" UI

    interactAction.action.Enable();
    interactAction.action.performed += OnPerformed;
    isEnabled = true;
    popupUI.enabled = true;
    isInside = true;
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) return;
    
    interactAction.action.performed -= OnPerformed;
    interactAction.action.Disable();
    isEnabled = false;
    popupUI.enabled = false;
    isInside = false;
  }

  public abstract void OnInteract();

  // void Update()
  // {
  //   if (!isEnabled) return;

  //   if (interactAction.action.WasPressedThisFrame())
  //     OnInteract();
  // }

  void OnPerformed(InputAction.CallbackContext ctx)
  {
    if (isInside)
      OnInteract();
  }
}