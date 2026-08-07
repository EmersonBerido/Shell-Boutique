using UnityEngine;
using System.Collections.Generic;

/*
    Take up to 2 colors and mix them together
    Pass in a shell GameObject and change the spriterenderer's color to the mixed color
*/
public class DyeShell : Interactable
{
    private List<DyeObject> heldDyes;
    private ShellObject heldShell;
    [SerializeField] int maxDyeCount = 2;

    void Start()
    {
        heldDyes = new();
    }
  public override void OnInteract()
  {
    if (Equipment.Instance.GetEquipped() != null) {
        // add to held
        Debug.Log("Adding to held");
        if (Equipment.Instance.GetEquipped().GetType() == typeof(DyeObject))
            HoldDye((DyeObject)Equipment.Instance.GetEquipped());
        else if (Equipment.Instance.GetEquipped().GetType() == typeof(ShellObject))
            HoldShell((ShellObject)Equipment.Instance.GetEquipped());

    } else if (Equipment.Instance.GetEquipped() == null && heldDyes.Count != 0 && heldShell != null) {
        // Receive Dyed Item
        Debug.Log("Receiving Dyed Shell");
        Color color = heldDyes.Count == 1 ? 
            heldDyes[0].color : 
            MixColor(heldDyes[0].color, heldDyes[1].color);

        Equipment.Instance.Equip(heldShell, color);

        heldShell = null;
        heldDyes.Clear();
    }
  }

    private void HoldDye(DyeObject dye)
    {
        if (heldDyes.Count >= maxDyeCount)
            return;
        
        heldDyes.Add(dye);
        Equipment.Instance.Unequip();
    }

    private void HoldShell(ShellObject shell)
    {
        if (heldShell != null)
            return;
        
        heldShell = shell;
        Equipment.Instance.Unequip();
    }

    private Color MixColor(Color a, Color b)
    {
        return new Color(
            a.r * b.r,
            a.g * b.g,
            a.b * b.b
        );
    }

}
