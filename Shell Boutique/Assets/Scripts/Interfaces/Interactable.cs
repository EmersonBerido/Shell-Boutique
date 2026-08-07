using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour {
  [SerializeField] private Canvas popupUI;
  [SerializeField] private InputActionReference interactAction;
  private bool isEnabled = false;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) return;

    // create "Interact" UI
    Debug.LogWarning("Interact with this object");

    interactAction.action.Enable();
    isEnabled = true;
    popupUI.enabled = true;
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) return;
    
    interactAction.action.Disable();
    isEnabled = false;
    popupUI.enabled = false;
  }

  public abstract void OnInteract();

  void Update()
  {
    if (!isEnabled) return;

    if (interactAction.action.WasPressedThisFrame())
      OnInteract();
  }
}