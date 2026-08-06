using UnityEngine;

public class Equipment : MonoBehaviour
{
  public static Equipment Instance {get; private set;}
  private ScriptableObject equipped;

  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    } else
    {
      Instance = this;
    }
  }

  public ScriptableObject GetEquipped() => equipped;
  public void Equip(ScriptableObject obj)
  {
    if (equipped != null) return;
    if (obj == null) return;

    equipped = obj;
    Debug.Log($"Equipped a object");
  }
  public void Unequip()
  {
    equipped = null;
  }
}