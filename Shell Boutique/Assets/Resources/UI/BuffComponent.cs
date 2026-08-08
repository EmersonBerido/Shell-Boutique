using UnityEngine;
using UnityEngine.UIElements;

public class BuffComponent : VisualElement
{
  public BuffComponent()
  {
    var tree = Resources.Load<VisualTreeAsset>("UI/Buff");
    tree.CloneTree(this);
  }
}