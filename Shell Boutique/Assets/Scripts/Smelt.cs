using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Smelt : Interactable
{
    [SerializeField] private List<ShellObject> shellRecipes;
    List<MaterialObject> itemsHeld;

    public List<MaterialObject> testInventory;
    [SerializeField] private ShellObject failedShell;

    [Header("Smelting")]
    [SerializeField] private float smeltTime = 8f;
    private bool isSmelting = false;
    private bool isHoldingShell = false;

    [Header("Indicators")]
    [SerializeField] private SpriteRenderer indicatorSR;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite finishedSprite;

    void Start()
    {
        itemsHeld = new();

        /* TESTING */
        // itemsHeld = testInventory;
        // if (CreateShell() != null)
        //     Debug.Log("Sucessfully crafted object");
        // else
        //     Debug.LogWarning("Failed to smelt object");
    }
    public override void OnInteract()
    {
        Debug.Log("Interacted with furncase");
        if (isSmelting)
            return;

        // if player holding item, smelt, else add to list
        if (Equipment.Instance.GetEquipped() == null && isHoldingShell)
        {
            // collect shell
            ShellObject shell = CreateShell();
            if (shell == null)
                shell = ScriptableObject.Instantiate(failedShell);
            Equipment.Instance.Equip(shell);
            indicatorSR.enabled = false;
            isHoldingShell = false;

        } else if (Equipment.Instance.GetEquipped() == null && itemsHeld.Count > 0)
        {
            // start creating shell
            StartCoroutine(StartSmelting());

        } else if (Equipment.Instance.GetEquipped() != null)
        {
            // add materials to itemsHeld
            if (Equipment.Instance.GetEquipped().GetType() != typeof(MaterialObject))
                return;

            itemsHeld.Add((MaterialObject) Equipment.Instance.GetEquipped());
            Equipment.Instance.Unequip();

        }
        // add change interact UI to change the text as well
    }

    IEnumerator StartSmelting()
    {
        // smelt for few seconds
        isSmelting = true;
        indicatorSR.enabled = true;
        indicatorSR.sprite = activeSprite;
        yield return new WaitForSeconds(smeltTime);

        indicatorSR.sprite = finishedSprite;
        isSmelting = false;
        isHoldingShell = true;
    }

    private ShellObject CreateShell()
    {
        if (itemsHeld.Count == 0) return null;

        // get material count for each material
        Dictionary<string, int> matsUsed = new();
        foreach (var mat in itemsHeld)
        {
            if (!matsUsed.ContainsKey(mat.material))
                matsUsed[mat.material] = 1;
            else
                matsUsed[mat.material] += 1;
        }

        ShellObject shellCreated = null;
        foreach (var shell in shellRecipes)
        {
            // check if each material in recipe matches with items held
            int matTotal = 0;
            bool isMatch = false;
            foreach(MaterialAmount x in shell.recipe)
            {
                matTotal += x.amount; 
                if (!matsUsed.ContainsKey(x.material.material) || matsUsed[x.material.material] != x.amount)
                {
                    isMatch = false;
                    break;
                }

                isMatch = true;
            }

            if (matTotal != itemsHeld.Count) continue;
            if (!isMatch) continue;

            shellCreated = shell;
        }

        if (shellCreated != null)
            Debug.Log("Sucessfully crafted object");
        else
            Debug.LogWarning("Failed to smelt object");
        itemsHeld.Clear();
        return shellCreated;
    }


}
