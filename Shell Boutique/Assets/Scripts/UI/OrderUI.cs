using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class OrderUI : MonoBehaviour
{
    public static OrderUI Instance {get; private set;}
    [SerializeField] private UIDocument uiDocument;

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

    public void AddOrder(Order order)
    {
        var root = uiDocument.rootVisualElement;
        var panel = root.Q<VisualElement>("Panel");
        var container = panel.Q<VisualElement>("OrdersContainer");

        var orderParent = new OrderComponent();
        var orderContainer = orderParent.Q<VisualElement>("Order");
        orderParent.style.height = new Length(100, LengthUnit.Percent);
        orderContainer.Q<Image>("Image").tintColor = GetColor(order.color);
        FillMaterials(orderContainer, order.shell.recipe);
        
        container.Add(orderParent);
    }

    public void RemoveOrder(Order order)
    {
        var container = uiDocument.rootVisualElement.
            Q<VisualElement>("Panel").
            Q<VisualElement>("OrdersContainer");

        foreach (OrderComponent curr in container.Query<OrderComponent>().ToList())
        {
            var c = curr.Q<VisualElement>("Order");
            if (VerifyMaterials(c, order) && VerifyColor(c, order))
                container.Remove(curr);
        }
    }

    private bool VerifyMaterials(VisualElement container, Order order)
    {
        if (container == null || order == null)
            return false;
        
        // Edge Case
        string cleanedMats = container.Q<Label>("Materials").text.
            Replace("\n"," ").Replace("\r", " ");
        string cleanedAmts = container.Q<Label>("Amount").text.
            Replace("\n"," ").Replace("\r", " ");
        List<string> matsParts = cleanedMats.Split(" ").ToList();
        matsParts = matsParts.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        List<string> amountParts = cleanedAmts.Split(" ").ToList();
        amountParts = amountParts.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        if (matsParts.Count != amountParts.Count) return false;

        // Create dictionary for fast checkup
        Dictionary<string, int> containerParts = new();
        int containerTotalAmount = 0;
        for (int i = 0; i < matsParts.Count; i++)
        {
            containerParts[matsParts[i]] = int.Parse(amountParts[i]);
            containerTotalAmount += int.Parse(amountParts[i]);
        }

        // compare materials and amount
        int orderTotalAmount = 0;
        foreach (MaterialAmount curr in order.shell.recipe)
        {
            orderTotalAmount += curr.amount;

            if (!containerParts.ContainsKey(curr.material.material))
                return false;

            if (containerParts[curr.material.material] != curr.amount)
                return false;
        }

        if (orderTotalAmount != containerTotalAmount)
            return false;
        return true;
    }

    private bool VerifyColor(VisualElement container, Order order)
    {
        if (GetShellColor(container.Q<Image>("Image").tintColor) == order.color) 
            return true;
        return false;
    }

    private void FillMaterials(VisualElement container, List<MaterialAmount> materials)
    {
        if (container == null || materials == null || materials.Count == 0)
            return;

        var types = container.Q<Label>("Materials");
        var amounts = container.Q<Label>("Amount");

        types.text = "";
        amounts.text = "";

        foreach (var item in materials)
        {
            types.text += $"{item.material.material}\n";
            amounts.text += $"{item.amount}\n";
        }
    }

    private Color GetColor(Order.ShellColors color)
    {
        return color switch
        {
            Order.ShellColors.None => Color.white,
            Order.ShellColors.Red => Color.red,
            Order.ShellColors.Purple => Color.purple,
            Order.ShellColors.Yellow => Color.yellow,
            Order.ShellColors.Blue => Color.blue,
            Order.ShellColors.Green => Color.green,
            Order.ShellColors.Orange => Color.orange,
            _ => Color.white
                
        };
    }

    private Order.ShellColors GetShellColor(Color color)
    {
        if (VerifyColor(color, Color.red))    return Order.ShellColors.Red;
        if (VerifyColor(color, Color.yellow)) return Order.ShellColors.Yellow;
        if (VerifyColor(color, Color.blue))   return Order.ShellColors.Blue;
        if (VerifyColor(color, Color.green))  return Order.ShellColors.Green;
        if (VerifyColor(color, Color.purple)) return Order.ShellColors.Purple;
        if (VerifyColor(color, Color.orange)) return Order.ShellColors.Orange;

        return Order.ShellColors.None;
    }

    private bool VerifyColor(Color colorA, Color colorB)
    {
        
        // approximate color closeness
        if (Vector3.SqrMagnitude(new Vector3(
            colorA.r - colorB.r,
            colorA.g - colorB.g,
            colorA.b - colorB.b
        )) < 0.02f * 0.02f)
            return true;

        return false;
    }

}
