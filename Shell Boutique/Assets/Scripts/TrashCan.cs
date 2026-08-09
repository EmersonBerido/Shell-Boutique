using UnityEngine;

public class TrashCan : Interactable
{
    public override void OnInteract()
    {
        Equipment.Instance.Unequip();
    }
}
