using UnityEngine;
using UnityEngine.UIElements;

public class OrderComponent : VisualElement
{
  public OrderComponent()
  {
    var tree = Resources.Load<VisualTreeAsset>("UI/Order");
    tree.CloneTree(this);
  }
}