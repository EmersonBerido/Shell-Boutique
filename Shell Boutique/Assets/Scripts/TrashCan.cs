using UnityEngine;

public class TrashCan : Interactable
{
    [SerializeField] private AudioClip audioClip;
    public override void OnInteract()
    {
        Equipment.Instance.Unequip();
        AudioSource.PlayClipAtPoint(audioClip, transform.position);  
    }
}
