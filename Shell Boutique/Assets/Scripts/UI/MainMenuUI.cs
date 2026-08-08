using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private InputActionReference startAction;
    [SerializeField] private float pause = 1f;

    void OnEnable()
    {
        startAction.action.Enable();
        startAction.action.performed += PrepareGame;
        startAction.action.canceled += PrepareGame;
        
    } 
    void OnDisable()
    {
        startAction.action.performed -= PrepareGame;
        startAction.action.canceled -= PrepareGame;
        startAction.action.Disable();
    } 

    private void PrepareGame(InputAction.CallbackContext ctx)
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(pause); 
        Debug.LogWarning("Add SFX here");
        SceneManager.LoadScene("Level");
    }

}
