using UnityEngine;
using UnityEngine.UI;
public class Equipment : MonoBehaviour
{
  public static Equipment Instance {get; private set;}
  private ScriptableObject equipped;
  [SerializeField] private SpriteRenderer sr;

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
  public void Equip(ScriptableObject obj, Color color = default)
  {
    if (equipped != null) return;
    if (obj == null) return;
    if (color == default) color = Color.white;

    equipped = obj;
    sr.enabled = true;
    sr.color = color;

    // set sprite
    if (obj.GetType() == typeof(MaterialObject))
    {
      MaterialObject newObj = (MaterialObject)obj;
      sr.sprite = newObj.sprite;
    } else if (obj.GetType() == typeof(DyeObject))
    {
      DyeObject newObj = (DyeObject)obj;
      sr.sprite = newObj.sprite;
    } else if (obj.GetType() == typeof(ShellObject))
    {
      ShellObject newObj = (ShellObject)obj;
      sr.sprite = newObj.sprite;
    } else return;

  }
  public void Unequip()
  {
    equipped = null;

    sr.enabled = false;
  }
}