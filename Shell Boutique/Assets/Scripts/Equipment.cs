using UnityEngine;
using UnityEngine.UI;
public class Equipment : MonoBehaviour
{
  public static Equipment Instance {get; private set;}
  private ScriptableObject equipped;
  [SerializeField] private Canvas UI;
  [SerializeField] private Image image;

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
    UI.enabled = true;

    // set sprite
    if (obj.GetType() == typeof(MaterialObject))
    {
      MaterialObject newObj = (MaterialObject)obj;
      image.sprite = newObj.sprite;
    } else if (obj.GetType() == typeof(DyeObject))
    {
      DyeObject newObj = (DyeObject)obj;
      image.sprite = newObj.sprite;
    } else if (obj.GetType() == typeof(ShellObject))
    {
      ShellObject newObj = (ShellObject)obj;
      image.sprite = newObj.sprite;
    } else return;

  }
  public void Unequip()
  {
    equipped = null;

    UI.enabled = false;
  }
}