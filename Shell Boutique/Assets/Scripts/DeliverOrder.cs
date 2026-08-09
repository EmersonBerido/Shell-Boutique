using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Order
{
    public enum ShellColors {None, Red, Blue, Yellow, Green, Purple, Orange}
    public ShellObject shell;
    public ShellColors color;
}

public class DeliverOrder : Interactable
{
    public static DeliverOrder Instance {get; private set;}

    [Header("Verification")]
    [SerializeField] private Color red;
    [SerializeField] private Color blue;
    [SerializeField] private Color yellow;
    private Color orange;
    private Color green;
    private Color purple;

    public List<Order> orders = new();
    [SerializeField] private AudioClip audioClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Instance found");
            Destroy(gameObject);
            return;
        } else
        {
            Instance = this;
        }
    }
    void Start()
    {
        orange = MixColor(red, yellow);
        green = MixColor(yellow, blue);
        purple = MixColor(red, blue);
    }

    public override void OnInteract()
    {
        if (Equipment.Instance.GetEquipped() == null || Equipment.Instance.GetEquipped().GetType() != typeof(ShellObject))
            return;

        // check all orders for a matching one
        ShellObject received = (ShellObject) Equipment.Instance.GetEquipped();
        Order matchedOrder = null;
        foreach (var order in orders)
        {
            if (!VerifyShell(order.shell, received) || !VerifyColor(order, received))
                continue;
            matchedOrder = order;
            break;
        }

        if (matchedOrder != null)
        {
            Debug.Log("Found a correct order");
            Debug.LogWarning("Still need to remove completed order from UI");
            Equipment.Instance.Unequip();
            GetOrder.Instance.FinishedOrder();
            OrderUI.Instance.RemoveOrder(matchedOrder);
            orders.Remove(matchedOrder);
            AudioSource.PlayClipAtPoint(audioClip, transform.position, 8f);  
        } else 
            Debug.Log("Found no matching orders");
        
    }

    public void AddOrder(Order newOrder)
    {
        if (newOrder == null)
            return;

        orders.Add(newOrder);
    }

    public bool OrdersRemain() => orders.Count > 0;

    private bool VerifyShell(ShellObject order, ShellObject received)
    {
        if (!order.Equals(received))
        {
            Debug.Log("Order not the same");
            return false;
        }

        Debug.Log("Order correct");
        return true;
    }

    
    private Color MixColor(Color a, Color b)
    {
        return new Color(
            a.r * b.r,
            a.g * b.g,
            a.b * b.b
        );
    }
    private bool VerifyColor(Order req, ShellObject shell)
    {
        Color color = GetColor(req.color);

        // approximate color closeness
        if (Vector3.SqrMagnitude(new Vector3(
            color.r - shell.currColor.r,
            color.g - shell.currColor.g,
            color.b - shell.currColor.b
        )) < 0.02f * 0.02f)
            return true;

        return false;
    }

    private Color GetColor(Order.ShellColors color)
    {
        return color switch
        {
            Order.ShellColors.None => Color.white,
            Order.ShellColors.Red => red,
            Order.ShellColors.Purple => purple,
            Order.ShellColors.Yellow => yellow,
            Order.ShellColors.Blue => blue,
            Order.ShellColors.Green => green,
            Order.ShellColors.Orange => orange,
            _ => Color.white
                
        };
    }
}
