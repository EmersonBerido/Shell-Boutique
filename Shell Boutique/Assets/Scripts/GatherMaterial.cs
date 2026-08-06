using UnityEngine;

/*
    After entering the hit box, prompt user to hit action button
    Give material immediately if not holding an item
*/
public class GatherMaterial : Interactable
{
    [SerializeField] private ScriptableObject material;

    public override void OnInteract()
    {
        // check if player has object equipped
        if (Equipment.Instance.GetEquipped() != null) return;

        // create copy of obj
        ScriptableObject obj = Instantiate(material);

        // give obj to player equipped
        Equipment.Instance.Equip(obj);
    }
}
