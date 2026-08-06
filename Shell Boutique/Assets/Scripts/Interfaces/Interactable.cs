using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour {
  [SerializeField] private Canvas popupUI;
  [SerializeField] private InputActionReference interactAction;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) return;

    // create "Interact" UI
    Debug.LogWarning("Interact with this object");

    interactAction.action.Enable();
    popupUI.enabled = true;
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (!other.CompareTag("Player")) return;
    
    interactAction.action.Disable();
    popupUI.enabled = false;
  }

  public abstract void OnInteract();

  void Update()
  {
    if (!interactAction.action.enabled) return;

    if (interactAction.action.WasPressedThisFrame())
      OnInteract();
  }
}